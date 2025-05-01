using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    /*
     * Sources:
     * - https://www.aforgenet.com/framework/features/convolution_filters/
     * - https://setosa.io/ev/image-kernels/
     * - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Texture2D.GetPixels.html
     */

    // Applies a convolution kernel to the camera render.
    // This code is INEFFICIENT since it goes through every pixel of the render texture.
    // It's recommended that this is not used.
    public class CameraKernelRenderFilter : CameraRenderFilterer
    {
        // This program only has 3x3 kernels. An expanded version should allow for dynamic kernel sizes.
        // This will likely not be added since this code is efficient either way.

        // The kernel for the camera render image.
        // This defaults to the identity array, which means no change.
        public float[,] kernel = new float[3, 3] 
        {
            { 0, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 0 }
        };

        // The identity kernel (no changes)
        public static float[,] identityKernel = new float[3,3]
        {
            { 0, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 0 }
        };

        // A blur kernel.
        public static float[,] blurKernel = new float[3, 3]
        {
            { 0.0625F, 0.125F, 0.0625F },
            { 0.125F, 0.25F, 0.125F },
            { 0.0625F, 0.125F, 0.0625F }
        };

        // A sharpen kernel.
        public static float[,] sharpenKernel = new float[3, 3]
        {
            { 0, -1, 0 },
            { -1, 5, -1 },
            { 0, -1, 0 }
        };

        // An outline kernel.
        public static float[,] outlineKernel = new float[3, 3]
        {
            { -1, -1, -1 },
            { -1, 8, -1 },
            { -1, -1, -1 }
        };


        // Filters the camera render as a texture 2D.
        // Remember to apply the changed pixels to the texture2D after you're done.
        public override Texture2D FilterRenderAsTexture2D(Texture2D texture2D)
        {
            // Example - invert the colours of the texture 2D pixels.
            // The old colours and the new colours.
            // It's a 1D array that goes row by row starting at the bottom left.
            Color[] oldColors = texture2D.GetPixels();
            Color[] newColors = oldColors.Clone() as Color[];

            // Runs the kernel calculations.
            for(int i = 0; i < oldColors.Length; i++)
            {
                // The kernel is a 2D array, so conversions from the 1D array need to be done.

                // The colour array. It's empty by default to account for unused spots.
                Color[,] colorArr = new Color[3, 3]
                {
                    { Color.clear, Color.clear, Color.clear},
                    { Color.clear, Color.clear, Color.clear},
                    { Color.clear, Color.clear, Color.clear}
                };

                // The array of indexes for the mixing.
                int[,] indexArr = new int[3, 3]
                {
                    { i - texture2D.width - 1, i - texture2D.width, i - texture2D.width + 1 },
                    { i - 1, i, i + 1 },
                    { i + texture2D.width - 1, i + texture2D.width, i + texture2D.width + 1 }
                };

                // Checks which spots are valid and which are not.
                // This is used for averaging the values.
                bool[,] validArr = new bool[3, 3];

                // Goes through the indexes to find the colours.
                for(int c = 0; c < indexArr.GetLength(0); c++) // Col
                {
                    for(int r = 0; r < indexArr.GetLength(1); r++) // Row
                    {
                        // If the index is valid, 
                        if (indexArr[c, r] >= 0 && indexArr[c, r] < oldColors.Length)
                        {
                            // Saves the color.
                            colorArr[c, r] = oldColors[indexArr[c, r]];

                            // Spot is valid.
                            validArr[c, r] = true;
                        }
                        else
                        {
                            // Spot is invalid.
                            validArr[c, r] = false;
                        }
                    }
                }


                // The colour vector4. XYZW = RGBA.
                Vector4 colorVec4 = new Vector4();

                // This isn't necessary, but it's being left in.
                // The value count.
                int valueCount = 0;

                // Multiplies each colour component by the kernel value and adds them together.
                for (int c = 0; c < colorArr.GetLength(0); c++) // Col
                {
                    for (int r = 0; r < colorArr.GetLength(1); r++) // Row
                    {
                        // Sums the colour values.
                        colorVec4.x += (colorArr[c, r].r * kernel[c, r]);
                        colorVec4.y += (colorArr[c, r].g * kernel[c, r]);
                        colorVec4.z += (colorArr[c, r].b * kernel[c, r]);
                        colorVec4.w += (colorArr[c, r].a * kernel[c, r]);

                        // If the spot is valid, then increase the value count.
                        if (validArr[c, r])
                            valueCount++;
                    }
                }

                // Divide the vector by the value count (wrong).
                // colorVec4 /= valueCount;

                // Clamp the colour values.
                colorVec4.x = Mathf.Clamp01(colorVec4.x);
                colorVec4.y = Mathf.Clamp01(colorVec4.y);
                colorVec4.z = Mathf.Clamp01(colorVec4.z);
                colorVec4.w = Mathf.Clamp01(colorVec4.w);

                // Creates the new colour. The alpha value is kept at 1.0.
                Color newColor = new Color(colorVec4.x, colorVec4.y, colorVec4.z, colorVec4.w);
                newColor.a = 1.0F;

                // Saves the new colour.
                newColors[i] = newColor;
            }
            
            // Sets the new colours and applies them.
            texture2D.SetPixels(newColors);
            texture2D.Apply();
            
            // Returns the result.
            return texture2D;
        }
    }
}