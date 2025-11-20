using Unity.Android.Gradle;
using UnityEngine;

namespace util
{
    // Alters the colour of the image using color grading.
    public class ColorGradingPostProcessor : PostProcessor
    {
        [Header("Texture IDs")]
        public string colorGradeRedId = "_ColorGradeRed";
        public string colorGradeGreenId = "_ColorGradeGreen";
        public string colorGradeBlueId = "_ColorGradeBlue";

        [Header("Textures")]
        // The three colour grading textures.
        public Texture2D colorGradeRed;
        public Texture2D colorGradeGreen;
        public Texture2D colorGradeBlue;

        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();

            // Set the textures to the material.
            SetColorGradeTexturesToMaterial();
        }

        // OnRenderImage is called after all rendering is complete to render image
        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Gets the material's tint.
            Texture2D red = (Texture2D)postMaterial.GetTexture(colorGradeRedId);
            Texture2D green = (Texture2D)postMaterial.GetTexture(colorGradeGreenId);
            Texture2D blue = (Texture2D)postMaterial.GetTexture(colorGradeBlueId);

            // If one of the textures don't match, update them.
            if (red != colorGradeRed || green != colorGradeGreen || blue != colorGradeBlue)
            {
                SetColorGradeTexturesToMaterial();
            }

            base.OnRenderImage(source, destination);
        }

        // Sets the textures to be used for color grading.
        public void SetColorGradeTextures(Texture2D red, Texture2D green, Texture2D blue)
        {
            // Sets the three textures.
            colorGradeRed = red;
            colorGradeGreen = green;
            colorGradeBlue = blue;

            // Sets the textures in the material.
            SetColorGradeTexturesToMaterial();
        }

        // Sets the textures to be used for color grading.
        public void SetColorGradeTexturesToMaterial()
        {
            // Sets the color grading textures.
            postMaterial.SetTexture(colorGradeRedId, colorGradeRed);
            postMaterial.SetTexture(colorGradeGreenId, colorGradeGreen);
            postMaterial.SetTexture(colorGradeBlueId, colorGradeBlue);
        }
    }
}