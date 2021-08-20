Shader "Custom/uv"
{
	Properties{
		 _Color("Color", Color) = (1,1,1,1)
		 _MainTex("Albedo (RGBA)", 2D) = "white" {}
		 _Metallic("Metallic", Range(0,1)) = 0.0
		 _Pos("Pos",Range(0,100)) = 0.0
	}
		SubShader{
			Tags { "RenderType" = "Transparent"}
			LOD 200

			CGPROGRAM
			 // Physically based Standard lighting model, and enable shadows on all light types
			 #pragma surface surf Standard alpha:fade
			 #pragma alpha:fade

			 // Use shader model 3.0 target, to get nicer looking lighting
			 #pragma target 3.0

			 sampler2D _MainTex;

			 struct Input {
				 float2 uv_MainTex;
			 };

			 half _Metallic;
			 fixed4 _Color;
			 float _Pos;

			 void surf(Input IN, inout SurfaceOutputStandard o) {
				 // Albedo comes from a texture tinted by color
				 fixed2 scrolledUV = IN.uv_MainTex;
				 scrolledUV.x += _Pos / 100;
				 fixed4 c = tex2D(_MainTex, scrolledUV) * _Color;
				 o.Albedo = c.rgb;

				 // Metallic and smoothness come from slider variables
				 o.Metallic = _Metallic;
				 o.Alpha = c.a;
			 }
			 ENDCG
		 }
			 FallBack "Diffuse"
}
