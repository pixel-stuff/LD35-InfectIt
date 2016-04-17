Shader "Custom/Sprite/WaterOndulation" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Main Tex", 2D) = "white" {}
		// refraction thing
		_Refraction("Refraction", Range(0.00, 100.0)) = 1.0
		_Speed("Speed", Float) = 0.2
		_Freq("Freq", Float) = 1.0
		_Amp("Amp", Float) = 1.0
		_Mask("Mask", Range(0.0, 1.0)) = 0.0
	}

	SubShader{
		Tags{ "Queue" = "Transparent"/* "RenderType" = "Transparent"*/ }
		LOD 200
		Cull Off
		Zwrite Off
		ZTest Off
		Blend SrcAlpha OneMinusSrcAlpha
		//Blend SrcAlpha One

		GrabPass{}

		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
					// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				float3 color : COLOR;
				float2 uv : TEXCOORD0;
				float3 worldRefl : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
			};

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			// refraction thing
			float4 _GrabTexture_TexelSize;
			float4 _DistortTex_ST;
			float _Refraction;
			float _Speed;
			float _Freq;
			float _Amp;
			float _Mask;
			sampler2D _GrabTexture : register(s0);
			sampler2D _DistortTex : register(s2);

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos = mul(_Object2World, v.vertex).xyz;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				o.color = v.color;

				half4 screenpos = ComputeGrabScreenPos(o.vertex);
				o.screenPos.xy = screenpos.xy / screenpos.w;
				half depth = length(mul(UNITY_MATRIX_MV, v.vertex));
				o.screenPos.z = depth;
				o.screenPos.w = depth;
				return o;
			}

			float hash(float2 uv) {
				return frac(sin(dot(uv, float2(100.3f, 10.73f)))*51.214255);
			}

			float noise(float2 uv) {
				return lerp(hash(uv + float2(-0.1f, 0.0f)), hash(uv + float2(0.1f, 0.0f)), hash(uv));
			}

			float hash3D(float3 uv) {
				return frac(sin(dot(uv, float3(100.3f, 10.73f, 1.0f)))*51.214255);
			}

			float noise3D(float3 uv) {
				float3 fl = floor(uv);
				float3 fr = frac(uv);
				return lerp(
					lerp(
						lerp(hash3D(fl + float3(0.0f, 0.0f, 0.0f)), hash3D(fl + float3(1.0f, 0.0f, 0.0f)), fr.x),
						lerp(hash3D(fl + float3(0.0f, 1.0f, 0.0f)), hash3D(fl + float3(1.0f, 1.0f, 0.0f)), fr.x),
						fr.y),
					lerp(
						lerp(hash3D(fl + float3(0.0f, 0.0f, 1.0f)), hash3D(fl + float3(1.0f, 0.0f, 1.0f)), fr.x),
						lerp(hash3D(fl + float3(0.0f, 1.0f, 1.0f)), hash3D(fl + float3(1.0f, 1.0f, 1.0f)), fr.x),
						fr.y),
					fr.z);
			}

			float perlin3D(float3 uv) {
				float total = 0;
				float p = 1.3f;
				for (int i = 0; i < 4; i++) {
					float freq = 2.0f*float(i);
					float amplitude = p*float(i);
					total += noise3D(uv*freq) * amplitude;
				}
				return total;
			}

			float heatNoise(float3 uv) {
				float h = 0.0f;
				h = perlin3D(uv);
				return h;
			}

			float3 disp(float2 uv) {
				return float3(sin((uv.y + _Time.y*_Speed)*_Freq)*_Amp, 0.0f, 0.0f);
			}

			float3 dispHeat(float2 uv) {
				float N = heatNoise((float3(uv.x, uv.y, _Time.y*_Speed) + _Time.y*_Speed)*_Freq)*_Amp;
				return float3(N, N, N);
			}

			#define DEG2RAD 3.14159 / 180.0
			/**
			* Rotate UV from angle around center
			* @author pierre.plans@gmail.com
			**/
			float2 rotateUV(in float2 uv, in float2 center, float angle) {
				float cosO = cos(angle*0.0174533);
				float sinO = sin(angle*0.0174533);
				uv = center + mul((uv - center), float2x2(cosO, -sinO, sinO, cosO));
				return uv;
			}

			/**
			* Scale UV
			* @author pierre.plans@gmail.com
			**/
			float2 scaleUV(in float2 uv, in float2 scale) {
				return uv / scale;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0, 0, 0, 1);

				float2 uv = i.uv;
				//uv.y += sin((uv.x + _Time.y*_Speed)*_Freq)*_Amp;
				float2 coord = -1.0 + 2.0*uv;

				float3 distort = dispHeat(uv) * float3(i.color.r, i.color.g, i.color.b);
				float2 offset = distort * _Refraction * _GrabTexture_TexelSize.xy;
				i.screenPos.xy = offset * i.screenPos.z + i.screenPos.xy;
				col = tex2D(_GrabTexture, i.screenPos);

				/*col.rgb = lerp(col.rgb, (tex2D(_MainTex, uv)*_Color).rgb, _Mask);
				col.r = 1.0;
				col.g = 0.0;
				col.b = 0.0;*/
				//col.a = _Alpha;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
