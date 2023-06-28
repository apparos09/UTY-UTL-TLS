using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Evades the target.
    // This is similar to the 'flee behaviour', except the target's next moves are predicted and taken into account.
    public class EvadeBehaviour : SteeringBehaviour
    {
        [Header("Evade")]
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
            // The distance vector from the target's predicted position to the object's current position.
            Vector3 distVec = transform.position - (target.transform.position + target.rigidBody.velocity);

            // Applies force.
            ApplyForce(distVec);
        }
    }
}