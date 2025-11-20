using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Sources:
 *  - https://www.youtube.com/watch?v=ahplcYCmfG0
 *  - https://stackoverflow.com/questions/44264468/convert-rendertexture-to-texture2d
 *  - https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Texture2D-ctor.html
 *  - http://docs.unity3d.com/6000.0/Documentation/ScriptReference/Graphics.CopyTexture.html
 *  - https://gist.github.com/openroomxyz/b7221ed30a0a0e04c32ae6d5fa948ac9
 */

namespace util
{
    // Filters the render image from the camera. This operates without the use of a shader by default.
    // This should NOT be used since it's VERY INEFFICIENT.
    // Use the CameraPostProcessor class with a shader instead.
    public class RenderFilterer : MonoBehaviour
    {
        // The camera the post processor is attached to.
        public new Camera camera;

        // Awake is called when the script instance is being loaded
        protected virtual void Awake()
        {
            TrySetCamera();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // OnRenderImage is called after all rendering is complete to render image
        protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // The input texture 2D.
            Texture2D inputTexture2D = RenderTextureToTexture2D(source);

            // The output texture 2D. Make 
            Texture2D outputTexture2D = FilterRenderAsTexture2D(inputTexture2D);

            // e.g. copies the source texture into the destination texture.
            // Graphics.Blit(source, destination);

            // Blits the output texture 2D to the destination.
            Graphics.Blit(outputTexture2D, destination);

            // Destroys the input and output texture 2D objects.
            // These are likely the same textuer2D object, but it doesn't error out...
            // So calling Destroy twice should be okay.
            Destroy(inputTexture2D);
            Destroy(outputTexture2D);
        }

        // Tries to set the camera the post processor is attached to.
        // Returns 'true' if the camera is set. Returns false if the camera...
        // Couldn't be set. If 'overwrite' is false, the camera object won't be overwritten...
        // If one is saved. Such a case will return false.
        public bool TrySetCamera(bool overwrite = false)
        {
            // The result.
            bool result;

            // Checks if the saved camera should be overwritten.
            if (overwrite)
            {
                // Tries to get the camera. This overwrites the set camera if it exists.
                result = TryGetComponent(out camera);
            }
            else
            {
                // If the camera is null, try to get the component.
                // If it's set, return false, since it shouldn't be overwritten.
                if (camera == null)
                    result = TryGetComponent(out camera);
                else
                    result = false;
            }

            return result;
        }

        // Convert the provided render texture to a texture 2D.
        public Texture2D RenderTextureToTexture2D(RenderTexture renderTexture)
        {
            // Read Pixels Method
            // The 2D texture result.
            Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height);
            
            // Saves the active render texture before setting it to the passed texture.
            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            
            // Reads the pixels from the render texture.
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            
            // Applies the read pixels.
            texture2D.Apply();
            
            // Sets the active render texture back to the original.
            RenderTexture.active = activeRenderTexture;
            
            // Return the texture.
            return texture2D;
        }

        // Filters the camera render as a texture 2D.
        // Remember to apply the changed pixels to the texture2D after you're done.
        public virtual Texture2D FilterRenderAsTexture2D(Texture2D texture2D)
        {
            return texture2D;

            // // Example - invert the colours of the texture 2D pixels.
            // // The colors.
            // Color[] colors = texture2D.GetPixels();
            // 
            // // Inverts the colours.
            // for(int i = 0; i < colors.Length; i++)
            // {
            //     // The inversion colour. The alpha is 0 so that it isn't changed.
            //     Color invert = Color.white;
            //     invert.a = 0;
            // 
            //     // Gets the new colours.
            //     colors[i] = invert - colors[i];
            // }
            // 
            // // Sets the new colours and applies them.
            // texture2D.SetPixels(colors);
            // texture2D.Apply();
            // 
            // // Returns the result.
            // return texture2D;
        }

        // Update is called once per frame
        protected void Update()
        {
            // ...
        }
    }
}