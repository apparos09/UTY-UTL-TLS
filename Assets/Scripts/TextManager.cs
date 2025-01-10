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
        // Runs the rotation tests.
        // RunRotationTests();

        // Runs the time tests.
        // RunTimeTests();

        // Runs the singleton tests.
        // RunSingletonTests();

        // Runs the string equation tests.
        RunStringEquationTests();

    }

    // Runs the rotation tests.
    public void RunRotationTests()
    {
        // ROTATION TEST
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
    }

    // Runs time tests.
    public void RunTimeTests()
    {
        // TIME FORMATTING TEST
        // Time Test
        float timeSeconds = 7268.0F; // 2 Hours, 1 Minute, 8 Seconds
        Debug.Log("Time (Seconds): " + timeSeconds + " | Time (HH:MM:SS:MS): " + StringFormatter.FormatTime(timeSeconds));

        timeSeconds = 300.0F; // 5 Minutes
        Debug.Log("Time (Seconds): " + timeSeconds + " | Time (MM:SS:MS): " + StringFormatter.FormatTime(timeSeconds, false));

        timeSeconds = 362.525F; // 6 Minutes, 2 Seconds, 525 Milliseconds
        Debug.Log("Time (Seconds): " + timeSeconds + " | Time (MM:SS:MS): " + StringFormatter.FormatTime(timeSeconds, false, true));

        timeSeconds = 200.0F; // 3 Minutes, 20 Seconds
        Debug.Log("Time (Seconds): " + timeSeconds + " | Time (MM:SS): " + StringFormatter.FormatTime(timeSeconds, false, true, false));

        timeSeconds = 6272.2F; // 1 Hours, 44 Minutes, 32 Seconds, 2 Milliseconds
        Debug.Log("Time (Seconds): " + timeSeconds + " | Time (HH:MM:SS:MS): " + StringFormatter.FormatTime(timeSeconds));
    }

    // Runs singleton tests.
    public void RunSingletonTests()
    {
        // Singleton Test
        // Before creation.
        bool singletonIst = false;
        singletonIst = Singleton.Instantiated;
        Debug.Log("Singleton Instantiated (Not Created): " + singletonIst.ToString());

        // Create instance and check again.
        Singleton singleton = Singleton.Instance;
        singletonIst = Singleton.Instantiated;
        Debug.Log("Singleton Instantiated (Created): " + singletonIst.ToString());

        DestroyImmediate(singleton.gameObject); // Makes sure it's destroyed right away so that the check can work.
        singletonIst = Singleton.Instantiated;
        Debug.Log("Singleton Instantiated (Destroyed): " + singletonIst.ToString());
    }

    // Runs the string equation tests.
    public void RunStringEquationTests()
    {
        // Equation array.
        string[] eqArr = new string[]
        {
            "4+3", // = 7
            "5 - 6", // = 1
            "2^4", // = 16
            "-3 + 12", // = 9
            "+4 - 6", // = -2
            "12 / 4", // = 3
            "3*3", // = 9
            "3.24", // = 3.24
            "5.", // = 5.0
            "+-/*", // Invalid
            "8+-5", // 3 (Invalid, see note 1)
            "8 / 0", // Invalid
            "3 - 2 ^ 3 * 8 / 2 + 7", // = -22
            "4 * (2 + 5)", // = 28
            "4 ) 5 + (3", // Invalid
            "4 + ((10)) - 1", // = 13
            "2 * (3 + (10 - 1))", // = 24
            "(3)(2)(10)", // = 60
            "(9 + 6) * 2", // = 30 
            "8^(1+1)", // = 64
            "*8+4/7", // Invalid
            "5+(-(10))", // = -5 (Invalid, see note 2)
            "10/(5-2)"  // = 3.3333~
        };

        // NOTE 1: the program can't handle having + and - next to each other, even if they logically can be in some cases.
        // It isn't necessary to fix for now, but it should be fixed at some point.
        // TODO: fix the +- error.

        // NOTE 2: the program marks (+) and (-) outside of brackets with no other numbers as invalid.
        // e.g., "5+(-(10))" should evaluate as 5+(-10) or simply 5-10. However, the program can't handle such cases.
        // As such, these cases cause an error.
        // At this time, it is not necessary to fix this error, but it SHOULD be fixed at some point.
        
        
        // TODO: fix the brackets and plus/minus symbol errors.


        // Goes through the array, calculating every result.
        foreach(string eq in eqArr)
        {
            // Calculates the math string.
            string result = CustomMath.CalculateMathString(eq);

            // If the result is empty, show it as an error.
            if (result == string.Empty)
                result = "Error";

            // Print the result.
            Debug.Log(eq + " = " + result);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
