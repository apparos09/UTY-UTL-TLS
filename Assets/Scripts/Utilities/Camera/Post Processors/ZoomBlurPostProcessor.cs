using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Zoom blur post processor.
    public class ZoomBlurPostProcessor : PostProcessor
    {
        [Header("Zoom Blur")]

        // The screen resolution ID. This is automatically filled using the screen resolution given by Unity.
        [Tooltip("The screen resolution ID. The screen resolution is automatically provided to the shader.")]
        public string screenResolutionID = "_ScreenResolution";

        // ID used for the center of the zoom effect.
        public string zoomCenterID = "_ZoomCenter";

        // The zoom center, which puts the image in a uv-style 0-1 space.
        // (0.5, 0.5) is the center of the image.
        public Vector2 zoomCenter = new Vector2(0.5F, 0.5F);

        // The ID for the length of the zoom effect.
        public string zoomLengthID = "_ZoomLength";

        // The length of the zoom effect.
        public float zoomLength = 0.0F;

        // The ID for the zoom in.
        public string zoomInID = "_ZoomIn";

        // Determines if the shader zooms in or out.
        public bool zoomIn = true;

        // The ID for the number of sampels used for the zoom effect.
        public string sampleCountID = "_SampleCount";

        // The number of samples used for the zoom blur effect.
        public int sampleCount = 0;

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
            // Gets the screen resolution.
            Vector2 screenRes = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

            // Gets the values from the shader.
            Vector4 screenResShader = postMaterial.GetVector(screenResolutionID);
            Vector4 zoomCenterShader = postMaterial.GetVector(zoomCenterID);
            float zoomLengthShader = postMaterial.GetFloat(zoomLengthID);
            int zoomInShader = postMaterial.GetInt(zoomInID);
            int sampleCountShader = postMaterial.GetInt(sampleCountID);

            // If one of the values have changed, update the material with the new values.
            if (screenResShader != (Vector4)screenRes ||
                zoomCenterShader != (Vector4)zoomCenter || zoomLengthShader != zoomLength || 
                zoomInShader != Convert.ToInt32(zoomIn) || sampleCountShader != sampleCount)
            {
                SetValuesToMaterial();
            }

            base.OnRenderImage(source, destination);
        }
        // Sets the textures to be used for color grading.
        public void SetValuesToMaterial()
        {
            // Gets the screen resolution and puts it in a vector.
            Vector2 screenResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

            // Sets the screen resolution to the shader.
            postMaterial.SetVector(screenResolutionID, screenResolution);

            // Sets the zoom center.
            postMaterial.SetVector(zoomCenterID, zoomCenter);

            // Sets the zoom length.
            postMaterial.SetFloat(zoomLengthID, zoomLength);

            // Sets if the shader is zooming in (true) or out (false).
            postMaterial.SetInt(zoomInID, Convert.ToInt32(zoomIn));

            // Sets the sample count.
            postMaterial.SetInt(sampleCountID, sampleCount);
        }
    }
}