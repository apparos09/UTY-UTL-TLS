namespace util
{
    // Loops a piece of audio using a cross-fade.
    public class AudioSegmentCrossFader : AudioSegmentLooper
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
            audioSource.time = clipStart;
            mainFade.audioSource.time = clipStart;

            // Play the audio, fading in the main fade, and fading out for the transition.
            mainFade.FadeIn();
            transitionFade.FadeOut();
        }
    }
}