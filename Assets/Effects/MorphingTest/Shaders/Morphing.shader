Shader "Custom/Sprite/Morphing" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		// refraction thing
		_MainTex1("Morph1", 2D) = "white" {}
		_MainTex2("Morph2", 2D) = "white" {}
		_Speed("Speed", Float) = 0.2
		_Freq("Freq", Float) = 1.0
		_Amp("Amp", Float) = 1.0
	}

		SubShader{
		Tags{ "Queue" = "Transparent"/* "RenderType" = "Transparent"*/ }
		LOD 200
		Cull Off
		Zwrite Off
		ZTest Off
		Blend SrcAlpha OneMinusSrcAlpha

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
			// refraction thing
			//float4 _GrabTexture_TexelSize;
			sampler2D _MainTex1;
			sampler2D _MainTex2;
			float4 _MainTex1_ST;
			//float _Refraction;
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
				o.uv = TRANSFORM_TEX(v.uv, _MainTex1);
				UNITY_TRANSFER_FOG(o, o.vertex);
				o.color = v.color;

				half4 screenpos = ComputeGrabScreenPos(o.vertex);
				o.screenPos.xy = screenpos.xy / screenpos.w;
				half depth = length(mul(UNITY_MATRIX_MV, v.vertex));
				o.screenPos.z = depth;
				o.screenPos.w = depth;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0, 0, 0, 1);

				float2 uv = i.uv;
				float2 coord = -1.0 + 2.0*uv;

				/*float3 distort = dispHeat(uv) * float3(i.color.r, i.color.g, i.color.b);
				float2 offset = distort * _Refraction * _GrabTexture_TexelSize.xy;
				i.screenPos.xy = offset * i.screenPos.z + i.screenPos.xy;
				col = tex2D(_GrabTexture, i.screenPos);

				float3 org = float3(0., -2., 4.);
				float3 dir = normalize(float3(coord.x*1.6, -coord.y, -1.5));
				float f = flame(float3(coord.x, coord.y, sin(coord.x)), _Time.y);
				float g = max(0.0, 0.9 - f);
				//float4 rm = raymarch(float3(0.0, 0.0, -10.0), float3(0.0, 0.0, 1.0), _Time.y);
				float4 flameCol = lerp(float4(1., .5, .1, 1.), float4(0.1, .5, 1., 1.), pow(g, 8.0));// coord.y*.02 + .4);

				col.rgb = lerp(col.rgb, flameCol, g);*/
				col.rgb = lerp(tex2D(_MainTex1, uv), tex2D(_MainTex2, uv), 0.5);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
