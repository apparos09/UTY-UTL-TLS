using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Zoom blur post processor.
    public class ZoomBlurEffectShader : PostProcessor
    {
        // ID used for the center of the zoom effect.
        public string zoomCenterID = "_ZoomCenter";

        // The zoom center, which puts the image in a uv-style 0-1 space.
        // (0.5, 0.5) is the center of the image.
        public Vector2 zoomCenter = new Vector2(0.5F, 0.5F);

        // The ID for the length of the zoom effect.
        public string zoomLengthID = "_ZoomLength";

        // The length of the zoom effect.
        public float zoomLength = 10.0F;

        // The ID for the number of sampels used for the zoom effect.
        public string sampleCountID = "_SampleCount";

        // The number of samples used for the zoom blur effect.
        public int sampleCount = 10;

        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();

            // Set the textures to the material.
            SetValuesToMaterial();
        }

        // OnRenderImage is called after all rendering is complete to render image
        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Gets the values from the shader.
            Vector4 zoomCenterShader = postMaterial.GetVector(zoomCenterID);
            float zoomLengthShader = postMaterial.GetFloat(zoomLengthID);
            int sampleCountShader = postMaterial.GetInt(sampleCountID);

            // If one of the values have changed, update the material with the new values.
            if (zoomCenterShader != (Vector4)zoomCenter || zoomLengthShader != zoomLength || sampleCountShader != sampleCount)
            {
                SetValuesToMaterial();
            }

            base.OnRenderImage(source, destination);
        }
        // Sets the textures to be used for color grading.
        public void SetValuesToMaterial()
        {
            // Sets the zoom center.
            postMaterial.SetVector(zoomCenterID, zoomCenter);

            // Sets the zoom length.
            postMaterial.SetFloat(zoomLengthID, zoomLength);

            // Sets the sample count.
            postMaterial.SetInt(sampleCountID, sampleCount);
        }
    }
}