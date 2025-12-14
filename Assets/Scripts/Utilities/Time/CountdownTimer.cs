using UnityEngine;

namespace util
{
    // A countdown timer.
    public class CountdownTimer : Timer
    {
        [Header("Countdown")]

        // The amount of time that the countdown runs for.
        public float timerLength = 1.0F;

        // Callback for the count down timer finsihing.
        private TimerCallback endCallback;

        // Resets the timer.
        public override void ResetTimer()
        {
            timer = timerLength;
        }

        // Adds a callback for the countdown ending.
        public void OnCountdownEndAddCallback(TimerCallback callback)
        {
            endCallback += callback;
        }

        // Removes a callback for the countdown ending.
        public void OnCountdownEndRemoveCallback(TimerCallback callback)
        {
            endCallback -= callback;
        }

        // Called when the countdown is finished.
        public void OnCountdownEnd()
        {
            // Checks if there are functions to call, and calls them if there is.
            if (endCallback != null)
                endCallback();
        }

        // Update is called once per frame
        protected override void Update()
        {
            // If the timer should be run.
            if (runTimer)
            {
                // Checks if there's time left.
                if(timer > 0.0F)
                {
                    // Checks if fixed time should be used, then checks if unscaled time should be used.
                    if (useFixedTime)
                    {
                        timer -= useUnscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
                    }
                    else
                    {
                        timer -= useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                    }

                    // Clamps the timer at 0 and calls the countdown end function.
                    if(timer <= 0.0F)
                    {
                        timer = 0.0F;
                        OnCountdownEnd();
                    }
                }
            }
        }
    }
}