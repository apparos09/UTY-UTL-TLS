using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Mirrors the rendered image.
    public class FlipPostProcessor : PostProcessor
    {
        [Header("Flip")]

        // ID used for flipping on the x-axis.
        public string flipXID = "_FlipX";

        // Flip X
        public bool flipX = false;

        // ID used for flipping on the y-axis.
        public string flipYID = "_FlipY";

        // Flip Y
        public bool flipY = false;

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
            int flipXShader = postMaterial.GetInt(flipXID);
            int flipYShader = postMaterial.GetInt(flipYID);

            // If one of the values have changed, update the material with the new values.
            if(flipXShader != Convert.ToInt32(flipX) || flipYShader != Convert.ToInt32(flipY))
            {
                SetValuesToMaterial();
            }

            base.OnRenderImage(source, destination);
        }
        // Sets the textures to be used for color grading.
        public void SetValuesToMaterial()
        {
            // Sets the flip values for the post processor by converting them from bools to ints.
            // 0 = false, 1 = true. Anything that's not 0 registers as true.
            int flipXInt = Convert.ToInt32(flipX);
            int flipYInt = Convert.ToInt32(flipY);

            // Sets the integers.
            postMaterial.SetInt(flipXID, flipXInt);
            postMaterial.SetInt(flipYID, flipYInt);
        }
    }
}