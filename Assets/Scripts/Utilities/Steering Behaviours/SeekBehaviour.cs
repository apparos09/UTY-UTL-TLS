using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Seeks a target.
    public class SeekBehaviour : SteeringBehaviour
    {
        [Header("Seek")]
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
            // The distance vector from the object's current position to the target's current position.
            Vector3 distVec = target.transform.position - transform.position;

            // Applies force.
            ApplyForce(distVec);
        }
    }
}
