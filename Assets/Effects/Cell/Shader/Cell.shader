Shader "Custom/Sprite/Cell" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_BorderFreq("Border Frequency", Range(0.0, 100.0)) = 10.0
		_BorderAmp("Border Amplitude", Range(0.0, 0.1)) = 0.005
		_BorderSpeed("Border Speed", Range(0.0, 10000.0)) = 4.0
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
		float _BorderFreq;
		float _BorderAmp;
		float _BorderSpeed;

		struct Input {
			float2 uv_MainTex;
		};

		#define DEG2RAD 3.14159 / 180.0
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

		/**
		* Scale UV
		* @author pierre.plans@gmail.com
		**/
		float2 scaleUV(in float2 uv, in float2 scale) {
			return uv*scale;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			float2 uv = IN.uv_MainTex;
			float2 coord = -1.0 + 2.0*uv;
			float dist = length(coord);
			float dt = dot(float2(0.0, 1.0), coord);
			float cosO = dot(float2(0.0, 1.0), coord) / dist;
			float angle = atan2(0.0, 1.0) - atan2(coord.y, coord.x);// acos(dot(float2(0.0, 1.0), coord) / dist);
			// ovalization
			float2 speedV = float2(1.3, 1.0);
			float2 speedVM = float2(1.0, 1.0);
			float speed = length(speedV);
			float cosOS = dot(speedV, coord) / (dist*speed);
			float angleS = acos(dot(speedV, coord) / (dist*speed));
			float dtS = dot(speedV, coord);
			float2 offset = float2(cos(angleS)*speedV.x, sin(angleS)*speedV.y);

			float2 uvOv = float2(dtS, dtS);

			//uv = uv + uvOv;
			//uv = scaleUV(uv, speedV);
			//uv = rotateUV(uv, vec2(0.0, 0.0), angleS);
			float2 center = float2(0.0, 0.0);
			center = scaleUV(center, speedV);
			center = rotateUV(center, float2(0.0, 0.0), angleS);

			float _borderOffset = sin(angle*_BorderFreq + _Time.y*_BorderSpeed)*_BorderAmp;

			bool testOval = length(coord)>abs(_borderOffset);
			// border animation
			if (dist>0.7)
				uv += _borderOffset;
			
			fixed4 c = tex2D(_MainTex, uv) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Legacy Shaders/Transparent/Diffuse"
}
