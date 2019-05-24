Shader "Custom/RhythmShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Completion ("Completion", Range(0.0, 1.0)) = 0.5
        _LockColor ("LockColor", Color) = (1.0, 0.1228, 0.0, 1.0)
        _UnlockColor ("UnlockColor", Color) = (1.0, 0.9375, 0.6179, 1.0)
    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        Tags { "Queue"="Transparent" "RenderType"="Transparent"}
        LOD 200
        ZWrite Off
        // Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade

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
        float _Completion;
        float4 _LockColor;
        float4 _UnlockColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_MainTex.xy;
            fixed4 c;
            if (uv.y > _Completion) {
                c = tex2D (_MainTex, IN.uv_MainTex) * _LockColor;
            } else {
                c = tex2D (_MainTex, IN.uv_MainTex) * _UnlockColor;                
            }
            
            // Albedo comes from a texture tinted by color
            // fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            // o.Alpha = c2.a;
            o.Alpha = _Color.a;
        }

        // void frag (Input IN, SurfaceOutputStandard o, inout fixed4 color) {
        // }
        ENDCG
    }
    FallBack "Diffuse"
}
