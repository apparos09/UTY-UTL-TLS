using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

// The interpolation scene manager.
public class InterpolationSceneManager : MonoBehaviour
{
    // The marker object that travels from point to point.
    public GameObject marker;

    // The interpolation method being used.
    public Interpolation.interType interpolation;

    // The list of game objects to interpolate between.
    public List<GameObject> points = new List<GameObject>();

    // The starting point index of the current interpolation operation.
    public int startPointIndex = 0;

    // The t-value.
    public float t = 0.0F;

    // The travel speed of the marker.
    public float speed = 1.0F;

    // Set to 'true' if the interpolation should be paused.
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!paused)
        {
            // There must be at least 2 points for interpolation to work.
            if(points.Count >= 2)
            {
                // Increase t-value, and clamp it.
                t += Time.deltaTime * speed;
                t = Mathf.Clamp01(t);


                // The four points.
                GameObject p0, p1, p2, p3;

                // The four indexes.
                int p0Index, p1Index, p2Index, p3Index;

                // MAIN POINTS
                // P1 - start point.
                p1Index = startPointIndex;
                p1 = points[p1Index];

                // P2 - end point.
                p2Index = p1Index + 1 < points.Count ? p1Index + 1 : 0;
                p2 = points[p2Index];

                // TANGENTS/EXTRAS
                // P0 - point before the start point.
                p0Index = p1Index - 1 >= 0 ? p1Index - 1 : points.Count - 1;
                p0 = points[p0Index];

                // P3 - point after the end point.
                p3Index = p2Index + 1 < points.Count ? p2Index + 1 : 0;
                p3 = points[p3Index];

                // Gets the new position.
                Vector3 newPos = Interpolation.InterpolateByType(
                    interpolation,
                    p0.transform.position,
                    p1.transform.position,
                    p2.transform.position,
                    p3.transform.position,
                    t);

                // Set the new position.
                marker.transform.position = newPos;

                // Move onto the next point.
                if(t >= 1.0F)
                {
                    // Reset t-value.
                    t = 0.0F;

                    // Increase index.
                    startPointIndex++;

                    // Index needs to be reset to 0.
                    if (startPointIndex >= points.Count)
                        startPointIndex = 0;
                }
            }
        }
        
    }
}
