Shader "Custom/Sprite/DesaturationOverTime" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_TimeStart("Time start", Float) = 0.0
		_TimeCurrent("Time current", Float) = 0.0
		_TimeEnd("Time end", Float) = 2.0
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }
		LOD 200
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Own alpha:fade

		half4 LightingOwn(SurfaceOutput s, half3 lightDir, half atten) {
			half4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		fixed4 _Color;
		float _TimeStart;
		float _TimeCurrent;
		float _TimeEnd;

		struct Input {
			float2 uv_MainTex;
		};

		float3 desaturate(float3 c, float p) {
			float3 cd;
			cd.r = cd.g = cd.b = (c.r + c.g + c.b) / 3.0;
			return lerp(c, cd, p);
		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			float p = min(1.0, max(0.0, _TimeCurrent / (_TimeEnd - _TimeStart)));
			o.Albedo = desaturate(c, p);
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
