using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Flees from a target.
    public class FleeBehaviour : SteeringBehaviour
    {
        [Header("Flee")]
        // The target object.
        public GameObject target;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Runs the flee behaviour.
        public override void RunBehaviour()
        {
            // The distance vector from the target's current position to the object's current position.
            Vector3 distVec = transform.position - target.transform.position;

            // Applies force.
            ApplyForce(distVec);
        }
    }
}