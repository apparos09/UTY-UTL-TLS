using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Radial blur post processor.
    public class RadialBlurPostProcessor : PostProcessor
    {
        // ID used for the center of the radial effect.
        public string radialCenterID = "_RadialCenter";

        // The radial center, which puts the image in a uv-style 0-1 space.
        // (0.5, 0.5) is the center of the image.
        public Vector2 radialCenter = new Vector2(0.5F, 0.5F);

        // THe angle ID for the radial effect.
        public string angleID = "_Angle";

        // The angle of the radial effect. By default, Unity works in radians.
        public float angle = 10.0F;

        // ID for value that determines if the angle is in degrees or radians.
        public string inDegreesID = "_InDegrees";

        // Determines if the provided angle is in degrees (true) or radians (false).
        public bool inDegrees = true;

        // The number of sampels used for the radial effect.
        public string sampleCountID = "_SampleCount";

        // The number of samples used for the radial blur effect.
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
            Vector4 radialCenterShader = postMaterial.GetVector(radialCenterID);
            float radialLengthShader = postMaterial.GetFloat(angleID);
            int sampleCountShader = postMaterial.GetInt(sampleCountID);

            // If one of the values have changed, update the material with the new values.
            if (radialCenterShader != (Vector4)radialCenter || radialLengthShader != angle || sampleCountShader != sampleCount)
            {
                SetValuesToMaterial();
            }

            base.OnRenderImage(source, destination);
        }
        // Sets the textures to be used for color grading.
        public void SetValuesToMaterial()
        {
            // Sets the radial center.
            postMaterial.SetVector(radialCenterID, radialCenter);

            // Sets the radial angle.
            postMaterial.SetFloat(angleID, angle);

            // Sets if the angle is in degrees or radians.
            postMaterial.SetInt(inDegreesID, Convert.ToInt32(inDegrees));

            // Sets the sample count.
            postMaterial.SetInt(sampleCountID, sampleCount);
        }
    }
}