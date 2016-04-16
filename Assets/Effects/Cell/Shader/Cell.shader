Shader "Custom/Sprite/Cell" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True"/* "RenderType" = "Transparent"*/ }
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

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}
		Fallback "Legacy Shaders/Transparent/Diffuse"
}
