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

        // Specifies if using one colour grade or not.
        fixed _SingleGradeMode;

        sampler2D _ColorGrade;
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
               
                // The new color.
                fixed4 newCol = col;
                
                // Checks that the new color is not black or white.
                fixed notBlack = newCol.r != 0.0F && newCol.g != 0.0F && newCol.b != 0.0F;
                fixed notWhite = newCol.r != 1.0F && newCol.g != 1.0F && newCol.b != 1.0F;
                
                if(notBlack && notWhite)
                {
                    // 0 = false, anything else is true.
                    if(_SingleGradeMode) // Single grade mode.
                    {
                        // R = U (X), G = UV (XY), B = V (Y)
                        newCol.r = tex2D(_ColorGrade, fixed2(col.r, 0.0F));
                        newCol.g = tex2D(_ColorGrade, fixed2(col.g, col.g));
                        newCol.b = tex2D(_ColorGrade, fixed2(0.0F, col.b));
                    }
                    else // Multi-grade mode.
                    {
                        // Gets the new colors for red, green, and blue color coordinates as uvs.
                        fixed4 newColRed = tex2D(_ColorGradeRed, fixed2(col.r, 0.5F));
                        fixed4 newColGreen = tex2D(_ColorGradeGreen, fixed2(col.g, 0.5F));
                        fixed4 newColBlue = tex2D(_ColorGradeBlue, fixed2(col.b, 0.5F));

                        // Sets new colors.
                        newCol.r = newColRed.r;
                        newCol.g = newColGreen.g;
                        newCol.b = newColBlue.b;
                        // newCol.a = col.a; // Unneeded.
                    }
                }

                

                // Returns the new color.
                return newCol;
            }
            ENDCG
        }
    }
}
