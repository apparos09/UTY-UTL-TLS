using UnityEngine;

namespace util
{
    // Alters the colour of the image using color grading.
    public class ColorGradingPostProcessor : PostProcessor
    {
        [Header("ColorGrading")]

        // If 'true', a single color grade is being used.
        // IF 'false', the grade is split between RGB.
        public bool singleGradeMode = true;

        // ID used for setting single grade mode.
        public string singleGradeModeId = "_SingleGradeMode";

        [Header("ColorGrading/Texture IDs")]
        // All in 1 color grade.
        public string colorGradeId = "_ColorGrade";

        // RGB
        public string colorGradeRedId = "_ColorGradeRed";
        public string colorGradeGreenId = "_ColorGradeGreen";
        public string colorGradeBlueId = "_ColorGradeBlue";

        [Header("ColorGrading/Textures")]
        // Combined color grade.
        public Texture2D colorGrade;

        // The three colour grading textures.
        public Texture2D colorGradeRed;
        public Texture2D colorGradeGreen;
        public Texture2D colorGradeBlue;

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

            // Converts single grade mode to a float value. 1.0 is true, 0.0 is false.
            float singleGradeModeFloat = singleGradeMode ? 1.0F : 0.0F;

            // The texture mode needs to be changed.
            if(postMaterial.GetFloat(singleGradeModeId) != singleGradeModeFloat)
            {
                postMaterial.SetFloat(singleGradeModeId, singleGradeModeFloat);
            }

            base.OnRenderImage(source, destination);
        }

        // Better to manually set them.
        // Sets the combined color grade texture.
        protected void SetColorGradeTexture(Texture2D colorGrade)
        {
            this.colorGrade = colorGrade;
            SetValuesToMaterial();
        }

        // Better to manually set them.
        // Sets the textures to be used for color grading.
        protected void SetColorGradeTextures(Texture2D red, Texture2D green, Texture2D blue)
        {
            // Sets the three textures.
            colorGradeRed = red;
            colorGradeGreen = green;
            colorGradeBlue = blue;

            // Sets the textures in the material.
            SetValuesToMaterial();
        }

        // Sets the textures to be used for color grading.
        public void SetValuesToMaterial()
        {
            // Setting the mode.
            float singleGradeModeFloat = singleGradeMode ? 1.0F : 0.0F;
            postMaterial.SetFloat(singleGradeModeId, singleGradeModeFloat);

            // Setting the texture seems to work, but not getting the texture.
            // Combined color grade.
            if(singleGradeMode)
            {
                postMaterial.SetTexture(colorGradeId, colorGrade);
            }
            else
            {
                // Sets the color grading textures. Data type sampler2D in the shader.
                postMaterial.SetTexture(colorGradeRedId, colorGradeRed);
                postMaterial.SetTexture(colorGradeGreenId, colorGradeGreen);
                postMaterial.SetTexture(colorGradeBlueId, colorGradeBlue);
            }
        }
    }
}