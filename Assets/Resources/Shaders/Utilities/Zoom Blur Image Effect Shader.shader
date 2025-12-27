Shader "Hidden/Zoom Blur Image Effect Shader"
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

        // The screen resolution, which is provided by Unity.
        fixed4 _ScreenResolution;

        // The center of the effect, the nalge of the effect, the direction of the effect, and the number of samples.
        // The shader assumes the angle is in radians.
        fixed4 _ZoomCenter;
        fixed _ZoomLength;
        int _ZoomIn;
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

                // The zoom center in screen pixels.
                fixed2 zoomCenterScreen = _ZoomCenter * _ScreenResolution;

                // Gets the pixel position as a UV and in the screen.
                fixed2 pixelPosUV = i.uv;
                fixed2 pixelPosScreen = pixelPosUV * _ScreenResolution;

                // A distance vector from the pixel position to the radial center.
                fixed2 distVec;
                distVec.x = _ZoomCenter.x - pixelPosUV.x;
                distVec.y = _ZoomCenter.y - pixelPosUV.y;

                // Gets the normalized distance vector.
                fixed2 distVecNormalized = normalize(distVec);

                // Divides the length by the sample count.
                fixed sampleInc = _ZoomLength / _SampleCount;

                // The number of samples used.
                int samplesUsed = 0;

                // Gets all the pixels being used based on the sample count.
                for(int n = 1; n <= _SampleCount; n++)
                {
                    // The sample offset from the current pixel position.
                    float sampleMult = sampleInc * n;

                    // If zooming in, take pixels from in front of the current pixel.
                    // If zooming out, use pixels from behind the current pixel.
                    fixed direc = (_ZoomIn) ? 1 : -1;

                    // The mix pixel position.
                    fixed2 mixPixelPosScreen = pixelPosScreen + direc * (distVecNormalized * sampleMult);

                    // Gets the mix pixel position in uv space.
                    fixed2 mixPixelPosUV = mixPixelPosScreen / _ScreenResolution;

                    // NOTE: this shader doesn't take into account UVs that go outside the bounds of the image.
                    // However, the shader appears to work fine regardless.

                    // Uses the mix pixel pos as uvs to get the pixel.
                    fixed4 mixPixel = tex2D(_MainTex, mixPixelPosUV.xy);

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
                

                // Gets the pixel color from the altered UV coordinates.
                // newCol = tex2D(_MainTex, newUV.xy);

                // Returns the new color.
                return newCol;
            }
            ENDCG
        }
    }
}
