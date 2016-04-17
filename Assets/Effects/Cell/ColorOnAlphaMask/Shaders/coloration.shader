Shader "Custom/Sprite/ColorOnAlphaMask" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		// refraction thing
		_MainTex("Main Tex", 2D) = "white" {}
	}

	SubShader{
		Tags{ "Queue" = "Transparent"/* "RenderType" = "Transparent"*/ }
		LOD 200
		Cull Off
		Zwrite Off
		ZTest Off
		Blend SrcAlpha One

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
			float _Speed;
			float _Freq;
			float _Amp;

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0, 0, 0, 1);
				col.rgb = _Color.rgb*tex2D(_MainTex, i.uv).a;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
