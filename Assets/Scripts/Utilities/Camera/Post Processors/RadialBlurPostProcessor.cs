using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace util
{
    // Radial blur post processor.
    public class RadialBlurPostProcessor : PostProcessor
    {
        [Header("Radial Blur")]

        // ID used for the center of the radial effect.
        public string radialCenterID = "_RadialCenter";

        // The radial center, which puts the image in a uv-style 0-1 space.
        // (0.5, 0.5) is the center of the image.
        public Vector2 radialCenter = new Vector2(0.5F, 0.5F);

        // THe angle ID for the radial effect.
        public string radialAngleID = "_RadialAngle";

        // The angle of the radial effect. By default, Unity works in radians.
        public float radialAngle = 0.0F;

        // Determines if the provided angle is in degrees (true) or radians (false).
        // This is applied to the angle BEFORE it's put into the shader.
        // The shader uses radians.
        [Tooltip("Sets whether the angle is in degrees or radians. If true, a conversion is done since the shader uses radians.")]
        public bool inDegrees = true;

        // ID to determine if the radial blur goes clockwise or counter-clockwise.
        public string clockwiseID = "_Clockwise";

        // Determines if it the radial turns clockwise (true) or counter-clockwise (false).
        public bool clockwise = true;

        // The number of sampels used for the radial effect.
        public string sampleCountID = "_SampleCount";

        // The number of samples used for the radial blur effect.
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
            // Gets the values from the shader.
            // Vector
            Vector4 radialCenterShader = postMaterial.GetVector(radialCenterID);
            
            // Gets the angle, and does a conversion if needed to check if the script's saved angle is the same...
            // As the angle in the shader.
            float angleShader = postMaterial.GetFloat(radialAngleID);
            float angleScript = (inDegrees) ? Mathf.Deg2Rad * radialAngle : radialAngle;

            // Clockwise and samples.
            int clockwiseShader = postMaterial.GetInt(clockwiseID);
            int sampleCountShader = postMaterial.GetInt(sampleCountID);

            // If one of the values have changed, update the material with the new values.
            if (radialCenterShader != (Vector4)radialCenter || angleShader != angleScript
                || clockwiseShader != Convert.ToInt32(clockwise) || sampleCountShader != sampleCount)
            {
                SetValuesToMaterial();
            }

            base.OnRenderImage(source, destination);
        }
        // Sets the textures to be used for color grading.
        public void SetValuesToMaterial()
        {
            // Checks if the angle is in degrees or radians.
            // If it's in degrees, the angle is converted before it's sent to the shader.
            float shaderAngle = (inDegrees) ? Mathf.Deg2Rad * radialAngle : radialAngle;

            // Sets the radial center.
            postMaterial.SetVector(radialCenterID, radialCenter);

            // Sets the radial angle.
            postMaterial.SetFloat(radialAngleID, shaderAngle);

            // Sets whether the radial turns clockwise (true) or counter-clockwise (false).
            postMaterial.SetInt(clockwiseID, Convert.ToInt32(clockwise));

            // Sets the sample count.
            postMaterial.SetInt(sampleCountID, sampleCount);
        }
    }
}