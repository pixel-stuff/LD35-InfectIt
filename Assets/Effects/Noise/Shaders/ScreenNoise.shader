
Shader "Custom/Effect/Noise" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Bloom("Bloom (RGB)", 2D) = "black" {}
		_MaskTex("Mask (A)", 2D) = "white" {}
		_NoiseTex("Mask (A)", 2D) = "white" {}
		_Color("Color (RGBA)", Color) = (0, 0, 0, 1)
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	sampler2D _MainTex;
	sampler2D _Bloom;
	sampler2D _MaskTex;
	sampler2D _NoiseTex;

	uniform half4 _MainTex_TexelSize;
	uniform half4 _Parameter;
	uniform half4 _Color;

	struct v2f_tap
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};

	v2f_tap vert(appdata_img v)
	{
		v2f_tap o;

		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord;

		return o;
	}

	float hash(half2 uv) {
		half h = dot(uv, half2(1.0, 1.0));
		return frac(sin(h)*13.405945);
	}

	float hash2(half2 uv) {
		return frac(1e4 * sin(17.0 * uv.x + uv.y * 0.1) * (0.1 + abs(sin(uv.y * 13.0 + uv.x))));
	}
	
	//  Classic Perlin 2D Noise 
	//  by Stefan Gustavson
	//
	half2 fade(half2 t) { return t*t*t*(t*(t*6.0 - 15.0) + 10.0); }
	half4 mod289(half4 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
	half4 permute(half4 x) { return mod289(((x * 34.0) + 1.0) * x); }

	float cnoise(half2 P) {
		half4 Pi = floor(P.xyxy) + half4(0.0, 0.0, 1.0, 1.0);
		half4 Pf = frac(P.xyxy) - half4(0.0, 0.0, 1.0, 1.0);
		Pi = fmod(Pi, 289.0); // To avoid truncation effects in permutation
		half4 ix = Pi.xzxz;
		half4 iy = Pi.yyww;
		half4 fx = Pf.xzxz;
		half4 fy = Pf.yyww;
		half4 i = permute(permute(ix) + iy);
		half4 gx = 2.0 * frac(i * 0.0243902439) - 1.0; // 1/41 = 0.024...
		half4 gy = abs(gx) - 0.5;
		half4 tx = floor(gx + 0.5);
		gx = gx - tx;
		half2 g00 = half2(gx.x, gy.x);
		half2 g10 = half2(gx.y, gy.y);
		half2 g01 = half2(gx.z, gy.z);
		half2 g11 = half2(gx.w, gy.w);
		half4 norm = 1.79284291400159 - 0.85373472095314 *
			half4(dot(g00, g00), dot(g01, g01), dot(g10, g10), dot(g11, g11));
		g00 *= norm.x;
		g01 *= norm.y;
		g10 *= norm.z;
		g11 *= norm.w;
		float n00 = dot(g00, half2(fx.x, fy.x));
		float n10 = dot(g10, half2(fx.y, fy.y));
		float n01 = dot(g01, half2(fx.z, fy.z));
		float n11 = dot(g11, half2(fx.w, fy.w));
		half2 fade_xy = fade(Pf.xy);
		half2 n_x = lerp(half2(n00, n01), half2(n10, n11), fade_xy.x);
		float n_xy = lerp(n_x.x, n_x.y, fade_xy.y);
		return 2.3 * n_xy;
	}

	fixed4 frag(v2f_tap i) : SV_Target
	{
		fixed4 color = tex2D(_MainTex, i.uv);
		fixed4 noiseT = tex2D(_NoiseTex, i.uv);
		fixed4 N = fixed4(0.0, 0.0, 0.0, 1.0);
		float Np = 0.0;
		if (_Parameter.w == 0.0) {
			N = fixed4(noiseT.r, noiseT.g, noiseT.b, 1.0);
			Np = noiseT.a;// (noiseT.r + noiseT.g + noiseT.b)*0.3333;
		}else if (_Parameter.w == 1.0) {
			fixed t = hash2((i.uv + _Time.y*_Parameter.x)*_Parameter.y)*_Parameter.z;
			N = fixed4(t, t, t, 1.0);
			Np = t;
		}else if (_Parameter.w == 2.0) {
			fixed t = cnoise((i.uv + _Time.y*_Parameter.x)*_Parameter.y)*_Parameter.z;
			N = fixed4(t, t, t, 1.0);
			Np = t;
		}

		return lerp(color, lerp(N, _Color, Np)*_Parameter.z, noiseT.a);
	}

	ENDCG

	SubShader {
		ZTest Off Cull Off ZWrite Off Blend Off

		// 0
		Pass{

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			ENDCG

		}
	}

	FallBack Off
}
