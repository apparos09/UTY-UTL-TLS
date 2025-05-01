using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Sources:
 * - https://discussions.unity.com/t/faster-way-to-alter-a-texture2d-besides-texture2d-apply/787780/4
 * - https://docs.unity3d.com/es/530/Manual/SL-DataTypesAndPrecision.html
 */

namespace util
{
    // The kernel post processor for the camera.
    public class CameraKernelPostProcessor : CameraPostProcessor
    {
        [Header("Kernel")]

        // The number of passes the kernel does.
        // public int passes = 1;

        // [Header("Kernel Array")]
        // The three kernel rows (defaults to identity).
        public Vector3 kernelRow0 = new Vector3(0, 0, 0);
        public Vector3 kernelRow1 = new Vector3(0, 1, 0);
        public Vector3 kernelRow2 = new Vector3(0, 0, 0);

        // The width and height of the render texture.
        protected const string RENDER_TEXTURE_WIDTH_KEY = "_MainTexWidth";
        protected const string RENDER_TEXTURE_HEIGHT_KEY = "_MainTexHeight";

        // The key names for the kernel rows.
        protected const string KERNEL_ROW0_KEY = "_KernelRow0";
        protected const string KERNEL_ROW1_KEY = "_KernelRow1";
        protected const string KERNEL_ROW2_KEY = "_KernelRow2";

        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();

            // Sets the kernels to start.
            SetKernelToMaterial();
        }

        // OnRenderImage is called after all rendering is complete to render image
        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // TODO: optimize.
            SetKernelToMaterial();

            // Sets the width and height of the main texture.
            // The source width and height are the same as the render texture's width and height.
            postMaterial.SetFloat(RENDER_TEXTURE_WIDTH_KEY, source.width);
            postMaterial.SetFloat(RENDER_TEXTURE_HEIGHT_KEY, source.height);

            base.OnRenderImage(source, destination);
        }

        // Sets the knerles to the material.
        protected void SetKernelToMaterial()
        {
            // Sets the initial kernels.
            postMaterial.SetVector(KERNEL_ROW0_KEY, kernelRow0);
            postMaterial.SetVector(KERNEL_ROW1_KEY, kernelRow1);
            postMaterial.SetVector(KERNEL_ROW2_KEY, kernelRow2);
        }
    }
}