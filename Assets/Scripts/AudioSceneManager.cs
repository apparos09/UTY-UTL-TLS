using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using util;

// an audio scene manager.
public class AudioSceneManager : MonoBehaviour
{
    // the audio source
    public AudioSource audioSource;

    // the audio source control (main)
    public AudioSourceControl audioControlMain;

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


        // Tests
        // // RESOURCE NAME TEST 1
        // // The following member calls and functions can be used to print the file name of the audio clip.
        // // The file extension (e.g., ogg) is not included.
        // Debug.Log(audioSource.resource); // fileName (UnityEngine.AudioClip)
        // Debug.Log(audioSource.resource.name); // fileName
        // Debug.Log(audioSource.resource.ToString()); // fileName (UnityEngine.AudioClip)

        // // RESOURCE NAME TEST 2
        // if (audioControlMain != null)
        // {
        //     if(audioControlMain.isActiveAndEnabled)
        //     {
        //         Debug.Log(audioControlMain.GetAudioSourceResourceName());
        //     }
        // }


        // AUDIO CLIP NAME TEST 1
        // Debug.Log(audioSource.clip); // fileName (UnityEngine.AudioClip)
        // Debug.Log(audioSource.clip.name); // fileName
        // Debug.Log(audioSource.clip.ToString()); // fileName (UnityEngine.AudioClip)

        // // // AUDIO CLIP NAME TEST 2
        // if (audioControlMain != null)
        // {
        //     if(audioControlMain.isActiveAndEnabled)
        //     {
        //         Debug.Log(audioControlMain.GetAudioSourceClipName());
        // 
        //     }
        // }


        // // FILE AND FILE PATH TEST
        // string testFilePath = "World/Myths/Element";
        // string testFile = "water.txt";
        // string testFileAndPath;
        // 
        // // Test 1 - all forward slashes result
        // testFilePath = "World/Myths/Element";
        // testFile = "water.txt";
        // testFileAndPath = FileReader.CombineFilePathAndFile(testFilePath, testFile);
        // Debug.Log(testFileAndPath);
        // 
        // // Test 2 - all backward slashes result
        // testFilePath = "World\\Myths\\Element";
        // testFile = "fire.txt";
        // testFileAndPath = FileReader.CombineFilePathAndFile(testFilePath, testFile);
        // Debug.Log(testFileAndPath);
        // 
        // // Test 3 - forward slashes and one backward slash added
        // testFilePath = "World/Myths/Element";
        // testFile = "earth.txt";
        // testFileAndPath = FileReader.CombineFilePathAndFile(testFilePath, testFile, false);
        // Debug.Log(testFileAndPath);
        // 
        // // Test 4 - mix of forward slashes and backward slashes
        // testFilePath = "World/Myths\\Element";
        // testFile = "air.txt";
        // testFileAndPath = FileReader.CombineFilePathAndFile(testFilePath, testFile, false);
        // Debug.Log(testFileAndPath);
        // 
        // // Test 5 = mix of forward and backward slashes to all forward slashes
        // testFilePath = "World/Myths\\Element";
        // testFile = "electric.txt";
        // testFileAndPath = FileReader.CombineFilePathAndFile(testFilePath, testFile, true);
        // Debug.Log(testFileAndPath);
        // 
        // // Test 5 = mix of forward and backward slashes to all back slashes
        // testFilePath = "World\\Myths/Element";
        // testFile = "energy.txt";
        // testFileAndPath = FileReader.CombineFilePathAndFile(testFilePath, testFile, true);
        // Debug.Log(testFileAndPath);
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
        if (clipStartText.text != looper.clipStart.ToString())
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
