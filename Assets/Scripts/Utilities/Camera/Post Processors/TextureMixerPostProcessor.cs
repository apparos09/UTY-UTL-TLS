using UnityEngine;

namespace util
{
    // Mixes a texture in with the game image.
    public class TextureMixerPostProcessor : PostProcessor
    {
        [Header("TextureMixer")]

        // Mix Texture
        public string mixTextureID = "_MixTexture";
        public Texture2D mixTexture;

        // Mix T
        // This uses a lerp calculation.
        public string mixTID = "_MixT";

        [Range(0.0F, 1.0F)]
        public float mixT = 0.0F;

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
            // Needs to be set every frame for some reason.
            SetValuesToMaterial();

            base.OnRenderImage(source, destination);
        }

        // Sets the textures to be used for texture mixing.
        public void SetValuesToMaterial()
        {
            postMaterial.SetTexture(mixTextureID, mixTexture);
            postMaterial.SetFloat(mixTID, mixT);
        }
    }
}