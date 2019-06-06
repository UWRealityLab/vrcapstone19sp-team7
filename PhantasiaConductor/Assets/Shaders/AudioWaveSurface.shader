Shader "Custom/AudioWaveSurface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _AudioTex ("Audio Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade finalcolor:wave

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _AudioTex;

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
            // fixed4 c = _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        #define mod(x, y) ((x) - (y)*floor((x)/(y)))

        float squared(float x) {
            return x * x;
        }

        float getAmp(float freq) {
            return tex2D(_AudioTex, float2(freq / 512.0f, 0)).x;
        }

        float getWeight(float f) {
            return (getAmp(f - 2.0f) + getAmp(f - 1.0f) + getAmp(f+2.0f) + getAmp(f + 1.0f) + getAmp(f)) / 5.0f;
        }        

        void wave(Input IN, SurfaceOutputStandard o, inout fixed4 color) {
            color.xyz = o.Albedo;
            color.a = o.Alpha;
            // float2 uvTrue = i.uv;
            float2 uvTrue = IN.uv_MainTex.xy;
            float2 uv = -1.0f + 2.0f * uvTrue;

            float lineIntensity;
            float glowWidth;

            float3 c = 0.0f;
            // float3 c = color;

            for (float i = 0; i < 5; i++) {
                uv.y += (0.2 * sin(uv.x + i / 7.0f - _Time.y * 0.6f));
                float Y = uv.y + getWeight(squared(i) * 20.0f) * (tex2D(_AudioTex, float2(uvTrue.x, 1)).x - 0.5f);
                // lineIntensity = 0.4f + squared(1.6f * abs(mod(uvTrue.x + i / 1.3f, 2.0f) - 1.0f));
                lineIntensity = 0.4 + squared(1.6 * abs(mod(uvTrue.x + i / 1.3 + _Time.y,2.0) - 1.0));
                // lineIntensity = 0.4f + squared(1.6f * abs(mod(uvTrue.x + i / 1.3f + _Time.y, 2.0f) - 1.0f)) / 1000;
                // lineIntensity = 0.4f;
                glowWidth = abs(lineIntensity / (150.0f * Y));
                // glowWidth = abs(lineIntensity / (50.0 * Y));
                c += float3(glowWidth * (2.0f + sin(_Time.y * 0.13f)),
                                glowWidth * (2.0f - sin(_Time.y * 0.23f)),
                                glowWidth * (2.0f - cos(_Time.y * 0.19f)));
            }
            c = clamp(c, 0.0, 1.0);
            
            // return float4(color, 1.0f);            
            color += float4(c, 0.0);
            color = clamp(color, 0.0, 1.0);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
