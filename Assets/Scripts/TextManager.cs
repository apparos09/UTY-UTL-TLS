using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

// Manages the sample scene.
public class TextManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Rotation Test
        Vector3 v = new Vector3(1, 0, 0);
        float theta = 45.0F;

        Debug.Log("Rotating " + v.ToString() + " by " + theta.ToString() + " Degrees (2D) | " +
            CustomMath.Rotate(new Vector2(v.x, v.y), theta, true).ToString());

        Debug.Log("Rotating " + v.ToString() + " by " + theta.ToString() + " Degrees (X-3D) | " +
            CustomMath.RotateX(v, theta, true).ToString());

        Debug.Log("Rotating " + v.ToString() + " by " + theta.ToString() + " Degrees (Y-3D) | " +
            CustomMath.RotateY(v, theta, true).ToString());

        Debug.Log("Rotating " + v.ToString() + " by " + theta.ToString() + " Degrees (Z-3D) | " +
            CustomMath.RotateZ(v, theta, true).ToString());

        // Time Test
        float timeSeconds = 7268.0F;
        Debug.Log("Time (Seconds): " + timeSeconds + " | Time (HH:MM:SS): " + StringFormatter.FormatTime(timeSeconds));

        timeSeconds = 300.0F; // 5 Minutes
        Debug.Log("Time (Seconds): " + timeSeconds + " | Time (HH:MM:SS): " + StringFormatter.FormatTime(timeSeconds, false));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
