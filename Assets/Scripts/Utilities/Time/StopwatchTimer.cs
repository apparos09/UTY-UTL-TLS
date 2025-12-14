using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // A stopwatch timer. Along with regular timer functions, the user can add laps.
    public class StopwatchTimer : Timer
    {
        [Header("Stopwatch")]

        // The current lap time.
        public float currentLapTimer = 0.0F;

        [Tooltip("The laps. When a lap is ended, it's added to this list.")]
        // The lap times.
        public List<float> lapTimes = new List<float>();

        // Resets the timer, the current lap timer, and removes all laps.
        public override void ResetTimer()
        {
            timer = 0.0F;
            currentLapTimer = 0.0F;
            lapTimes.Clear();
        }

        // Generates a lap, saves it to the list, and starts a new lap.
        public void Lap()
        {
            // Saves the current lap time in the list and resets the current time.
            lapTimes.Add(currentLapTimer);
            currentLapTimer = 0.0F;
        }

        // Returns the number of laps.
        // If the current lap is included, the count is added to by 1.
        public int GetLapCount(bool includeCurrentLap)
        {
            return (includeCurrentLap) ? lapTimes.Count + 1 : lapTimes.Count;
        }

        // Gets the current lap number. A lap's index value (if applicable) is 1 less than its number.
        // E.g., the lap at index 0 in the lapTimes list is lap 1.
        public int GetCurrentLapNumber()
        {
            // Does Count + 1 to get the current lap's number.
            return lapTimes.Count + 1;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the lap timer should be run.
            if (runTimer)
            {
                // Checks if fixed time should be used, then checks if unscaled time should be used.
                if (useFixedTime)
                {
                    currentLapTimer += useUnscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
                }
                else
                {
                    currentLapTimer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                }
            }
        }
    }
}