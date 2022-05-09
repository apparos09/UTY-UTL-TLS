using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// an audio scene manager.
public class AudioSceneManager : MonoBehaviour
{
    // the audio source
    public AudioSource audioSource;

    // the audio slider.
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // plays the audio
    public void PlayAudio()
    {
        audioSource.Stop();
        audioSource.time = 0.0F;
        audioSource.Play();
    }

    // pauses the audio
    public void PauseAudio()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
        else if (!audioSource.isPlaying)
            audioSource.UnPause();
    }

    // stops the audio
    public void StopAudio()
    {
        audioSource.Stop();
        audioSource.time = 0.0F;
    }

    // called when the slider is manually changed.
    public void OnSliderChange()
    {
        // audio source not set.
        if (audioSource == null || slider == null)
            return;

        // t-value.
        float t = Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value);

        // sets the time based on the slider.
        audioSource.time = audioSource.clip.length * t;
    }

    // Update is called once per frame
    void Update()
    {
        // parameters set.
        if(audioSource != null && slider != null)
        {
            // audio is playing.
            if(audioSource.isPlaying)
            {
                slider.value = audioSource.time / audioSource.clip.length;
            }
        }
    }
}
