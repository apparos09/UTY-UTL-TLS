using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // The audio for clicking a button.
    public class ButtonAudio : MonoBehaviour
    {
        // The button this script is for.
        public Button button;

        // THe audio for the user inputs.
        public AudioSource audioSource;

        // The button sound effect.
        public AudioClip audioClip;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Moved here in case the button has not been set enabled before the game was closed.

            // Button not set.
            if (button == null)
            {
                // Tries to grab the button from the parent object.
                gameObject.TryGetComponent<Button>(out button);
            }

            // Listener for the tutorial toggle.
            button.onClick.AddListener(delegate
            {
                OnClick();
            });
        }

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }


        // Called when the button is clicked.
        private void OnClick()
        {
            audioSource.PlayOneShot(audioClip);
        }

        // Script is destroyed.
        private void OnDestroy()
        {
            // Remove the listener for onClick.
            button.onClick.RemoveListener(OnClick);
        }
    }
}