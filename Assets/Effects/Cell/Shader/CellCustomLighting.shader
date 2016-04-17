Shader "Custom/Sprite/Cell-Custom-Lighting" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_BorderFreq("Border Frequency", Range(0.0, 100.0)) = 10.0
		_BorderAmp("Border Amplitude", Range(0.0, 0.1)) = 0.005
		_BorderSpeed("Border Speed", Range(0.0, 10000.0)) = 4.0
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_MaskTex("Mask texture", 2D) = "white" {}
	// refraction thing
		_Refraction("Refraction", Range(0.00, 100.0)) = 1.0
		_Speed("Distort. Speed", Float) = 0.2
		_Freq("Distort. Freq", Float) = 1.0
		_Amp("Distort. Amp", Float) = 1.0
		_DistortTex("Distort (RGB)", 2D) = "white" {}
		// Lighting
		_LightColor("Light Color", Color) = (1,1,1,1)
		_LightDistanceMax("Light Distance Max.", Float) = 10.0
		_NormalTex("Normal Map", 2D) = "white" {}
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "Queue" = "Overlay"/* "RenderType" = "Transparent"*/ }
		LOD 200
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		GrabPass {}

		Pass {
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
			};

			sampler2D _MainTex;
			sampler2D _MaskTex;
			fixed4 _Color;
			// Border stuff
			float _BorderFreq;
			float _BorderAmp;
			float _BorderSpeed;
			// refraction thing
			float4 _GrabTexture_TexelSize;
			float4 _DistortTex_ST;
			float _Refraction;
			float _Speed;
			float _Freq;
			float _Amp;
			sampler2D _GrabTexture : register(s0);
			sampler2D _DistortTex : register(s2);
			// Lighting
			sampler2D _NormalTex;
			float4 _LightColor;
			float _LightDistanceMax;

			struct Input {
				float2 uv_MainTex;
				float4 vertex;
				float4 color;
				float2 uv;
				float4 screenPos;
			};

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _DistortTex);
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
				float N = heatNoise(float3(uv.x, uv.y, _Time.y*_Speed));
				return float3(N, N, N);
			}

			float3 dispTex(float2 uv) {
				return tex2D(_DistortTex, uv*3.0f + float2(_Time.y / 40.0f, _Time.w / 40.0f)).rgb;
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
				// getting the sprite texture
				//float4 spriteColor = tex2D(_SpriteTex, i.uv);

				float2 uv = i.uv;
				float2 coord = -1.0 + 2.0*uv;
				// computing mathematical stuff for borders
				float dist = length(coord);
				float dt = dot(float2(0.0, 1.0), coord);
				float cosO = dot(float2(0.0, 1.0), coord) / dist;
				/*float angle = atan2(0.0, 1.0) - atan2(coord.y, coord.x);// acos(dot(float2(0.0, 1.0), coord) / dist);
																		// computing mathematical stuff for ovalization
																		// ovalization
				if (angle < 0.0) angle = angle + 200.0*DEG2RAD;*/
				float angle = atan2(0.0*coord.y + 1.0*coord.x, 1.0*coord.y + 0.0*coord.x);
				float2 speedV = float2(1.0, 1.0);
				float2 speedVM = float2(1.0, 1.0);
				float speed = length(speedV);
				float cosOS = dot(speedV, coord) / (dist*speed);
				float angleS = acos(dot(speedV, coord) / (dist*speed));
				float dtS = dot(speedV, coord);
				float2 offset = float2(cos(angleS)*speedV.x, sin(angleS)*speedV.y);

				float2 uvOv = float2(dtS, dtS);
				//uv = scaleUV(uv, speedV);
				//uv = uv + uvOv;
				//uv = scaleUV(uv, speedV);
				//uv = rotateUV(uv, vec2(0.0, 0.0), angleS);
				//float2 center = float2(0.0, 0.0);
				//center = scaleUV(center, speedV);
				//center = rotateUV(center, float2(0.0, 0.0), angleS);

				float _borderOffset = sin(angle*_BorderFreq + _Time.y*_BorderSpeed)*_BorderAmp;

				bool testOval = length(coord) > abs(_borderOffset);
				// border animation
				if (dist > 0.7)
					uv += _borderOffset;

				fixed4 spriteColor = tex2D(_MainTex, uv) * _Color;
				fixed4 mask = tex2D(_MaskTex, uv).a;
				//col.r = spriteColor.a;
				// getting 
				if (mask.a == 0.0f) {
					col = tex2D(_GrabTexture, i.screenPos);
				}else if(spriteColor.a==0.0) {
					float3 distort = dispTex(uv);// hash(i.uv) * float3(i.color.r, i.color.g, i.color.b);
					float2 offset = distort * _Refraction * _GrabTexture_TexelSize.xy;
					//if (!(spriteColor.r == 1.0f && spriteColor.g == 1.0f && spriteColor.b == 1.0f)) {
						i.screenPos.xy = offset * i.screenPos.z + i.screenPos.xy;
					//}
					col = tex2D(_GrabTexture, i.screenPos);
					/*col.r = 1.0;
					col.gb = 0.0;*/
				}
				else col = spriteColor;
				/*else if (spriteColor.a > 0.0f) {
					float2 grabTexcoord = i.screenPos.xy;
					fixed4 colTransparency = tex2D(_GrabTexture, grabTexcoord);

					float3 distort = dispHeat(i.uv) * float3(i.color.r, i.color.g, i.color.b);
					float2 offset = distort * _Refraction * _GrabTexture_TexelSize.xy;
					if (!(spriteColor.r == 1.0f && spriteColor.g == 1.0f && spriteColor.b == 1.0f)) {
						i.screenPos.xy = offset * i.screenPos.z + i.screenPos.xy;
					}
					fixed4 colDistort = tex2D(_GrabTexture, i.screenPos);

					col = lerp(colTransparency, colDistort, 1.0-spriteColor.a);

				}
				else {
					float2 grabTexcoord = i.screenPos.xy;
					col = tex2D(_GrabTexture, grabTexcoord);
				}*/


				//col.r = spriteColor.a;
				/*col.r = spriteColor.a>0.0?1.0:0.0;
				col.g = col.b = 0.0;*/

				// Lighting
				float4 worldPos = mul(_Object2World, i.vertex);
				float3 lPos = _WorldSpaceLightPos0.xyz;
				float3 LtoD = lPos.xyz - worldPos.xyz;
				float3 EtoD =  _WorldSpaceCameraPos - worldPos.xyz;
				float3 normal = /*UnpackNormal(*/tex2D(_NormalTex, uv).rgb/*)*/;
				float Lambert = min(1.0, max(0.0, dot(-normalize(EtoD), normalize(normal))));

				//col.rgb = lerp(col.rgb, _LightColor, (1.0 - length(LtoD) / _LightDistanceMax))*max(0.0, dot(normalize(LtoD), normalize(normal)));
				col.rgb = col.rgb + Lambert * _LightColor * (1.0 - length(LtoD) / _LightDistanceMax);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
