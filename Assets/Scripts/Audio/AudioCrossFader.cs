using UnityEditor.Animations;

namespace util
{
    // Loops a piece of audio using a cross-fade.
    public class AudioCrossFader : AudioSegmentLooper
    {
        // The main audio fade.
        public AudioFader mainFade;

        // The transition fade.
        public AudioFader transitionFade;

        // The fade length. This overrides the fade lengths of the set fades.
        public float fadeDuration = 5.0F;

        // Start is called before the first frame update
        protected void Start()
        {
            // The transition shouldn't loop.
            transitionFade.audioSource.loop = false;

            // Stop the audio when the transition fade ends.
            transitionFade.stopOnFadeOut = true;
        }

        // Called to loop the clip back to its start.
        protected override void OnLoopClip()
        {
            // Set the fade durations.
            mainFade.fadeDuration = fadeDuration;
            transitionFade.fadeDuration = fadeDuration;

            // Set the trasition fade to the current audio time.
            transitionFade.audioSource.time = audioSource.time; // Doesn't use clip end.

            // Set the main audio source to the clip start.
            // These two should be the same audio source.

            // Checks if the loop should be relative to where the audio currently is...
            // Versus what clipStart and clipEnd are set to.
            if (loopRelative)
            {
                // Calculates how much clipStart should be offset by.
                float offsetStart = audioSource.time - clipEnd;

                // Set current clip start as clipStart adjusted by the offset amount.
                float currClipStart = clipStart + offsetStart;


                // If the current clip start is negative (i.e., it's before the start of the audio itself)...
                // Then use normal clipStart.
                if (currClipStart >= 0)
                {
                    audioSource.time = currClipStart;
                    mainFade.audioSource.time = currClipStart;
                }
                else
                {
                    audioSource.time = clipStart;
                    mainFade.audioSource.time = clipStart;
                }
            }
            else
            {
                audioSource.time = clipStart;
                mainFade.audioSource.time = clipStart;
            }

            // Play the audio, fading in the main fade, and fading out for the transition.
            mainFade.FadeIn();
            transitionFade.FadeOut();

            // If the main fade or main audio source should not loop, pause all of the audio sources.
            if(!audioSource.loop || !mainFade.audioSource.loop)
            {
                audioSource.Pause();
                mainFade.audioSource.Pause();
                transitionFade.audioSource.Pause();
            }
        }
    }
}