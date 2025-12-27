Shader "Hidden/Radial Blur Image Effect Shader"
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

        // The center of the effect, the nalge of the effect, the direction of the effect, and the number of samples.
        // The shader assumes the angle is in radians.
        fixed4 _RadialCenter;
        float _RadialAngle;
        int _Clockwise;
        int _SampleCount;

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

                // Gets the pixel position as a UV.
                fixed2 currPixelPos = i.uv;

                // A distance vector from the pixel position to the radial center.
                fixed2 distVec;
                distVec.x = _RadialCenter.x - currPixelPos.x;
                distVec.y = _RadialCenter.y - currPixelPos.y;

                // Divides the angle by the sample count.
                fixed sampleInc = _RadialAngle / _SampleCount;

                // The number of samples used.
                int samplesUsed = 0;

                // Gets all the pixels being used based on the sample count.
                for(int n = 1; n <= _SampleCount; n++)
                {
                    // The angle.
                    float angle = sampleInc * n;

                    // Determines angle direction.
                    angle *= (_Clockwise) ? 1 : -1;

                    // The mix pixel position.
                    fixed2 mixPixelPos;

                    // Rotates based on the angle gotten from the sample.
                    mixPixelPos.x = (distVec.x * cos(angle)) - (distVec.y * sin(angle));
                    mixPixelPos.y = (distVec.x * sin(angle)) + (distVec.y * cos(angle));

                    // NOTE: this doesn't dismiss requested pixel positions that go beyond the bounds of the image.
                    // However, the shader appears to work fine without doing so.

                    // Subtracts the mix pixel position from the center to get the proper position.
                    mixPixelPos.x = _RadialCenter.x - mixPixelPos.x;
                    mixPixelPos.y = _RadialCenter.y - mixPixelPos.y;

                    // Gets the pixel to be mixed.
                    fixed4 mixPixel = tex2D(_MainTex, mixPixelPos.xy);

                    // Adds the mix pixel to the new color.
                    newCol += mixPixel;

                    // Adds to the samples used.
                    samplesUsed++;
                }

                // If there were samples used, average out the color.
                if(samplesUsed > 0)
                {
                    newCol /= samplesUsed;
                }

                // Returns the new color.
                return newCol;
            }
            ENDCG
        }
    }
}
