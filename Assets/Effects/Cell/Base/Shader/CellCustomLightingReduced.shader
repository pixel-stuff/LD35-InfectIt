Shader "Custom/Sprite/Cell-Custom-Lighting-Reduced" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_BorderFreq("Border Frequency", Range(0.0, 100.0)) = 10.0
		_BorderAmp("Border Amplitude", Range(0.0, 0.1)) = 0.005
		_BorderSpeed("Border Speed", Range(0.0, 10000.0)) = 4.0
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		// Lighting
		_LightPos("Light Pos", Vector) = (0, 0, 0)
		_LightColor("Light Color", Color) = (1,1,1,1)
		_LightDistanceMax("Light Distance Max.", Float) = 10.0
		[Toggle]_IsLightActive("Is Light Active ?", Float) = 1.0
		_LightIntensity("Light Intensity", Float) = 1.0
		_NormalTex("Normal Map", 2D) = "bleu" {}
		// shifting penetration
		_AnglePenetration("Angle of Penetration", Float) = -1.0
		_DirPenetration("Dir. of Penetration", Vector) = (0, 0, 0, 0)
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True"/* "RenderType" = "Transparent"*/ }
		LOD 200
		Cull Off
		Zwrite Off
		ZTest Off
		Blend SrcAlpha OneMinusSrcAlpha

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
				float3 worldPos : TEXCOORD2;
			};

			sampler2D _MainTex;
			fixed4 _Color;
			// Border stuff
			float _BorderFreq;
			float _BorderAmp;
			float _BorderSpeed;
			float4 _MainTex_ST;
			// Lighting
			sampler2D _NormalTex;
			float3 _LightPos;
			float4 _LightColor;
			float _LightDistanceMax;
			float _IsLightActive;
			float _LightIntensity;

			// Penetration
			float _AnglePenetration;
			float4 _DirPenetration;

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				//v.vertex = v.vertex*sin(v.vertex+_Time.y);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos = mul(_Object2World, v.vertex).xyz;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			#define DEG2RAD (3.14159 / 180.0)
			#define RAD2DEG (180.0 / 3.14159)
			#define PI 3.14159

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

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0, 0, 0, 1);
				// getting the sprite texture
				//float4 spriteColor = tex2D(_SpriteTex, i.uv);

				float2 uv = i.uv;
				#ifdef SHADER_API_GLES
				float2 coord = -1.0 + 2.5*i.uv;
				#else
				float2 coord = -1.0 + 2.0*i.uv;
				#endif
				// computing mathematical stuff for borders
				float dist = length(coord-half2(0.0, 0.0));
				float2 toc = float2(0.0, 1.0);
				toc = rotateUV(float2(0.0, 1.0), float2(0.0, 0.0), _AnglePenetration);
				float dt = dot(toc, coord);
				float cosO = dot(toc, coord) / dist;
				float angle = atan2(coord.y, coord.x);
				angle = fmod(-angle, PI);// fmod(-180.0 / PI * angle, 360.0)*DEG2RAD;
				if (angle < 0.0) angle += 2.0 * PI;
				if (angle > 2.0*PI) angle -= 2.0 * PI;
				float sinAngle = sin(angle+PI/2.0);

				// penetration
				float penetrationAngleStart = (_AnglePenetration - 15.0)*DEG2RAD;
				//if (penetrationAngleStart < -15.0*DEG2RAD) penetrationAngleStart += 2.0 * PI;
				float penetrationAngleEnd = (_AnglePenetration + 15.0)*DEG2RAD;
				//if (penetrationAngleEnd < 15.0*DEG2RAD) penetrationAngleEnd += 2.0 * PI;

				float _borderOffset = sin(sinAngle*_BorderFreq + _Time.y*_BorderSpeed)*_BorderAmp;
				float alpha = 0.0;
				//if (_AnglePenetration >= 0.0 && abs(_AnglePenetration*DEG2RAD - angle) < 15.0*DEG2RAD) {
				// when the angle is _AnglePenetration it is 0 else it becomes 1
				//if(abs(_AnglePenetration*DEG2RAD - angle) / ((penetrationAngleEnd - penetrationAngleStart)*0.7)<1.0 && angle < (penetrationAngleEnd - penetrationAngleStart)*0.7) angle += _AnglePenetration-angle;
				//if (abs(_AnglePenetration*DEG2RAD - angle) / ((penetrationAngleEnd - penetrationAngleStart)*0.7) < 1.0 && angle > (360.0*DEG2RAD - (penetrationAngleEnd - penetrationAngleStart)*0.7)) angle += _AnglePenetration + angle;
				//if(_AnglePenetration*DEG2RAD - angle)
					//alpha = min(1.0, max(0.0, 1.0 - abs(_AnglePenetration*DEG2RAD - angle) / ((penetrationAngleEnd - penetrationAngleStart)*0.7)));
				float3 dirP = float3(0.0, 0.0, 0.0);
				dirP.xy = rotateUV(_DirPenetration.xy, float2(0.0f, 0.0f), _AnglePenetration);
				alpha = (max(0.0, dot(normalize(coord), normalize(dirP))) - 0.01)/2.5; // 0.99
				_borderOffset = lerp(_borderOffset, /*(angle<25.0*DEG2RAD || angle>185.0*DEG2RAD)?alpha*0.2:*/alpha, alpha);
					//_borderOffset -= (sin(sinAngle*_BorderFreq*4.0 + _Time.y*_BorderSpeed) + PI*0.5)*_BorderAmp*1.6;
				//}

				// border animation
				//if (dist > 0.7)
					uv += coord*_borderOffset;

				col = tex2D(_MainTex, uv) * _Color;
				// Lighting
				if (_IsLightActive) {
					float3 worldPos = i.worldPos;
					float3 lPos = _LightPos;
					float3 LtoD = lPos - worldPos.xyz;
					float3 EtoD = _WorldSpaceCameraPos - worldPos.xyz;
					float3 normal = tex2D(_NormalTex, uv).rgb;
					float Lambert = min(1.0, max(0.0, dot(normalize(float3(0.0, 0.0, 1.0)), normalize(normal))));
					float gradient = (1.0 - length(LtoD) / _LightDistanceMax);
					float3 debug = Lambert;
					col.rgb = lerp(col.rgb, Lambert * _LightColor * gradient * _LightIntensity, gradient);
				}
				// apply fog
				/*if (_AnglePenetration >= 0.0 && abs(_AnglePenetration*DEG2RAD - angle) < 15.0*DEG2RAD) {
					col.rg = alpha;
					col.b = 0.0;
				}*/
				/*col.rgb = (angle+3.14/2.0);
				col.r = abs(penetrationAngleStart-angle)<15.0*DEG2RAD ? 1.0 : 0.0;
				col.g = col.b = 0.0;*/
				/*if (angle-2.0*PI<(_AnglePenetration-15.0*DEG2RAD)) {
					//_AnglePenetration
					angle -= 2.0*PI;
				}
				float t = _AnglePenetration*DEG2RAD - angle;
				col.rgb = _AnglePenetration<30.0 && angle>330.0*DEG2RAD? angle-330.0*DEG2RAD :abs(t);
				UNITY_APPLY_FOG(i.fogCoord, col);*/
				return col;
			}
			ENDCG
		}
	}
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
