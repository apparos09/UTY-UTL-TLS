Shader "Hidden/Color Grading Image Effect Shader"
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

        sampler2D _ColorGradeRed;
        sampler2D _ColorGradeGreen;
        sampler2D _ColorGradeBlue;


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
                
                // The three color coordinates for the current pixel.
                fixed colR = col.r;
                fixed colG = col.g;
                fixed colB = col.b;

                // Gets the new colors for red, green, and blue color coordinates as uvs.
                fixed4 newColRed = tex2D(_ColorGradeRed, fixed2(col.r, col.r));
                fixed4 newColGreen = tex2D(_ColorGradeRed, fixed2(col.g, col.g));
                fixed4 newColBlue = tex2D(_ColorGradeRed, fixed2(col.b, col.b));

                // Creates the new color.
                fixed4 newCol = col;
                newCol.r = newColRed.r;
                newCol.g = newColGreen.g;
                newCol.b = newColBlue.b;

                // Returns the new color.
                return newCol;
            }
            ENDCG
        }
    }
}
