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
     * - https://discussions.unity.com/t/faster-way-to-alter-a-texture2d-besides-texture2d-apply/787780/4
     * - https://www.youtube.com/watch?v=V9ZYDCnItr0
     */

    // Applies a convolution kernel to the camera render.
    // This code is INEFFICIENT since it goes through every pixel of the render texture.
    // It's recommended that you DO NOT USE THIS. Use the post processing version that operates with a shader instead.
    public class CameraKernelRenderFilter : CameraRenderFilterer
    {
        // The most common kernel sizes are 1x1 and 3x3.
        // 3x3 convolutions are more efficient than 5x5 and 7x7.
        // Even kernels are not used since they lack a centre.

        [Header("Kernel")]
        // The three kernel rows (defaults to identity).
        // These use vectors to restrict the size and allow editing in the inspector.
        public Vector3 kernelRow0 = new Vector3(0, 0, 0);
        public Vector3 kernelRow1 = new Vector3(0, 1, 0);
        public Vector3 kernelRow2 = new Vector3(0, 0, 0);

        // The row and colum count for the kernels.
        public const int KERNEL_ROW_COUNT = 3;
        public const int KERNEL_COLUMN_COUNT = 3;

        // The identity kernel (no changes)
        public static float[,] identityKernel = new float[3, 3]
        {
            { 0, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 0 }
        };

        // A blur kernel.
        public static float[,] blurKernel = new float[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
        {
            { 0.0625F, 0.125F, 0.0625F },
            { 0.125F, 0.25F, 0.125F },
            { 0.0625F, 0.125F, 0.0625F }
        };

        // A sharpen kernel.
        public static float[,] sharpenKernel = new float[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
        {
            { 0, -1, 0 },
            { -1, 5, -1 },
            { 0, -1, 0 }
        };

        // An emboss kernel.
        public static float[,] embossKernel = new float[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
        {
            { -2, -1, 0 },
            { -1, 1, 1 },
            { 0, 1, 2 }
        };

        // An edge kernel.
        public static float[,] edgeKernel = new float[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
        {
            { 0, -1, 0 },
            { -1, 4, -1 },
            { 0, -1, 0 }
        };

        // An outline kernel.
        public static float[,] outlineKernel = new float[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
        {
            { -1, -1, -1 },
            { -1, 8, -1 },
            { -1, -1, -1 }
        };

        // A left sobel kernel.
        public static float[,] leftSobelKernel = new float[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
        {
            { 1, 0, -1 },
            { 2, 0, -2 },
            { 1, 0, -1 }
        };

        // A right sobel kernel.
        public static float[,] rightSobelKernel = new float[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
        {
            { -1, 0, 1 },
            { -2, 0, 2 },
            { -1, 0, 1}
        };

        // A top sobel kernel.
        public static float[,] topSobelKernel = new float[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
        {
            { 1, 2, 1 },
            { 0, 0, 0 },
            { -1, -2, -1}
        };

        // A bottom sobel kernel.
        public static float[,] bottomSobelKernel = new float[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
        {
            { -1, -2, -1 },
            { 0, 0, 0 },
            { 1, 2, 1 }
        };

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Testing the script using preset kernels.
            // SetKernels(blurKernel);
            // SetKernels(sharpenKernel);
            // SetKernels(embossKernel);
            // SetKernel(edgeKernel);
            // SetKernels(outlineKernel);
            // SetKernels(leftSobelKernel);
            // SetKernels(rightSobelKernel);
            // SetKernels(topSobelKernel);
            // SetKernels(bottomSobelKernel);
        }


        // Filters the camera render as a texture 2D.
        // Remember to apply the changed pixels to the texture2D after you're done.
        public override Texture2D FilterRenderAsTexture2D(Texture2D texture2D)
        {
            // Creates the kernel array from the vectors.
            float[,] kernel = new float[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
            {
                { kernelRow0.x, kernelRow0.y, kernelRow0.z },
                { kernelRow1.x, kernelRow1.y, kernelRow1.z },
                { kernelRow2.x, kernelRow2.y, kernelRow2.z }
            };

            // Example - invert the colours of the texture 2D pixels.
            // The old colours and the new colours.
            // It's a 1D array that goes row by row starting at the bottom left.
            // The new colors array no longer clones the old one to save on computation time. The colors default to clear.
            Color[] oldColors = texture2D.GetPixels();
            Color[] newColors = new Color[oldColors.Length];

            // Runs the kernel calculations.
            for (int i = 0; i < oldColors.Length; i++)
            {
                // The kernel is a 2D array, so conversions from the 1D array need to be done.

                // The colour array. Empty spots will be filled with clear colours.
                Color[,] colorArr = new Color[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT];

                // This is hard-coded as a 3X3, but since that's the most efficient...
                // It's fine to leave it this way.
                // The array of indexes for the mixing.
                int[,] indexArr = new int[KERNEL_ROW_COUNT, KERNEL_COLUMN_COUNT]
                {
                    { i - texture2D.width - 1, i - texture2D.width, i - texture2D.width + 1 },
                    { i - 1, i, i + 1 },
                    { i + texture2D.width - 1, i + texture2D.width, i + texture2D.width + 1 }
                };

                // Checks which spots are valid and which are not.
                // This is used for averaging the values.
                bool[,] validArr = new bool[3, 3];

                // Goes through the indexes to find the colours.
                for (int r = 0; r < indexArr.GetLength(0); r++) // Row
                {
                    for (int c = 0; c < indexArr.GetLength(1); c++) // Col
                    {
                        // If the index is valid, 
                        if (indexArr[r, c] >= 0 && indexArr[r, c] < oldColors.Length)
                        {
                            // Saves the color.
                            colorArr[r, c] = oldColors[indexArr[r, c]];

                            // Spot is valid.
                            validArr[r, c] = true;
                        }
                        else
                        {
                            // Save a clear colour. This is redundant, but for clarity it's still done.
                            colorArr[r, c] = Color.clear;

                            // Spot is invalid.
                            validArr[r, c] = false;
                        }
                    }
                }


                // The colour vector4. XYZW = RGBA.
                Vector4 colorVec4 = new Vector4();

                // This isn't necessary, but it's being left in.
                // The value count.
                int valueCount = 0;

                // Multiplies each colour component by the kernel value and adds them together.
                for (int r = 0; r < colorArr.GetLength(0); r++) // Row
                {
                    for (int c = 0; c < colorArr.GetLength(1); c++) // Col
                    {
                        // Sums the colour values.
                        colorVec4.x += (colorArr[r, c].r * kernel[r, c]);
                        colorVec4.y += (colorArr[r, c].g * kernel[r, c]);
                        colorVec4.z += (colorArr[r, c].b * kernel[r, c]);
                        colorVec4.w += (colorArr[r, c].a * kernel[r, c]);

                        // If the spot is valid, then increase the value count.
                        if (validArr[r, c])
                            valueCount++;
                    }
                }

                // Divide the vector by the value count (wrong method, so it goes unused).
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

        // Sets kernel using the provided array. The array must be 3x3.
        public void SetKernel(float[,] arr)
        {
            // The length is wrong.
            if (arr.Length != 9)
            {
                Debug.LogError("This is not a 3x3 array. The kernel could not be set.");
                return;
            }

            // Row 0
            kernelRow0.x = arr[0, 0];
            kernelRow0.y = arr[0, 1];
            kernelRow0.z = arr[0, 2];

            // Row 1
            kernelRow1.x = arr[1, 0];
            kernelRow1.y = arr[1, 1];
            kernelRow1.z = arr[1, 2];

            // Row 2
            kernelRow2.x = arr[2, 0];
            kernelRow2.y = arr[2, 1];
            kernelRow2.z = arr[2, 2];
        }
    }
}