using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Wanders the area.
    // TODO: wander is really buggy for some reason. Maybe have it seek a specific target instead?
    public class WanderBehaviour : SteeringBehaviour
    {
        [Header("Wander")]
        // The time the object waits  (in seconds) to change its direction.
        public float adjustTime = 50.0F;

        // The timer for adjusting the forward direction of the object.
        private float adjustTimer = 0.0F;

        // The rotation axis for wandering.
        public Vector3 rotAxis = Vector3.up;

        // The maximum rotation adjustment for the object for one cycle (in degrees).
        public float adjustRotLimit = 30.0F;

        // The adjustment range of the rotation.
        // If set to 1 or 0, the rotation adjustment is set to max.
        // If set beyond 1, then 
        public int adjustRange = 10;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Applies a random rotation factor to start off.
            transform.Rotate(rotAxis, Random.Range(0, 360));
        }

        // OnCollisionEnter is called when this collider/rigidbody has begun touching another collider/rigidbody.
        private void OnCollisionEnter(Collision collision)
        {
            // Creates a ray.
            Ray ray = new Ray(transform.position, transform.forward);

            // The raycast distance (TODO: see if this is causing issues).
            float rayDist = transform.localScale.magnitude * speed;

            // Casts a ray foward. Its length is set to the object's velocity.
            // rayDist = (rigidBody.velocity.magnitude == 0) ? 1 : rigidBody.velocity.magnitude;

            // If the ray distance is less than 1, set it to 1.
            if (rayDist < 1)
                rayDist = 1;

            // Gets the raycast result.
            bool raycastResult = Physics.Raycast(ray, rayDist);

            // If the way forward is blocked.
            if(raycastResult)
            {
                // Reverse the movement direction.
                // ApplyForce(-transform.forward);

                // Reverse the direction.
                transform.Rotate(rotAxis, 90.0F); // 180

                // Debug.Log("Rot Change Strong");
            }

            // TODO: optimize
        }

        // Adjust the movement direction.
        private void AdjustDirection()
        {
            // The rotation angle.
            float angle = 0.0F;

            // The random max.
            int randMax = adjustRange;

            // Sets the rotation of the object.
            if (randMax > 1)
                angle = adjustRotLimit / Random.Range(1, randMax + 1);
            else
                angle = adjustRotLimit;

            // Rotate by the set angle.
            transform.Rotate(rotAxis, angle);
        }

        // Runs the flee behaviour.
        public override void RunBehaviour()
        {
            // Go in the current direction.
            Vector3 distVec = transform.forward;

            // Projects a target in front of the entity.
            distVec.Scale(transform.localScale);
            distVec *= 10.0F;

            // If the x-value should remain the same.
            if (rotAxis.x == 1.0F)
                distVec.x = transform.position.x;

            // If the y-value should remain the same.
            if (rotAxis.y == 1.0F)
                distVec.y = transform.position.y;

            // If the z-value should remain the same.
            if (rotAxis.z == 1.0F)
                distVec.z = transform.position.z;

            // If the distance vector is now equal to the object's position, set it to the object's forward vector.
            if (distVec == transform.position)
                distVec = transform.forward;

            // Applies force.
            ApplyForce(distVec);
        }

        // Update is called once per frame
        protected override void Update()
        {
            // If the behaviour is running.
            if(runBehaviour)
            {
                // Reduce the timer.
                adjustTimer -= Time.deltaTime;

                // If the timer is less than or equal to 0.
                if(adjustTimer <= 0)
                {
                    // Adjust the direction.
                    AdjustDirection();

                    // Reset the timer.
                    adjustTimer = adjustTime;
                }
            }

            // Runs the behaviour.
            base.Update();
        }
    }
}
