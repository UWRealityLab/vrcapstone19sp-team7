Shader "Unlit/Star"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float3 blackColor = 0.0f;
			float3 whiteColor = 1.0f;
			float blinkSpeed = 1.0f;

            fixed4 frag (v2f i) : SV_Target
            {
				
				float2 uv = i.uv;
				
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
				float4 blk = float4(0.0, 0.0, 0.0, 1.0);
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
				
                return col;
            }
            ENDCG
        }
    }
}
