Shader "Unlit/SpinFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 1.0
        _SampleDensity ("Sample Density", Int) = 1000
        _FadeScale ("Fade Scale", Float) = 1.0
        _FadeDelay ("Fade Delay", Float) = 0.0
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
            float4 _MainTex_ST;
            float _Speed;
            float _FadeScale;
            int _SampleDensity;

            #define PI 3.14159

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = _Time.y;
                float2 center = 0.5f;

                // stop once we reach full rotation
                t = clamp(t * _Speed, 0.0f, PI * 2.0);

                // float radialDistance = length(center - i.uv);

                // it would be best if we found a way to increase the sample density by distance
                int density = _SampleDensity;
                
                // delta between samples
                float deltaT = t / density;
                float sampleT = 0.0f;
                
                float3 totalColor = 0.0f;

                float2 shiftedPos = i.uv.xy - center;
                
                for (int i = 0; i < density; i++) {
                    float2 cs = float2(cos(sampleT), sin(sampleT));
                    float2x2 m = float2x2(cs.x, -cs.y, cs.y, cs.x);

                    float2 samplePos = mul(m, shiftedPos);
                    samplePos += center;
                    samplePos = clamp(samplePos, 0.0f, 1.0f);

                    totalColor = max(totalColor, tex2D(_MainTex, samplePos));
                    sampleT += deltaT;
                }
                totalColor += _Time.y * _FadeScale;
                totalColor = clamp(totalColor, 0.0f, 1.0f);

                return float4(totalColor, 1.0f);
            }
            ENDCG
        }
    }
}