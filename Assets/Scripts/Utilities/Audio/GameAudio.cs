using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // A base for a game audio manager.
    public class GameAudio : MonoBehaviour
    {
        // The audio sources.
        // Background Music
        public AudioSource bgmSource;
        public AudioSourceLooper bgmLooper;

        // Sound Effects
        public AudioSource sfxWorldSource;
        public AudioSource sfxUISource;

        // Voice
        public AudioSource vceSource;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the looper exists.
            if (bgmLooper != null)
            {
                // If the looper's audio source has not been set.
                if (bgmLooper.audioSource == null)
                {
                    // Set the audio source.
                    bgmLooper.audioSource = bgmSource;
                }
            }
        }



        // BACKGROUND MUSIC
        // Plays the provided background music.
        // The arguments 'clipStart' and 'clipEnd' are used for the BGM looper.
        public void PlayBackgroundMusic(AudioClip bgmClip, float clipStart, float clipEnd, float delay)
        {
            if (bgmSource != null)
            {
                // If the looper has been set, change it thorugh that.
                if (bgmLooper != null)
                {
                    // Stop the audio and set the clip. This puts the audio at its start.
                    bgmLooper.StopAudio(true);
                    bgmLooper.audioSource.clip = bgmClip;

                    // Sets the start and end for the BGM.
                    bgmLooper.clipStart = clipStart;
                    bgmLooper.clipEnd = clipEnd;

                    // Play the BGM through the looper, providing the delay.
                    bgmLooper.PlayAudio(true, delay);
                }
                else // No looper, so change settings normally.
                {
                    // Stops the BGM source and sets the current clip.
                    bgmSource.Stop();
                    bgmSource.clip = bgmClip;

                    // Play the BGM with the normal settings.
                    // If a delay has been provided, play delay is used.
                    if(delay > 0)
                    {
                        bgmSource.PlayDelayed(delay);
                    }
                    else
                    {
                        bgmSource.Play();
                    }
                    
                }
            }    

        }

        // Plays background music with the provided clip start and end.
        // This has the audio play instantly.
        public void PlayBackgroundMusic(AudioClip bgmClip, float clipStart, float clipEnd)
        {
            PlayBackgroundMusic(bgmClip, clipStart, clipEnd, 0);

        }

        // Plays the background music with the provided delay.
        public void PlayBackgroundMusic(AudioClip bgmClip, float delay)
        {
            PlayBackgroundMusic(bgmClip, 0, bgmClip.length, delay);
        }

        // Plays the background music (clipStart and clipEnd are autoset to the start and end of the audio).
        // The audio also plays instantly.
        public void PlayBackgroundMusic(AudioClip bgmClip)
        {
            PlayBackgroundMusic(bgmClip, 0, bgmClip.length);
        }

        // Plays the provided background music.
        // If 'stopAudio' is 'true', then the BGM is stopped before playing the one shot.
        public void PlayBackgroundMusicOneShot(AudioClip bgmClip, bool stopCurrAudio = true)
        {
            // The BGM source is set.
            if(bgmSource != null)
            {
                // If the current audio should be stopped.
                if (stopCurrAudio)
                    bgmSource.Stop();

                // Play one shot.
                bgmSource.PlayOneShot(bgmClip);
            }
            
        }

        // Stops the provided background music.
        public void StopBackgroundMusic()
        {
            if(bgmSource != null)
                bgmSource.Stop();
        }



        // SOUND EFFECTS
        // Plays the provided sound effect.
        // If 'inWorld' is true, it's played using the world SFX source.
        // IF 'inWorld' is false, it's played using the UI SFX source.
        public void PlaySoundEffect(AudioClip sfxClip, bool inWorld)
        {
            // Gets set to 'true' when a sound effect has been successfully played.
            bool playedSfx = false;

            // Checks the audio source to use.
            if (inWorld) // World SFX
            {
                // If the source is activate and enabled, use it.
                if (sfxWorldSource.isActiveAndEnabled)
                {
                    sfxWorldSource.PlayOneShot(sfxClip);
                    playedSfx = true;
                }
                else
                {
                    playedSfx = false;
                }


            }
            else // UI SFX
            {
                // If the source is activate and enabled, use it.
                if (sfxUISource.isActiveAndEnabled)
                {
                    sfxUISource.PlayOneShot(sfxClip);
                    playedSfx = true;
                }
                else
                {
                    playedSfx = false;
                }

            }

            // If it's in the editor, throw this message.
            // Unity provides a message anyway if you attempt to play a disabled audio source...
            // So this is to hide that.
            if (Application.isEditor && !playedSfx)
            {
                Debug.LogWarning("The SFX source is disabled, so it can't be played.");
            }

        }

        // Plays the sound effect for the world.
        public void PlaySoundEffectWorld(AudioClip sfxClip)
        {
            PlaySoundEffect(sfxClip, true);
        }

        // Plays the sound effect for the UI.
        public void PlaySoundEffectUI(AudioClip sfxClip)
        {
            PlaySoundEffect(sfxClip, false);
        }

        // Stops the sound effect.
        public void StopSoundEffect(bool inWorld)
        {
            // Checks which sound effect to stop.
            if (inWorld)
                sfxWorldSource.Stop();
            else
                sfxUISource.Stop();
        }

        // Stops the sound effect for the world.
        public void StopSoundEffectWorld()
        {
            StopSoundEffect(true);
        }

        // Stops the sound effect for the UI.
        public void StopSoundEffectUI()
        {
            StopSoundEffect(false);
        }



        // VOICE
        // Plays the voice clip.
        public void PlayVoice(AudioClip vceClip)
        {
            if(vceSource != null)
                vceSource.PlayOneShot(vceClip);
        }

        // Stops the voice clip.
        public void StopVoice()
        {
            if (vceSource != null)
                vceSource.Stop();
        }
    }
}