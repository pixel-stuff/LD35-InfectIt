Shader "Custom/ScreenRedEdge"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_EdgeWidth("Edge Width", Range(0.0, 0.5)) = 0.1
		_EdgeIntensity("Edge Intensity", Range(0.0, 1.0)) = 0.5
		_AnimationFrequency("Anim. Freq.", Range(0.0, 100.0)) = 0.5
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
			
			//sampler2D _MainTex;
			float4 _Color;
			float _EdgeWidth;
			float _EdgeIntensity;
			float _AnimationFrequency;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0, 0, 0, 0);
				float2 coord = -1.0 + 2.0*i.uv;
				float width = _EdgeWidth;
				float pW = 1.0 - width;
				float aT = abs(sin(_Time.y*_AnimationFrequency))*_EdgeIntensity;
				if (abs(coord.x) > pW && abs(coord.y) > pW) {
					col.rgb = _Color.rgb;
					col.a = (1.0-(1.0-abs(coord.x))/ width + 1.0 - (1.0 - abs(coord.y)) / width)*aT;
				}
				if (abs(coord.x) <= pW && abs(coord.y)>pW) {
					col.rgb = _Color.rgb;
					col.a = (1.0 - (1.0 - abs(coord.y)) / width)*aT;
				}
				if (abs(coord.x) > pW && abs(coord.y)<= pW) {
					col.rgb = _Color.rgb;
					col.a = (1.0 - (1.0 - abs(coord.x)) / width)*aT;
				}
				return col;
			}
			ENDCG
		}
	}
}
