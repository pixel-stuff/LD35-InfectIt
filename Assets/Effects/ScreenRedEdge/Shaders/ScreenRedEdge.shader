Shader "Custom/ScreenRedEdge"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "Queue" = "Transparent+10"}
		// No culling or depth
		Cull Off ZWrite On ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0, 0, 0, 0);
				float2 coord = -1.0 + 2.0*i.uv;
				if (abs(coord.x) > 0.8 && abs(coord.y) > 0.8) {
					col.r = 1.0;
					col.a = 1.0-(1.0-abs(coord.x))/0.2 + 1.0 - (1.0 - abs(coord.y)) / 0.2;
				}
				return col;
			}
			ENDCG
		}
	}
}
