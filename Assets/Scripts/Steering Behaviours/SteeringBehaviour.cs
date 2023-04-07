/*
 * References:
 * 	- http://www.red3d.com/cwr/steer/
 *  - https://www.red3d.com/cwr/steer/gdc99/
 *  - https://github.com/libgdx/gdx-ai/wiki/Steering-Behaviors
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // A steering behaviour for an object.
    public abstract class SteeringBehaviour : MonoBehaviour
    {
        // public enum steer { none, seek, flee, pursue, evade, wander, arrive, avoid, pathFollow}
        
        // Determines if the behaviour should be run or not.
        public bool runBehaviour = true;

        // The object's rigidbody.
        public Rigidbody rigidBody;

        // The force mode
        public ForceMode forceMode = ForceMode.Force;

        // The movement speed.
        public float speed = 100.0F;

        // If set to 'true', deltaTime is applied to the speed of the object.
        public bool applyDeltaTime = true;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Tries to grab the rigid body.
            if (rigidBody == null)
                rigidBody = GetComponent<Rigidbody>();
        }

        // Zeroes out the velocity on the rigid body.
        public void ResetVelocity()
        {
            rigidBody.velocity = Vector3.zero;
        }

        // Applies force to the attached object. The vector provided is the distance of the force.
        protected void ApplyForce(Vector3 direction)
        {
            ApplyForce(direction, speed, forceMode, false);
        }

        // Applies force with the provided speed. Argument 'overwrite' determines if this should be set as the object's speed.
        protected void ApplyForce(Vector3 direction, float a_speed, bool overwrite)
        {
            ApplyForce(direction, a_speed, forceMode, overwrite);
        }

        // Applies force with the provided force mode.
        // Argument 'overwrite' determines if this should be set as the object's force mode.
        protected void ApplyForce(Vector3 direction, ForceMode a_forceMode, bool overwrite)
        {
            ApplyForce(direction, speed, a_forceMode, overwrite);
        }

        // Applies force to the attached object. The vector provided is the distance of the force.
        // If 'overwrite' is set to 'true', then the set speed and force mode are overwritten.
        protected void ApplyForce(Vector3 direction, float a_speed, ForceMode a_forceMode, bool overwrite)
        {
            // Set forward to the normalized distance vector.
            transform.forward = direction.normalized;

            // Calculates the force that's being applied.
            Vector3 force = transform.forward * a_speed;

            // Applies delta time to the object's force.
            if (applyDeltaTime)
                force *= Time.deltaTime;


            // Adds force to the rigidbody.
            rigidBody.AddForce(force, a_forceMode);

            // Values should be overwritten.
            if(overwrite)
            {
                speed = a_speed;
                forceMode = a_forceMode;
            }
        }

        // Runs the behaviour for the object.
        public abstract void RunBehaviour();

        // Update is called once per frame
        protected virtual void Update()
        {
            // If the behaviour should be run.
            if (runBehaviour)
                RunBehaviour();
        }
    }
}