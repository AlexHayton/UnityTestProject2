Shader "Custom/SwirlySmoke" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Noise ("Noise", 2D) = "noise" {}
		_DistortSpeed ("Turbulence", float) = 0.01
		_DarkSpots("Dark Spots", float) = 0.1
		_Overbright("Overbright", float) = 1
		_ScrollSpeed ("Scroll Speed (only x and y)", Vector) = (1,1,0,0)
	}
	
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
		Lighting Off
		Cull Back

		ZWrite Off
		Fog { Mode Off }
		Blend One One
		//LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		float4 _Color;
		sampler2D _MainTex;
		sampler2D _Noise;
		float _DistortSpeed;
		float _DarkSpots;
		float _Overbright;
		float4 _ScrollSpeed;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Noise;
		};


		void surf (Input IN, inout SurfaceOutput o) {
			float offset = _Time.y*_DistortSpeed;
			float2 texLookup = IN.uv_Noise + float2(0.25,-0.25) + offset*_ScrollSpeed.xy;
			float4 c = tex2D (_Noise, texLookup);
			c = tex2D(_Noise, c.yx - 8.0*float2(offset*0.71,offset*0.33));
	
			float intensity = c.r + c.g * 0.5 + c.b * 0.25 + c.a * 0.12;
	
			intensity = (intensity - _DarkSpots)*(_Overbright);
			intensity = clamp( 0, intensity, 1 );
			intensity = intensity * intensity;
	
			c = intensity * _Color * tex2D( _MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Emission = c.rgb;
			o.Alpha = c.a;


		}
		ENDCG
	} 
	FallBack "Diffuse"
}
