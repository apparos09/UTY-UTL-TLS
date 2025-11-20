using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Inverts the colours for the camera render.
    public class InvertRenderFilter : RenderFilterer
    {
        // Filters the camera render as a texture 2D to invert the colours.
        public override Texture2D FilterRenderAsTexture2D(Texture2D texture2D)
        {
            // Example - invert the colours of the texture 2D pixels.
            // The colors.
            Color[] colors = texture2D.GetPixels();
            
            // Inverts the colours.
            for(int i = 0; i < colors.Length; i++)
            {
                // The inversion colour. The alpha is 0 so that it isn't changed.
                Color invert = Color.white;
                invert.a = 0;
            
                // Gets the new colours.
                colors[i] = invert - colors[i];
            }
            
            // Sets the new colours and applies them.
            texture2D.SetPixels(colors);
            texture2D.Apply();
            
            // Returns the result.
            return texture2D;
        }
    }
}