using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // A camera tint post processor for a camera.
    public class TintPostProcessor : PostProcessor
    {
        [Header("Tint")]

        // The name of the tint variable that's being changed.
        public string tintNameId = "_Tint";

        // The tint for the post processing effect.
        public Color tint = Color.white;

        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();

            // Sets the initial tint.
            postMaterial.SetColor(tintNameId, tint);
        }

        // OnRenderImage is called after all rendering is complete to render image
        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Gets the material's tint.
            Color matTint = postMaterial.GetColor(tintNameId);

            // If the tints don't match, change the tint.
            if (matTint != tint)
                postMaterial.SetColor(tintNameId, tint);

            base.OnRenderImage(source, destination);
        }
    }
}