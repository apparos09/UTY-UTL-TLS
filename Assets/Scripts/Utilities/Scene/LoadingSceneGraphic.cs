using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace util
{
    // The loading scene graphic.
    public class LoadingSceneGraphic : MonoBehaviour
    {
        // The scene that is getting loaded.
        public string nextScene = "";

        // The loader for loading scenes asynchronously.
        public AsyncSceneLoader asyncLoader;

        // Loads the next scene on Loading Graphic - Opening End.
        [Tooltip("If true, the next scene is loaded at the end of the opening animation.")]
        public bool loadNextScene = true;

        // Loads the scene asynchronously if true.
        [Tooltip("If 'true', the scene is loaded asynchronously.")]
        public bool loadSceneAsync = true;

        // CALLBACKS
        // A callback for the loading screen.
        public delegate void LoadingCallback();

        // Callback for the start of an animation.
        private LoadingCallback animStartCallback;

        // Callback for the end of an animation.
        private LoadingCallback animEndCallback;

        // A callback for the start of the loading screen opening.
        private LoadingCallback openingStartCallback;

        // A callback for the end of the loading screen opening.
        private LoadingCallback openingEndCallback;

        // A callback for the start of the loading screen closing.
        private LoadingCallback closingStartCallback;

        // A callback for the end of the loading screen closing.
        private LoadingCallback closingEndCallback;

        [Header("Animation")]
        // The animator for the loading screen.
        public Animator animator;

        // Gets set to 'true' when an animation is playing.
        private bool animPlaying = false;

        // Opening, loading, and closing animations.

        // Loading opening animation. This is the opening portion of the loading animation.
        public string openingAnim = "";

        // Loading progress animation. This plays when a scene is loading.
        public string progressAnim = "";

        // Loading closing animation. This is the ending portion of the loading animation.
        public string closingAnim = "";

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // If the animator is not set, set it.
            if (animator == null)
                animator = GetComponent<Animator>();

            // Gets the asynchronous loader.
            if (asyncLoader == null)
            {
                asyncLoader = GetComponent<AsyncSceneLoader>();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // Returns 'true' if the async loader is loading.
        public bool IsLoading
        {
            get
            {
                return asyncLoader.IsLoading;
            }

        }

        // CALLBACKS
        // Adds a callback for when an animation starts.
        public void OnAnimationStartAddCallback(LoadingCallback callback)
        {
            animStartCallback += callback;
        }

        // Removes a callback for when an animation starts.
        public void OnAnimationStartRemoveCallback(LoadingCallback callback)
        {
            animStartCallback -= callback;
        }

        // Adds a callback for when an animation ends.
        public void OnAnimationEndAddCallback(LoadingCallback callback)
        {
            animEndCallback += callback;
        }

        // Removes a callback for when an animation ends.
        public void OnAnimationEndRemoveCallback(LoadingCallback callback)
        {
            animEndCallback -= callback;
        }

        // Opening
        // Adds a callback for when the loading screen opening starts.
        public void OnLoadingScreenOpeningStartAddCallback(LoadingCallback callback)
        {
            openingStartCallback += callback;
        }

        // Removes a callback for when the loading screen opening starts.
        public void OnLoadingScreenOpeningStartRemoveCallback(LoadingCallback callback)
        {
            openingStartCallback -= callback;
        }

        // Adds a callback for when the loading screen opening ends.
        public void OnLoadingScreenOpeningEndAddCallback(LoadingCallback callback)
        {
            openingEndCallback += callback;
        }

        // Removes a callback for when the loading screen opening ends.
        public void OnLoadingScreenOpeningEndRemoveCallback(LoadingCallback callback)
        {
            openingEndCallback -= callback;
        }



        // Closing
        // Adds a callback for when the loading screen closing starts.
        public void OnLoadingScreenClosingStartAddCallback(LoadingCallback callback)
        {
            closingStartCallback += callback;
        }

        // Removes a callback for when the loading screen closing starts.
        public void OnLoadingScreenClosingStartRemoveCallback(LoadingCallback callback)
        {
            closingStartCallback -= callback;
        }

        // Adds a callback for when the loading screen closing ends.
        public void OnLoadingScreenClosingEndAddCallback(LoadingCallback callback)
        {
            closingEndCallback += callback;
        }

        // Removes a callback for when the loading screen closing ends.
        public void OnLoadingScreenClosingEndRemoveCallback(LoadingCallback callback)
        {
            closingEndCallback -= callback;
        }


        // ANIMATIONS 
        // Returns 'true' if the animation is playing.
        public bool IsAnimationPlaying()
        {
            return animPlaying;
        }

        // Called when an animation starts.
        protected virtual void OnAnimationStart()
        {
            animPlaying = true;

            // Trigger the callbacks.
            if (animStartCallback != null)
                animStartCallback();
        }

        // Called when an animation ends.
        protected virtual void OnAnimationEnd()
        {
            animPlaying = false;

            // Trigger the callbacks.
            if (animEndCallback != null)
                animEndCallback();
        }

        // Plays the loading graphic opening animation.

        public void PlayLoadingGraphicOpeningAnimation()
        {
            // If there is an opening animation, play it.
            if (openingAnim != string.Empty)
                animator.Play(openingAnim);
        }

        // Loading Graphic - Opening Start
        public void OnLoadingGraphicOpeningStart()
        {
            OnAnimationStart();

            // Trigger the callbacks.
            if (openingStartCallback != null)
                openingStartCallback();
        }

        // Loading Graphic - Opening End
        public void OnLoadingGraphicOpeningEnd()
        {
            OnAnimationEnd();

            // Trigger the callbacks.
            if (openingEndCallback != null)
                openingEndCallback();

            // If the next scene should be loaded, load it.
            if (loadNextScene)
            {
                // The next scene is set.
                if (nextScene != string.Empty)
                {
                    // Checks if the scene should be loaded asynchronously or not.
                    if (loadSceneAsync) // Async
                    {
                        asyncLoader.LoadScene(nextScene);
                    }
                    else // Sync
                    {
                        SceneManager.LoadScene(nextScene);
                    }
                }

            }
        }

        // Plays the animation for scene loading in progress.
        // This is meant to loop, so it has no opening or closing animation call.
        public void PlayLoadingGraphicProgressAnimation()
        {
            // If there is a progress animation, play it.
            if(progressAnim != string.Empty)
                animator.Play(progressAnim);
        }

        // Plays the loading graphic closing animation.

        public void PlayLoadingGraphicClosingAnimation()
        {
            // If there is a closing animation, play it.
            if (closingAnim != string.Empty)
                animator.Play(closingAnim);
        }

        // Loading Graphic - Closing Start
        public void OnLoadingGraphicClosingStart()
        {
            OnAnimationStart();

            // Trigger the callbacks.
            if (closingStartCallback != null)
                closingStartCallback();
        }

        // Loading Graphic - Closing End
        public void OnLoadingGraphicClosingEnd()
        {
            OnAnimationEnd();

            // Trigger the callbacks.
            if (closingEndCallback != null)
                closingEndCallback();
        }
    }
}