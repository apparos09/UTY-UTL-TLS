using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Pursues the target.
    // This is similar to the 'flee behaviour', except the target's next moves are predicted and taken into account.
    public class PursueBehaviour : SteeringBehaviour
    {
        [Header("Pursue")]
        // The target object.
        public SteeringBehaviour target;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Runs the flee behaviour.
        public override void RunBehaviour()
        {
            // The distance vector from the object's current position to the target's predicted position.
            Vector3 distVec = (target.transform.position + target.rigidBody.velocity) - transform.position;

            // Applies force.
            ApplyForce(distVec);
        }
    }
}