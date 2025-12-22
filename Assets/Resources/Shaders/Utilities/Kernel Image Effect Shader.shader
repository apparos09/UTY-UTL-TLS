Shader "Hidden/Kernel Image Effect Shader"
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

        // Clamp function.
        fixed clampFixed (fixed value, fixed min, fixed max)
        {
            fixed result;

            if(value < min)
            {
                result = min;
            }
            else if(value > max)
            {
                result = max;
            }
            else
            {
                result = value;
            }
                
            return result;   
        }

        // The size of the texture.
        fixed _MainTexWidth;
        fixed _MainTexHeight;

        // The kernel rows (0, 1, 2) - the w value is ignored.
        // SetVector takes a vector4, so these use fixed4.
        fixed4 _KernelRow0;
        fixed4 _KernelRow1;
        fixed4 _KernelRow2;
        
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
                // The old colour and new colour.
                fixed4 oldCol = tex2D(_MainTex, i.uv);
                fixed4 newCol = oldCol;

                // The position of the pixel in the texture.
                fixed2 pixelPos;
                pixelPos.x = _MainTexWidth * i.uv.x;
                pixelPos.y = _MainTexHeight * i.uv.y;

                // The pixel mix.
                fixed4 pixelMix = fixed4(0, 0, 0, 0);

                // Adds together the values.
                for(fixed n = 0; n < 9; n++)
                {
                    // The colour of the pixel being used.
                    fixed4 pCol;

                    // The UV value for the pixel.
                    fixed2 pUV;

                    // The multiplier from the kernel.
                    fixed mult;

                    // Gets the UV value of the related pixel.
                    // Even though this is a decimal value, the switch statement works with whole numbers.
                    // Usuing decimals with a switch statement isn't allowed.
                    // In Unity, floats and doubles can't use switch statements, but it appears that...
                    // Fixed values can be used in the shader as long as they're whole numbers.
                    // The shader should be able to run on other platforms as it is.
                    switch(n)
                    {
                        default: // Invalid
                            pUV = fixed2(-1, -1);
                            mult = 0;
                            break;

                        case 0: // (0,0)
                            pUV = fixed2((pixelPos.x - 1) / _MainTexWidth, (pixelPos.y - 1) / _MainTexHeight);
                            mult = _KernelRow0.x;
                            break;

                        case 1: // (0,1)
                            pUV = fixed2(pixelPos.x / _MainTexWidth, (pixelPos.y - 1) / _MainTexHeight);
                            mult = _KernelRow0.y;
                            break;

                        case 2: // (0,2)
                            pUV = fixed2((pixelPos.x + 1) / _MainTexWidth, (pixelPos.y - 1) / _MainTexHeight);
                            mult = _KernelRow0.z;
                            break;

                        case 3: // (1,0)
                            pUV = fixed2((pixelPos.x - 1) / _MainTexWidth, pixelPos.y / _MainTexHeight);
                            mult = _KernelRow1.x;
                            break;

                        case 4: // (1,1) - Center
                            pUV = fixed2(pixelPos.x / _MainTexWidth, pixelPos.y / _MainTexHeight);
                            mult = _KernelRow1.y;
                            break;

                        case 5: // (1,2)
                            pUV = fixed2((pixelPos.x + 1) / _MainTexWidth, pixelPos.y / _MainTexHeight);
                            mult = _KernelRow1.z;
                            break;

                        case 6: // (2,0)
                            pUV = fixed2((pixelPos.x - 1) / _MainTexWidth, (pixelPos.y + 1) / _MainTexHeight);
                            mult = _KernelRow2.x;
                            break;

                        case 7: // (2,1)
                            pUV = fixed2(pixelPos.x / _MainTexWidth, (pixelPos.y + 1) / _MainTexHeight);
                            mult = _KernelRow2.y;
                            break;

                        case 8: // (2,2)
                            pUV = fixed2((pixelPos.x + 1) / _MainTexWidth, (pixelPos.y + 1) / _MainTexHeight);
                            mult = _KernelRow2.z;
                            break;
                    }

                    // Invalid UV, so skip.
                    if(pUV.x < 0 || pUV.x >= 1 || pUV.y < 0 || pUV.y >= 1)
                    {
                        continue;
                    }

                    // Sets the pixel colour.
                    pCol = tex2D(_MainTex, pUV);
                    
                    // Add to the pixel mix.
                    pixelMix.x += pCol.r * mult;
                    pixelMix.y += pCol.g * mult;
                    pixelMix.z += pCol.b * mult;
                    pixelMix.w += pCol.w * mult;

                }

                // Sets the new colour. The alpha value is the same as the old value.
                newCol.r = clampFixed(pixelMix.x, 0, 1);
                newCol.g = clampFixed(pixelMix.y, 0, 1);
                newCol.b = clampFixed(pixelMix.z, 0, 1);
                newCol.a = oldCol.a;

                // Return the new colour.
                return newCol;
            }
            ENDCG
        }
    }
}
