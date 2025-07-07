using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Sources:
 * - https://setosa.io/ev/image-kernels/
 * - https://www.aforgenet.com/framework/features/convolution_filters/
 * - https://discussions.unity.com/t/faster-way-to-alter-a-texture2d-besides-texture2d-apply/787780/4
 * - https://docs.unity3d.com/es/530/Manual/SL-DataTypesAndPrecision.html
 */

namespace util
{
    // The kernel post processor for the camera.
    public class CameraKernelPostProcessor : CameraPostProcessor
    {
        // The saved post process state state. The shader should only be updated if the state has changed.
        protected struct ProcessState
        {
            public float renderWidth;
            public float renderHeight;

            public Vector3 kernelRow0;
            public Vector3 kernelRow1;
            public Vector3 kernelRow2;
        }

        [Header("Kernel")]

        // [Header("Kernel Array")]
        // The three kernel rows (defaults to identity).
        public Vector3 kernelRow0 = new Vector3(0, 0, 0);
        public Vector3 kernelRow1 = new Vector3(0, 1, 0);
        public Vector3 kernelRow2 = new Vector3(0, 0, 0);

        // The last state of the kernel post process.
        private ProcessState lastState = new ProcessState();

        // The width and height of the render texture.
        protected const string RENDER_TEXTURE_WIDTH_KEY = "_MainTexWidth";
        protected const string RENDER_TEXTURE_HEIGHT_KEY = "_MainTexHeight";

        // The key names for the kernel rows.
        protected const string KERNEL_ROW0_KEY = "_KernelRow0";
        protected const string KERNEL_ROW1_KEY = "_KernelRow1";
        protected const string KERNEL_ROW2_KEY = "_KernelRow2";


        // DEFAULT KERNELS
        // The row and colum count for the kernels.
        private const int KERNEL_ROW_COUNT = 3;
        private const int KERNEL_COLUMN_COUNT = 3;

        // The identity kernel (no changes)
        public static float[,] identityKernel = new float[3,3]
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


        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();

            // Sets the width and height to -1 so that the state registered as needing an update.
            lastState.renderWidth = -1;
            lastState.renderHeight = -1;

            // Sets the kernels to start.
            SetKernelToMaterial();
        }

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

        // OnRenderImage is called after all rendering is complete to render image
        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Checks if the state has changed.
            bool stateChanged = HasProcessStateChanged(source.width, source.height);

            // If the state has change,d update the material.
            if(stateChanged)
            {
                // Set the kernel to the material.
                SetKernelToMaterial();

                // Sets the width and height of the main texture.
                // The source width and height are the same as the render texture's width and height.
                postMaterial.SetFloat(RENDER_TEXTURE_WIDTH_KEY, source.width);
                postMaterial.SetFloat(RENDER_TEXTURE_HEIGHT_KEY, source.height);

            }

            // Call base function to render the image.
            base.OnRenderImage(source, destination);

            // Updates the last process state if the state has changed.
            if(stateChanged)
            {
                UpdateProcessState(source.width, source.height);
            }
        }

        // Sets the kernels to the material.
        protected void SetKernelToMaterial()
        {
            // Sets the initial kernels.
            postMaterial.SetVector(KERNEL_ROW0_KEY, kernelRow0);
            postMaterial.SetVector(KERNEL_ROW1_KEY, kernelRow1);
            postMaterial.SetVector(KERNEL_ROW2_KEY, kernelRow2);
        }

        // Sets kernel using the provided array. The array must be 3x3.
        public void SetKernel(float[,] arr)
        {
            // The length is wrong.
            if(arr.Length != 9)
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

        // Checks if the process state has changed.
        private bool HasProcessStateChanged(float renderWidth, float renderHeight)
        {
            // If the width or height has changed, then the state has changed.
            if(lastState.renderWidth != renderWidth || lastState.renderHeight != renderHeight)
            {
                return true;
            }

            // If one of the kernels have changed, the process state has changed.
            if(
                lastState.kernelRow0 != kernelRow0 || 
                lastState.kernelRow1 != kernelRow1 || 
                lastState.kernelRow2 != kernelRow2
                )
            {
                return true;
            }

            // No changes.
            return false;
        }

        // Updates the process state.
        private void UpdateProcessState(float renderWidth, float renderHeight)
        {
            // Saves the last state.
            lastState.renderWidth = renderWidth;
            lastState.renderHeight = renderHeight;

            // Kernels
            lastState.kernelRow0 = kernelRow0;
            lastState.kernelRow1 = kernelRow1;
            lastState.kernelRow2 = kernelRow2;
        }

    }
}