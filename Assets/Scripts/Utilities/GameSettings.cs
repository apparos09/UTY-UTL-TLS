using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // The game settings.
    public class GameSettings : MonoBehaviour
    {
        // The game settings instance.
        private static GameSettings instance;

        // Gets set to 'true' when the singleton is instanced.
        private bool initialized = false;

        // If 'true', tutorial elements are used.
        public bool useTutorial = true;

        // If 'true', TextToSpeech is used with text boxes.
        public bool UseTextToSpeech = false;

        // Constructor
        private GameSettings()
        {
            // ...

        }

        // Awake is called when the script is being loaded
        void Awake()
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
            }

            // Run code for initialization.
            if (!initialized)
            {
                initialized = true;
            }
        }

        // Start is called just before any of the Update methods is called the first time
        private void Start()
        {
            // Don't destroy this object on load.
            DontDestroyOnLoad(this);
        }

        // Gets the instance.
        public static GameSettings Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<GameSettings>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Game Settings (singleton)");
                        instance = go.AddComponent<GameSettings>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public bool Initialized
        {
            get
            {
                return initialized;
            }
        }

        // Quits the application.
        public static void QuitApplication()
        {
            Application.Quit();
        }
    }
}
