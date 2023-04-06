using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Seeks a target.
    public class SeekBehaviour : SteeringBehaviour
    {
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
            // The distance vector between the target and the object's current position.
            Vector3 distVec = target.transform.position - transform.position;

            // Set forward to the normalized distance vector.
            transform.forward = distVec.normalized;

            // Calculates the force that's being applied.
            Vector3 force = transform.forward * speed;

            // Applies delta time to the object's force.
            if (applyDeltaTime)
                force *= Time.deltaTime;



            // Adds force to the rigidbody.
            rigidBody.AddForce(force, forceMode);
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}
