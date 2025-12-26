Shader "Hidden/Flip Image Effect Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    CGINCLUDE
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

        // Integers that determine what axis to flip on.
        // These are bool values that are converted to ints when passed into the shader.
        int _FlipX;
        int _FlipY;

    ENDCG

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
               
                // The new color.
                fixed4 newCol = col;

                // The old UV coordinate.
                float2 oldUV = i.uv;

                // The new UV coordinate, which is set to the old UV by default.
                float2 newUV = i.uv;

                // If not equal to 0, it's true. If equal to 0, it's false. Any non-false value registers as true.
                if(_FlipX != 0) // Flip X
                {
                    newUV.x = abs(oldUV.x - 1.0F);
                }

                // Flip Y
                if(_FlipY != 0) // Flip Y
                {
                    newUV.y = abs(oldUV.y - 1.0F);
                }

                // Gets the pixel color from the altered UV coordinates.
                newCol = tex2D(_MainTex, newUV.xy);

                // Returns the new color.
                return newCol;
            }
            ENDCG
        }
    }
}
