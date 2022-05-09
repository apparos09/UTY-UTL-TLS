using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// user controls for the camera.
public class CameraController : MonoBehaviour
{
    // camera controls.
    public bool cameraLock = false; // locks the camera if 'true'

    // vectors for movement.
    public Vector3 movementSpeed = new Vector3(12.5F, 3.0F, 12.5F);

    // vector for rotation.
    public Vector3 rotationSpeed = new Vector3(50.0F, 50.0F, 50.0F);

    // reset position and orientation
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    // the position limits.
    [Header("Limits")]

    // limits for the position
    public Vector3 positionLimits = new Vector3(10.0F, 10.0F, 10.0F);

    // if 'true', the position limits are used.
    public bool usePositionLimits = false;

    // Start is called before the first frame update
    void Start()
    {
        // gets the default position and rotation
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }

    // called to toggle the camera lock on and off. 
    public void CameraLock()
    {
        cameraLock = !cameraLock;
    }

    // Update is called once per frame
    void Update()
    {
        // locks the camera so it can't move
        if (!cameraLock)
        {
            // Movement of the Camera
            // forward movement and backward movement
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, movementSpeed.z * Time.deltaTime));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -movementSpeed.z * Time.deltaTime));
            }

            // leftward and rightward movement
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-movementSpeed.x * Time.deltaTime, 0, 0));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(movementSpeed.x * Time.deltaTime, 0, 0));
            }

            // upward movement and downward movement
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(new Vector3(0, movementSpeed.y * Time.deltaTime, 0));
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(new Vector3(0, -movementSpeed.y * Time.deltaTime, 0));
            }


            // Rotation of the Camera
            // x-axis rotation
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Rotate(Vector3.right, -rotationSpeed.x * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Rotate(Vector3.right, +rotationSpeed.x * Time.deltaTime);
            }

            // y-axis rotation
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(Vector3.up, -rotationSpeed.y * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(Vector3.up, +rotationSpeed.y * Time.deltaTime);
            }

            // z-axis rotation
            if (Input.GetKey(KeyCode.PageUp))
            {
                transform.Rotate(Vector3.forward, -rotationSpeed.z * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.PageDown))
            {
                transform.Rotate(Vector3.forward, +rotationSpeed.z * Time.deltaTime);
            }
        }

        // resets the camera's position to what it was when the program first ran.
        if(Input.GetKey(KeyCode.T))
        {
            transform.position = defaultPosition;
        }

        // resets the camera's orientation to what it was when the program first ran.
        if(Input.GetKey(KeyCode.R))
        {
            transform.rotation = defaultRotation;
        }

        // clamps the position.
        if(usePositionLimits)
        {
            // makes sure the position limits are aboslute values.
            positionLimits = new Vector3(
                Mathf.Abs(positionLimits.x),
                Mathf.Abs(positionLimits.y),
                Mathf.Abs(positionLimits.z)
                );

            // final position.
            Vector3 finalPos = new Vector3(
                Mathf.Clamp(transform.position.x, -positionLimits.x, +positionLimits.x),
                Mathf.Clamp(transform.position.y, -positionLimits.y, +positionLimits.y),
                Mathf.Clamp(transform.position.z, -positionLimits.z, +positionLimits.z)
                );

            // set to the final position.
            transform.position = finalPos;
        }

    }
}
