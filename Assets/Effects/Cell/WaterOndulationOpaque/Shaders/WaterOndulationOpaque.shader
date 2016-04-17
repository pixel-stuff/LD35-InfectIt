Shader "Custom/Sprite/WaterOndulation" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Main Tex", 2D) = "white" {}
		// refraction thing
		_Refraction("Refraction", Range(0.00, 100.0)) = 1.0
		_Speed("Speed", Float) = 0.2
		_Freq("Freq", Float) = 1.0
		_Amp("Amp", Float) = 1.0
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True"/* "RenderType" = "Transparent"*/ }
		LOD 200
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		//GrabPass{}

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
			/*float4 _GrabTexture_TexelSize;
			float4 _DistortTex_ST;
			float _Refraction;*/
			float _Speed;
			float _Freq;
			float _Amp;
			/*sampler2D _GrabTexture : register(s0);
			sampler2D _DistortTex : register(s2);*/

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
				uv.y += sin((uv.x + _Time.y*_Speed)*_Freq)*_Amp;
				/*float2 coord = -1.0 + 2.0*uv;

				float3 distort = 1.0 * float3(i.color.r, i.color.g, i.color.b);
				float2 offset = distort * _Refraction * _GrabTexture_TexelSize.xy;
				i.screenPos.xy = offset * i.screenPos.z + i.screenPos.xy;
				col = tex2D(_GrabTexture, i.screenPos);*/
				col.rgb = (tex2D(_MainTex, uv)*_Color).rgb;

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
