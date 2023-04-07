using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Has the object arrive at a target.
    public class ArriveBehaviour : SteeringBehaviour
    {
        [Header("Arrive")]
        // The target object.
        public GameObject target;

        // The distance from the target where the object starts to slow down.
        // The object increases and decrease velocity at the same rate... 
        // So if the object should slow down more, increase the slow distance.
        public float slowDistance = 10.0F;

        // The threshold that must be stopped below for the object's velocity to cease.
        public float velocityStop = 0.001F;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Runs the flee behaviour (may need to be improved).
        public override void RunBehaviour()
        {
            // If the positions are equal, do nothing.
            // This is to prevent a look vector of 0 from being set.
            if (transform.position == target.transform.position)
            {
                rigidBody.velocity = Vector3.zero;
                return;
            }

            // The distance vector from the object's current position to the target's current position.
            Vector3 distVec = target.transform.position - transform.position;

            // Applies force for going towards the target.
            ApplyForce(distVec);

            // If within the slow down distance.
            if(distVec.magnitude <= Mathf.Abs(slowDistance))
            {
                // If below the stop distance, stop the rigid body entirely.
                if(distVec.magnitude <= Mathf.Abs(velocityStop))
                {
                    rigidBody.velocity = Vector3.zero;
                    transform.position = target.transform.position;
                }
                else
                {
                    // Default.
                    // ApplyForce(-distVec);


                    // Didn't work.

                    // Ver. 1
                    // // Checks how many steps are remaining to reach the target.
                    // int steps;
                    // 
                    // // If the object is already moving, set the slowdown speed relative to the velocity.
                    // if (rigidBody.velocity != Vector3.zero)
                    //     steps = Mathf.CeilToInt(distVec.magnitude / rigidBody.velocity.magnitude);
                    // else // Just use the distance vector's magnitude.
                    //     steps = Mathf.CeilToInt(distVec.magnitude);
                    // 
                    // // Calculates the slowdown speed for the object.
                    // float slowSpeed = (steps != 0) ? speed + rigidBody.velocity.magnitude / steps: speed;
                    // 
                    // // Apply force in the opposite direction to slow the entity down.
                    // ApplyForce(-distVec, slowSpeed, false);
                    // 
                    // // Zeroes out the velocity if within range.
                    // if (rigidBody.velocity.magnitude <= Mathf.Abs(velocityStop))
                    //     ResetVelocity();

                    // Ver. 2
                    // Gets the distance from the target to the object.
                    Vector3 fromTargetDist = transform.position - target.transform.position;

                    // The portion the remaining distance is of the slowdown distance/radius.
                    float distPortion = fromTargetDist.magnitude / slowDistance;

                    // Sets the velocity to be the remaining travel portion.
                    Vector3 newVel = rigidBody.velocity.normalized * (distPortion * slowDistance);

                    // If the new velocity's magnitude is greater than that of the old velocity...
                    // Simply apply a counter force. If it's less than the current velocity, overwrite it entirely.
                    if (newVel.magnitude > rigidBody.velocity.magnitude)
                        ApplyForce(fromTargetDist, newVel.magnitude, false);
                    else
                        rigidBody.velocity = newVel;
                }
            }
        }
    }
}
