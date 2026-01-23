using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // The canvas for the loading graphic.
    // TODO: the async scene loader probably shouldn't be attached to the graphic.
    public class LoadingSceneCanvas : MonoBehaviour
    {
        // The singleton instance.
        private static LoadingSceneCanvas instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The canvas this script is attached to.
        public Canvas canvas;

        // The loading scene graphic.
        public LoadingSceneGraphic loadingGraphic;

        // If 'true', the loading graphic is used.
        private const bool USE_LOADING_GRAPHIC = true;

        // If 'true', the loading screen canvas isn't destroyed on load. This is applied in Start()
        [Tooltip("If 'true', this object is set to not be destroyed on load. This is set in the Start() function.")]
        public bool dontDestroyOnLoad = true;

        // Constructor
        private LoadingSceneCanvas()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Canvas is not set.
            if (canvas == null)
            {
                // Tries to set the canvas.
                if (!TryGetComponent(out canvas))
                {
                    Debug.LogWarning("The canvas component could not be found.");
                }
            }

            // If this is the instance, apply certain functions.
            if(instance == this)
            {
                // Don't destroy this object.
                // This is done because this graphic is used for loading scenes.
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        // Gets the instance.
        public static LoadingSceneCanvas Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<LoadingSceneCanvas>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Loading Graphic Canvas (singleton)");
                        instance = go.AddComponent<LoadingSceneCanvas>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Returns 'true' if the loading screen graphic should be used.
        // Returns 'false' if the loading screen graphic isn't being used...
        // The canvas is null or the loadingGraphic is null.
        // TODO: if this returns false, the loading scene won't work, since it relies on the loading scene graphic.
        // Either make this clearer, or move the loading functions to the loading scene canvas.
        public bool IsUsingLoadingGraphic()
        {
            // If loading should be used, and the related objects exist.
            return USE_LOADING_GRAPHIC && canvas != null && loadingGraphic != null;
        }

        // If the object has been instantiated, and the loading screen is being used.
        public static bool IsInstantiatedAndUsingLoadingGraphic()
        {
            // Checks if instantiated.
            if (Instantiated)
            {
                // Checks if the loading screen is being used.
                return Instance.IsUsingLoadingGraphic();
            }
            else
            {
                return false;
            }
        }

        // Gets the next scene.
        public string GetNextScene()
        {
            return loadingGraphic.nextScene;
        }

        // Sets the next scene.
        public void SetNextScene(string sceneName)
        {
            loadingGraphic.nextScene = sceneName;
        }

        // Loads the scene using the loading scene graphic.
        public void LoadScene()
        {
            // Plays the loading screen opening animation.
            // If there is no opening animation, it calls start and end.
            loadingGraphic.PlayLoadingGraphicOpeningAnimation();
        }

        // Sets the scene name and runs the loading screen.
        public void LoadScene(string sceneName)
        {
            SetNextScene(sceneName);
            LoadScene();
        }

        // Returns 'true' if the animation is playing.
        public bool IsAnimationPlaying()
        {
            return loadingGraphic.IsAnimationPlaying();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }

    }
}