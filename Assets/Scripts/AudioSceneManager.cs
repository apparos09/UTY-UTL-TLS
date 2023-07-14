using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

// an audio scene manager.
public class AudioSceneManager : MonoBehaviour
{
    // the audio source
    public AudioSource audioSource;

    // the audio slider.
    public Slider slider;

    // The current time of the audio.
    public Text audioCurrTime;

    // The audio current time in seconds.
    public Text audioCurrTimeSeconds;

    // THe audio length.
    public Text audioLength;

    // The clip start time text.
    public Text clipStartText;

    // The clip end time text.
    public Text clipEndText;

    [Header("Audio Functions")]

    // The audio fader
    public AudioFader audioFader;

    // The audio looper.
    public AudioSourceLooper looper;

    // Start is called before the first frame update
    void Start()
    {
        // audioFader.FadeOut();
        // audioFader.FadeIn();

        // Display
        audioLength.text = audioSource.clip.length.ToString();

        clipStartText.text = looper.clipStart.ToString();
        clipEndText.text = looper.clipEnd.ToString();
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
        // Updates the start clip text.
        if(clipStartText.text != looper.clipStart.ToString())
            clipStartText.text = looper.clipStart.ToString();
        
        // Updates the end clip text.
        if(clipEndText.text != looper.clipEnd.ToString())
            clipEndText.text = looper.clipEnd.ToString();

        // parameters set.
        if (audioSource != null && slider != null)
        {
            // Audio is playing.
            if(audioSource.isPlaying)
            {
                slider.value = audioSource.time / audioSource.clip.length;
                audioCurrTime.text = FormatAudioTime(audioSource.time);
                audioCurrTimeSeconds.text = audioSource.time.ToString();
            }
        }
        else
        {
            audioCurrTime.text = "-";
        }
    }
}
