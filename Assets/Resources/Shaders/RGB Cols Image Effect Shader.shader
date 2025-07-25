Shader "Hidden/RGB Cols Image Effect Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Splits the screen into three columns, each a different color.
                // The colors are coded to be RGB.
                if(i.uv.x <= 0.33F) // Red (R) - Left
                {
                    col *= fixed4(1, 0, 0, 1);
                }
                else if(i.uv.x <= 0.66F) // Green (G) - Center
                {
                    col *= fixed4(0, 1, 0, 1);
                }
                else // Blue (B) - Right
                {
                    col *= fixed4(0, 0, 1, 1);
                }

                return col;
            }
            ENDCG
        }
    }
}
