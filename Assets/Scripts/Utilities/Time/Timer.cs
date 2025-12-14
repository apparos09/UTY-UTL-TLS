using UnityEngine;

namespace util
{
    // A timer.
    public class Timer : MonoBehaviour
    {
        // The time.
        public float timer = 0.0F;

        // If true, the timer is being run.
        public bool runTimer = true;

        // If 'true', fixed time is used. If false, regular time is used.
        [Tooltip("Uses fixed delta time if true, and regular delta time if false.")]
        public bool useFixedTime = false;

        // If 'true', the timer uses unscaled time. If false, it uses scaled time.
        [Tooltip("Uses unscaled time if true, and scaled (normal) time if false.")]
        public bool useUnscaledTime = true;

        // CALLBACKS
        // a callback for a timer function.
        public delegate void TimerCallback();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            // ...
        }

        // Starts the timer. This has the option to reset the timer as well.
        public virtual void StartTimer(bool resetTimer)
        {
            runTimer = true;

            // If the timer should be reset.
            if (resetTimer)
                ResetTimer();
        }

        // Stops the timer, which gives the option to reset the timer.
        public virtual void StopTimer(bool resetTimer)
        {
            runTimer = false;

            // If the timer should be reset.
            if(resetTimer)
                ResetTimer();
        }

        // Pausing
        // Returns 'true' if the timer is running.
        public virtual bool IsTimerRunning()
        {
            return runTimer;
        }

        // Checks if the timer is paused.
        public virtual bool IsPaused()
        {
            return !runTimer;
        }

        // Pauses the timer.
        public virtual void SetPaused(bool paused)
        {
            runTimer = !paused;
        }

        // Pause the timer.
        public virtual void PauseTimer()
        {
            SetPaused(true);
        }

        // Unpauses the timer.
        public virtual void UnpauseTimer()
        {
            SetPaused(false);
        }

        // Toggles the pause function.
        public virtual void TogglePaused()
        {
            SetPaused(!IsPaused());
        }

        // Resets the timer.
        public virtual void ResetTimer()
        {
            timer = 0.0F;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // If the timer should be run.
            if(runTimer)
            {
                // Checks if fixed time should be used, then checks if unscaled time should be used.
                if(useFixedTime)
                {
                    timer += useUnscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
                }
                else
                {
                    timer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                }
            }
        }
    }
}