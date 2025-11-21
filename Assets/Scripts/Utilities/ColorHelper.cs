using UnityEngine;

namespace util
{
    // Functions used to assit with colors.
    public class ColorHelper : MonoBehaviour
    {
        // Converts a color from 0-1 to 0-255. XYZW = RGBA.
        public static Vector4 ConvertColor01To0255(Color color)
        {
            // The new color.
            Vector4 newColor = new Vector4(color.r * 255.0F, color.g * 255.0F, color.b * 255.0F, color.a);
            newColor.x = Mathf.Clamp(newColor.x, 0.0F, 255.0F);
            newColor.y = Mathf.Clamp(newColor.y, 0.0F, 255.0F);
            newColor.z = Mathf.Clamp(newColor.z, 0.0F, 255.0F);

            return newColor;
        }

        // Converts a color from 0-1 to 0-255. XYZW = RGBA.
        public static Vector4 ConvertColor01To0255(float r, float g, float b, float a)
        {
            Color newColor = new Color(r, g, b, a);
            return ConvertColor01To0255(newColor);
        }

        // Inverts the provided color.
        public static Color InvertColor(Color color)
        {
            // Calculates the inverted color.
            Color inverted = color;
            inverted.r = Mathf.Abs(color.r - 1.0F);
            inverted.g = Mathf.Abs(color.g - 1.0F);
            inverted.b = Mathf.Abs(color.b - 1.0F);

            // Returns the inverted color.
            return inverted;
        }

    }
}