Shader "Custom/Transparent/Diffuse" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_LightRay("Player pos", Vector) = (0,-1, 0, 0)
		_NormalRay("Object pos", Vector) = (0, 1, 0, 0)
		_XUVRatio("X Ratio for UV coordinates", Range(0.0, 1.0)) = 0.5
		_YUVRatio("Y Ratio for UV coordinates", Range(0.0, 1.0)) = 0.5
		_SpriteSize("Sprite Size in World Units", float) = 1.0
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True"/* "RenderType" = "Transparent"*/ }
		LOD 200
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Own alpha:fade

	half4 _NormalRay;
	half4 _LightRay;
	float _XUVRatio;
	float _YUVRatio;
	float _SpriteSize;

	half4 LightingOwn(SurfaceOutput s, half3 lightDir, half atten) {
		//half NdotL = dot(s.Normal, lightDir);
		half3 SRay = half3(0.0, 1.0, 0.0);
		half3 LRay = /*_WorldSpaceLightPos0 + */lightDir;
		half NdotL = dot(SRay, -normalize(_LightRay.xyz));
		if (abs(NdotL) > 0.8) NdotL = 1.0;
		else NdotL = 0.0;
		half4 c;
		c.rgb = s.Albedo;// *pow(NdotL, 10.9);// /* _LightColor0.rgb*/ *(NdotL * atten);
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
		half3 pPos = mul(_Object2World, _LightRay).xyz;
		half3 oPos = mul(_Object2World, _NormalRay).xyz;
		half2 uv = half2(IN.uv_MainTex.x*_XUVRatio*2.0 - 1.0, IN.uv_MainTex.y);//half2(uvw.x*_XUVRatio*2.0-1.0, uvw.y*_YUVRatio*2.0 + 1.0);
		half3 lDir = half3(pPos.xy - (oPos.xy + (-1.0+2.0*uv)*_SpriteSize), 0.0f);
		half NdotL = dot(half3(0.0, 1.0, 0.0), -normalize(-lDir));
		if (NdotL > 0.0) {
			if (abs(NdotL) > 0.98) NdotL = 1.0;
			else if (abs(NdotL) > 0.6) NdotL = lerp(1.0f, 0.0f, (NdotL-0.02) / 0.48);
			else NdotL = 0.0;
		}

		o.Albedo = c.rgb * 2.0 * pow(NdotL, 10.0);
		o.Alpha = c.a;
	}
	ENDCG
	}
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
