Shader "Custom/StarSurface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade finalcolor:star

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb * 2.0;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }

		void star(Input IN, SurfaceOutputStandard o, inout fixed4 color) {

			float2 uv = IN.uv_MainTex.xy;

			//float d = 0.5f + 0.1f + 0.1f * sin(blinkSpeed * _Time.y);
			float d = 0.5f + 0.05f + 0.05f * sin(_Time.y * 1.5f);


			float2 p1 = float2(0.0, 0.0);
			float2 p2 = float2(0.0, 1.0);
			float2 p3 = float2(1.0, 0.0);
			float2 p4 = float2(1.0, 1.0);

			float d1 = length(uv - p1);
			float d2 = length(uv - p2);
			float d3 = length(uv - p3);
			float d4 = length(uv - p4);
			fixed4 col = float4(1.0, 1.0, 1.0, 1.0f);
			float4 blk = float4(0.0, 0.0, 0.0, 0.0);
			if (d1 < d) {
				col = blk;
			}
			if (d2 < d) {
				col = blk;
			}
			if (d3 < d) {
				col = blk;
			}
			if (d4 < d) {
				col = blk;
			}

			// sample the texture
			//fixed4 col = tex2D(_MainTex, i.uv);
			// apply fog

			color = col * 1.5f;
		}
        ENDCG
    }
    FallBack "Diffuse"
}
