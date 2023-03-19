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

    // The audio fader
    public AudioFader audioFader;

    // The current time of the audio.
    public Text audioCurrTime;

    // Start is called before the first frame update
    void Start()
    {
        // audioFader.FadeOut();
        // audioFader.FadeIn();
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

    // Converts the audio time from the provided seconds.
    public string FormatAudioTime(float timeInSeconds)
    {
        // The current time.
        float currTime = timeInSeconds;

        // Hours.
        float hours = Mathf.Floor(currTime / 3600.0F);
        currTime -= hours * 3600.0F;

        // Minutes.
        float minutes = Mathf.Floor((currTime)/ 60.0F);
        currTime -= minutes * 60.0F;

        // Seconds.
        float seconds = Mathf.Floor(currTime);

        if (seconds < 0.0F)
            seconds = 0.0F;

        // The resulting time code.
        string result = hours.ToString("00") + ": " + minutes.ToString("00") + ": " + seconds.ToString("00");

        return result;
    }

    // Update is called once per frame
    void Update()
    {
        // parameters set.
        if(audioSource != null && slider != null)
        {
            // Audio is playing.
            if(audioSource.isPlaying)
            {
                slider.value = audioSource.time / audioSource.clip.length;
                audioCurrTime.text = FormatAudioTime(audioSource.time);
            }
        }
        else
        {
            audioCurrTime.text = "-";
        }
    }
}
