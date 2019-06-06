Shader "Unlit/AudioWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AudioTex ("Audio Channel", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _AudioTex;
            float4 _MainTex_ST;

            #define mod(x, y) ((x) - (y)*floor((x)/(y)))

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            

            float squared(float x) {
                return x * x;
            }

            float getAmp(float freq) {
                return tex2D(_AudioTex, float2(freq / 512.0f, 0)).x;
            }

            float getWeight(float f) {
                return (getAmp(f - 2.0f) + getAmp(f - 1.0f) + getAmp(f+2.0f) + getAmp(f + 1.0f) + getAmp(f)) / 5.0f;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvTrue = i.uv;
                float2 uv = -1.0f + 2.0f * uvTrue;

                float lineIntensity;
                float glowWidth;

                float3 color = 0.0f;
                // color.r = 1.0f;

                for (float i = 0; i < 5; i++) {
                    uv.y += (0.2 * sin(uv.x + i / 7.0f - _Time.y * 0.6f));
                    float Y = uv.y + getWeight(squared(i) * 20.0f) * (tex2D(_AudioTex, float2(uvTrue.x, 1)).x - 0.5f);
                    // lineIntensity = 0.4f + squared(1.6f * abs(mod(uvTrue.x + i / 1.3f, 2.0f) - 1.0f));
                    lineIntensity = 0.4 + squared(1.6 * abs(mod(uvTrue.x + i / 1.3 + _Time.y,2.0) - 1.0));
                    // lineIntensity = 0.4f + squared(1.6f * abs(mod(uvTrue.x + i / 1.3f + _Time.y, 2.0f) - 1.0f)) / 1000;
                    // lineIntensity = 0.4f;
                    glowWidth = abs(lineIntensity / (150.0f * Y));
                    // glowWidth = abs(lineIntensity / (50.0 * Y));
                    color += float3(glowWidth * (2.0f + sin(_Time.y * 0.13f)),
                                    glowWidth * (2.0f - sin(_Time.y * 0.23f)),
                                    glowWidth * (2.0f - cos(_Time.y * 0.19f)));
                }
                color = clamp(color, 0, 1);
                
                return float4(color, 1.0f);
            }
            ENDCG
        }
    }
}
