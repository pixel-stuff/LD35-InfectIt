Shader "Custom/Sprite/Cell-Lite" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Main Tex", 2D) = "white" {}
	// Lighting
		_LightPos("Light Pos", Vector) = (0, 0, 0)
		_LightColor("Light Color", Color) = (1,1,1,1)
		_LightDistanceMax("Light Distance Max.", Float) = 10.0
		[Toggle]_IsLightActive("Is Light Active ?", Float) = 1.0
		_LightIntensity("Light Intensity", Float) = 1.0
		_NormalTex("Normal Map", 2D) = "bleu" {}
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True"/* "RenderType" = "Transparent"*/ }
		LOD 200
		Cull Off
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
				float4 screenPos : TEXCOORD2;
				float3 lPos : TEXCOORD3;
				float3 worldPos : TEXCOORD4;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			// Lighting
			sampler2D _NormalTex;
			float3 _LightPos;
			float4 _LightColor;
			float _LightDistanceMax;
			float _IsLightActive;
			float _LightIntensity;

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
				//v.vertex = v.vertex*sin(v.vertex+_Time.y);
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
				o.lPos = unity_LightPosition[0];
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;

				// Lighting
				if (_IsLightActive) {
					float3 worldPos = i.worldPos;
					float3 lPos = _LightPos;
					float3 LtoD = lPos - worldPos.xyz;
					float3 EtoD = _WorldSpaceCameraPos - worldPos.xyz;
					float3 normal = tex2D(_NormalTex, i.uv).rgb;
					float Lambert = min(1.0, max(0.0, dot(normalize(float3(0.0, 0.0, 1.0)), normalize(normal))));
					float gradient = saturate(1.0 - length(LtoD) / _LightDistanceMax);
					col.rgb = lerp(col.rgb, Lambert * _LightColor * gradient * _LightIntensity, gradient);
					//col.rgb = gradient;
				}
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
