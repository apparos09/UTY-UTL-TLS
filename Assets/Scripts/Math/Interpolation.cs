using System.Collections.Generic;
using UnityEngine;

// TODO: implement catmull-rom and bezier versions with speed control.

namespace util
{
    // Source: https://github.com/eppz/Unity.Library.eppz_easing
    // This source was provided in the Computer Animation course.
    public class Interpolation : MonoBehaviour
    {
        // interpolation types for switching modes.
        public enum interType
        {
            lerp,
            bezier,
            catmullRom,
            easeIn1,
            easeIn2,
            easeIn3,
            easeOut1,
            easeOut2,
            easeOut3,
            easeInOut1,
            easeInOut2,
            easeInOut3,
            easeInCircular,
            easeOutCircular,
            easeInOutCircular,
            easeInBounce1,
            easeInBounce2,
            easeInBounce3,
            easeOutBounce1,
            easeOutBounce2,
            easeOutBounce3,
            easeInOutBounce1,
            easeInOutBounce2,
            easeInOutBounce3
        }



        // Calculates the lerp path length. The index acts as the end point (e.g., length[1] is the distance from p0 to p1). 
        // If 'loop' is true, then the path length includes end point to start point.
        // Sum them all up to get the total path length.
        public static List<float> CalculateLerpPointDistances(List<Vector3> points, bool loop)
        {
            // The point lengths.
            List<float> pointLengths = new List<float>();

            // Point length.
            pointLengths.Add(0.0F);

            // Goes through the points.
            for (int i = 1; i < points.Count; i++)
            {
                // Calculate the distance, and add it to the point lengths list.
                float dist = Vector3.Distance(points[i - 1], points[i]);
                pointLengths.Add(dist);
            }


            // If the path should loop back to the start.
            if (loop)
            {
                // Calculates and adds the distance loop back to the start.
                float dist = Vector3.Distance(points[points.Count - 1], points[0]);
                pointLengths.Add(dist);
            }

            // Return the point length list.
            return pointLengths;
        }


        // Calculates the spline point samples.
        public static List<Vector3> CalculateSplineSamples(bool isBezier, List<Vector3> points, bool loop, int sampleCount = 11)
        {
            // The list of spline distances.
            List<Vector3> splineSamples = new List<Vector3>();

            // The number of points to go through.
            int pointCount = (loop) ? points.Count : points.Count - 1;

            // Goes through each point.
            for (int p = 0; p < pointCount; p++)
            {
                // The points and the indexes.
                Vector3 p0, p1, p2, p3;
                int startIndex, endIndex;

                startIndex = p;
                endIndex = (p + 1 >= points.Count) ? 0 : p + 1;

                p0 = (startIndex - 1 < 0) ? points[points.Count - 1] : points[startIndex - 1];
                p1 = points[startIndex];
                p2 = points[endIndex];
                p3 = (endIndex + 1 >= points.Count) ? points[0] : points[endIndex + 1];


                // Will save the sample points, and the times needed.
                float[] sampleTimes = new float[sampleCount];
                Vector3[] curveSamples = new Vector3[sampleCount];

                // The sample increment.
                float sampleInc = 1.0F / (sampleCount - 1);

                // Calculates the samples points.
                for (int i = 0; i < curveSamples.Length; i++)
                {
                    // Calculate the sample time.
                    sampleTimes[i] = sampleInc * i;

                    // Calculate the point.
                    if (isBezier) // Bezier
                        curveSamples[i] = Bezier(p0, p1, p2, p3, sampleTimes[i]);
                    else // Catmull-Rom
                        curveSamples[i] = CatmullRom(p0, p1, p2, p3, sampleTimes[i]);

                    // Add to the spline sample list.
                    splineSamples.Add(curveSamples[i]);
                }
            }

            // Returns the spline distances.
            return splineSamples;
        }

        // Calculate the bezier point distances.
        public static List<Vector3> CalculateBezierSamples(List<Vector3> points, bool loop, int sampleCount = 11)
        {
            // The spline samples.
            List<Vector3> splineSamples = CalculateSplineSamples(true, points, loop, sampleCount);

            // Return the distances.
            return splineSamples;
        }

        // Calculate the catmull rom distances.
        public static List<Vector3> CalculateCatmullRomSamples(List<Vector3> points, bool loop, int sampleCount = 11)
        {
            // The spline samples.
            List<Vector3> splineSamples = CalculateSplineSamples(false, points, loop, sampleCount);

            // Return the distances.
            return splineSamples;
        }


        // Calculates distances on the curve.
        // if 'isCatmull' is true, it's a catmull-rom curve. If isCatmull is false, it's a bezier curve.
        public static List<float> CalculateSplinePointDistances(bool isBezier, List<Vector3> points, bool loop, int sampleCount = 11)
        {
            // The list of spline distances.
            List<float> splineDists = new List<float>();

            // Add 0 distance for start.
            splineDists.Add(0.0F);

            // The number of points to go through.
            int pointCount = (loop) ? points.Count : points.Count - 1;


            // Goes through each point.
            for (int p = 0; p < pointCount; p++)
            {
                // The points and the indexes.
                Vector3 p0, p1, p2, p3;
                int startIndex, endIndex;

                startIndex = p;
                endIndex = (p + 1 >= points.Count) ? 0 : p + 1;

                p0 = (startIndex - 1 < 0) ? points[points.Count - 1] : points[startIndex - 1];
                p1 = points[startIndex];
                p2 = points[endIndex];
                p3 = (endIndex + 1 >= points.Count) ? points[0] : points[endIndex + 1];


                // TODO: maybe re-use 'CalculateSplineSamples' function.
                // STEP 1 - Calculate sample times and sample points.
                float[] sampleTimes = new float[sampleCount];
                Vector3[] samplePoints = new Vector3[sampleCount];

                // The sample increment.
                float sampleInc = 1.0F / (sampleCount - 1);

                // Calculates the samples points.
                for (int i = 0; i < samplePoints.Length; i++)
                {
                    // Calculate the sample time.
                    sampleTimes[i] = sampleInc * i;

                    // Calculate the point.
                    if (isBezier) // Bezier
                        samplePoints[i] = Bezier(p0, p1, p2, p3, sampleTimes[i]);
                    else // Catmull-Rom
                        samplePoints[i] = CatmullRom(p0, p1, p2, p3, sampleTimes[i]);
                }

                // Step 2 - Calculate pairwise distances.
                float[] sampleDists = new float[sampleCount];

                // First distance is 0.
                sampleDists[0] = 0.0F;

                // Calculates the sample distances.
                for (int i = 1; i < sampleDists.Length; i++)
                {
                    // Calculate the distance.
                    float dist = Vector3.Distance(samplePoints[i - 1], samplePoints[i]);

                    // Save the distances.
                    sampleDists[i] = dist;
                }

                // Step 3 - Calculate the distance along the curve.
                float[] distsOnCurve = new float[sampleCount];

                // The distance sum.
                float distSum = 0.0F;

                // First curve dist is 0.
                distsOnCurve[0] = 0.0F;
                distSum += distsOnCurve[0];

                // Calculate the rest of the curve distances.
                for (int i = 1; i < distsOnCurve.Length; i++)
                {
                    // Calculate the distance.
                    float dist = distsOnCurve[i - 1] + sampleDists[i];

                    // Save the distance.
                    distsOnCurve[i] = dist;

                    // Adds to the distance sum.
                    distSum += dist;
                }

                // Step 4 - sum the distances and save them to the list.
                splineDists.Add(distSum);
            }

            // Returns the spline distances.
            return splineDists;
        }

        // Calculate the bezier point distances.
        public static List<float> CalculateBezierPointDistances(List<Vector3> points, bool loop, int sampleCount = 11)
        {
            // The point lengths.
            List<float> pointDists = CalculateSplinePointDistances(true, points, loop, sampleCount);

            // Return the distances.
            return pointDists;
        }

        // Calculate the catmull rom distances.
        public static List<float> CalculateCatmullRomPointDistances(List<Vector3> points, bool loop, int sampleCount = 11)
        {
            // The point lengths.
            List<float> pointDists = CalculateSplinePointDistances(false, points, loop, sampleCount);

            // Return the distances.
            return pointDists;
        }



        // Lerp - linear interpolation (standard) [self defined]
        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float t)
        {
            return ((1.0F - t) * v1 + t * v2);
        }

        // Catmull Rom - goes between points 1 and 2 using points 0 and 3 to create a curve.
        public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float u)
        {
            // the catmull-rom matrix, which has a 0.5F scalar applied from the start.
            Matrix4x4 matCatmullRom = new Matrix4x4();

            // setting the rows
            matCatmullRom.SetRow(0, new Vector4(0.5F * -1.0F, 0.5F * 3.0F, 0.5F * -3.0F, 0.5F * 1.0F));
            matCatmullRom.SetRow(1, new Vector4(0.5F * 2.0F, 0.5F * -5.0F, 0.5F * 4.0F, 0.5F * -1.0F));
            matCatmullRom.SetRow(2, new Vector4(0.5F * -1.0F, 0.5F * 0.0F, 0.5F * 1.0F, 0.5F * 0.0F));
            matCatmullRom.SetRow(3, new Vector4(0.5F * 0.0F, 0.5F * 2.0F, 0.5F * 0.0F, 0.5F * 0.0F));


            // Points
            Matrix4x4 pointsMat = new Matrix4x4();

            pointsMat.SetRow(0, new Vector4(p0.x, p0.y, p0.z, 0));
            pointsMat.SetRow(1, new Vector4(p1.x, p1.y, p1.z, 0));
            pointsMat.SetRow(2, new Vector4(p2.x, p2.y, p2.z, 0));
            pointsMat.SetRow(3, new Vector4(p3.x, p3.y, p3.z, 0));


            // Matrix for u to the power of given functions.
            Matrix4x4 uMat = new Matrix4x4(); // the matrix for 'u' (also called 't').

            // Setting the 'u' values to the proper row, since this is being used as a 1 X 4 matrix.
            uMat.SetRow(0, new Vector4(Mathf.Pow(u, 3), Mathf.Pow(u, 2), Mathf.Pow(u, 1), Mathf.Pow(u, 0)));

            // Result matrix from a calculation. 
            Matrix4x4 result;

            // Order of [u^3, u^2, u, 0] * M * <points matrix>
            // The catmull-rom matrix has already had the (1/2) scalar applied.
            result = matCatmullRom * pointsMat;

            result = uMat * result; // [u^3, u^2, u, 0] * (M * points)

            // the resulting values are stored at the top.
            return result.GetRow(0);
        }

        // Bezier - interpolates between two points using 2 control points to change the movement curve.
        public static Vector3 Bezier(Vector3 t1, Vector3 p1, Vector3 p2, Vector3 t2, float u)
        {
            // Bezier matrix
            Matrix4x4 matBezier = new Matrix4x4();

            matBezier.SetRow(0, new Vector4(-1, 3, -3, 1));
            matBezier.SetRow(1, new Vector4(3, -6, 3, 0));
            matBezier.SetRow(2, new Vector4(-3, 3, 0, 0));
            matBezier.SetRow(3, new Vector4(1, 0, 0, 0));


            // Result matrix from a calculation. 
            Matrix4x4 result;

            // The two points on the line, and their control points
            Matrix4x4 pointsMat = new Matrix4x4();

            pointsMat.SetRow(0, new Vector4(p1.x, p1.y, p1.z, 0));
            pointsMat.SetRow(1, new Vector4(t1.x, t1.y, t1.z, 0));
            pointsMat.SetRow(2, new Vector4(t2.x, t2.y, t2.z, 0));
            pointsMat.SetRow(3, new Vector4(p2.x, p2.y, p2.z, 0));


            // Matrix for 'u' to the exponent 0 through 3.
            Matrix4x4 uMat = new Matrix4x4(); // the matrix for 'u' (also called 't').

            // Setting the 'u' values to the proper row, since this is being used as a 1 X 4 matrix.
            // The exponent values are being applied as well.
            uMat.SetRow(0, new Vector4(Mathf.Pow(u, 3), Mathf.Pow(u, 2), Mathf.Pow(u, 1), Mathf.Pow(u, 0)));

            // Doing the bezier calculation
            // Order of [u^3, u^2, u, 0] * M * <points matrix>
            result = matBezier * pointsMat; // bezier matrix * points matrix
            result = uMat * result; // u matrix * (bezier matrix * points matrix)

            // the needed values are stored at the top of the result matrix.
            return result.GetRow(0);
        }




        // LERP Expanded Calculations
        // EaseIn Operation
        public static Vector3 EaseIn(Vector3 v1, Vector3 v2, float t, float pow)
        {
            return Vector3.Lerp(v1, v2, Mathf.Pow(t, pow));
        }

        // 1. EaseIn1 - Slow In, Fast Out (Quadratic)
        public static Vector3 EaseIn1(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, Mathf.Pow(t, 2));
        }

        // 2. EaseIn2 - Slow In, Fast Out (Cubic)
        public static Vector3 EaseIn2(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, Mathf.Pow(t, 3));
        }

        // 3. EaseIn3 - Slow In, Fast Out (Optic)
        public static Vector3 EaseIn3(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, Mathf.Pow(t, 8));
        }




        // EaseOut Operation
        public static Vector3 EaseOut(Vector3 v1, Vector3 v2, float t, float pow)
        {
            return Vector3.Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, pow));
        }

        // 4. EaseOut1 Operation - Fast In, Slow Out
        public static Vector3 EaseOut1(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, 2));
        }

        // 5. EaseOut2 Operation - Fast In, Slow Out (Inverse Cubic)
        public static Vector3 EaseOut2(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, 3));
        }

        // 6. EaseOut3 Operation - Fast In, Slow Out (Inverse Octic)
        public static Vector3 EaseOut3(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, 8));
        }




        // 7. EaseInOut1 - Shrink, Offset, Simplify In / Out
        public static Vector3 EaseInOut1(Vector3 v1, Vector3 v2, float t)
        {
            t = (t < 0.5F) ? 2 * Mathf.Pow(t, 2) : -2 * Mathf.Pow(t, 2) + 4 * t - 1;

            return Lerp(v1, v2, t);
        }

        // 8. EaseInOut2 - Shrink, Offset, Simplify In / Out
        // Equation: y = (x < 0.5) ? 4x ^ 3 : 4x ^ 3-12x ^ 2 + 12x - 4
        public static Vector3 EaseInOut2(Vector3 v1, Vector3 v2, float t)
        {
            t = (t < 0.5F) ? 4 * Mathf.Pow(t, 3) : 4 * Mathf.Pow(t, 3) - 12 * Mathf.Pow(t, 2) + 12 * t - 3;

            return Lerp(v1, v2, t);
        }

        // 9. EaseInOut3 - Shrink, Offset, Simplify In / Out
        public static Vector3 EaseInOut3(Vector3 v1, Vector3 v2, float t)
        {
            t = (t < 0.5F) ? 128 * Mathf.Pow(t, 8) : 0.5F + (1 - Mathf.Pow(2 * (1 - t), 8)) / 2.0F;

            return Lerp(v1, v2, t);
        }




        // 10. EaseInCircular - Inwards (Valley) Curve
        public static Vector3 EaseInCircular(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, 1.0F - Mathf.Sqrt(1 - Mathf.Pow(t, 2)));
        }

        // 11. EaseOutCircular - Outwards (Hill) Curve
        public static Vector3 EaseOutCircular(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, Mathf.Sqrt(-(t - 2) * t));
        }

        // 12. EaseInOutCircular - Curve Inward, Then Outwards (Valley -> Hill)
        public static Vector3 EaseInOutCircular(Vector3 v1, Vector3 v2, float t)
        {
            // changing the value of 't'.
            t = (t < 0.5F) ?
                0.5F * (1 - Mathf.Sqrt(1 - 4 * Mathf.Pow(t, 2))) :
                0.5F * (Mathf.Sqrt(-4 * (t - 2) * t - 3) + 1);

            return Lerp(v1, v2, t);
        }




        // EaseInBounce Operation
        public static Vector3 EaseInBounce(Vector3 v1, Vector3 v2, float t, float pow)
        {
            return Vector3.Lerp(v1, v2, Mathf.Pow(t, 2) * (pow * t - (pow - 1.0F)));
        }

        // 13. EASE_IN_BOUNCE_1 - Offset Power Composition
        public static Vector3 EaseInBounce1(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, Mathf.Pow(t, 2) * (2 * t - 1));
        }

        // 14. EASE_IN_BOUNCE_2 - Offset Power Composition
        public static Vector3 EaseInBounce2(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, Mathf.Pow(t, 2) * (3 * t - 2));
        }

        // 15. EASE_IN_BOUNCE_3 - Offset Power Composition
        public static Vector3 EaseInBounce3(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, Mathf.Pow(t, 2) * (4 * t - 3));
        }




        // EaseOutBounce operation
        public static Vector3 EaseOutBounce(Vector3 v1, Vector3 v2, float t, float pow)
        {
            // pow + 2 + pow - 1 -> pow * 2 + 1
            return Vector3.Lerp(v1, v2, t * (t * (pow * t - (pow * 2 + 1) + (pow + 2))));
        }

        // 16. EASE_OUT_BOUNCE_1 - Inverse offset, power composition
        public static Vector3 EaseOutBounce1(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, t * (t * (2 * t - 5) + 4));
        }

        // 17. EASE_OUT_BOUNCE_2 - Inverse offset, power composition
        public static Vector3 EaseOutBounce2(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, t * (t * (3 * t - 7) + 5));
        }

        // 18. EASE_OUT_BOUNCE_3 - Inverse offset, power composition
        public static Vector3 EaseOutBounce3(Vector3 v1, Vector3 v2, float t)
        {
            return Lerp(v1, v2, t * (t * (4 * t - 9) + 6));
        }





        // 19. EASE_IN_OUT_BOUNCE_1 - Shrink, offset, simplify In / Out
        public static Vector3 EaseInOutBounce1(Vector3 v1, Vector3 v2, float t)
        {
            t = (t < 0.5F) ?
                8 * Mathf.Pow(t, 3) - 2 * Mathf.Pow(t, 2) :
                8 * Mathf.Pow(t, 3) - 22 * Mathf.Pow(t, 2) + 20 * t - 5;

            return Lerp(v1, v2, t);
        }

        // 20. EASE_IN_OUT_BOUNCE_2 - Shrink, offset, simplify In / Out
        public static Vector3 EaseInOutBounce2(Vector3 v1, Vector3 v2, float t)
        {
            t = (t < 0.5F) ?
                12 * Mathf.Pow(t, 3) - 4 * Mathf.Pow(t, 2) :
                12 * Mathf.Pow(t, 3) - 32 * Mathf.Pow(t, 2) + 28 * t - 7;

            return Lerp(v1, v2, t);
        }

        // 21. EASE_IN_OUT_BOUNCE_3 - Shrink, offset, simplify In / Out
        public static Vector3 EaseInOutBounce3(Vector3 v1, Vector3 v2, float t)
        {
            t = (t < 0.5F) ?
                16 * Mathf.Pow(t, 3) - 6 * Mathf.Pow(t, 2) :
                16 * Mathf.Pow(t, 3) - 42 * Mathf.Pow(t, 2) + 36 * t - 9;

            return Lerp(v1, v2, t);
        }


        // INTERPOLATION //

        // Interpolates using the 2 provided points.
        public static Vector3 Interpolate(interType type, Vector3 v1, Vector3 v2, float t)
        {
            return Interpolate(type, v1, v1, v2, v2, t);
        }

        // Interpolate by Type
        // If bezier or catmull-rom are selected, v0 and v3 will be used accordingly.
        // If an interpolation method with 2 points are selected, v1 and v2 will be used.
        public static Vector3 Interpolate(interType type, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, float t)
        {
            Vector3 result; // result of operation

            // goes based on type
            switch (type)
            {
                case interType.lerp:
                    result = Lerp(v1, v2, t);
                    break;

                case interType.catmullRom: // Catmull Rom Curve
                    result = CatmullRom(v0, v1, v2, v3, t);
                    break;

                case interType.bezier: // Bezier Curve
                    result = Bezier(v0, v1, v2, v3, t);
                    break;

                case interType.easeIn1:
                    result = EaseIn1(v1, v2, t);
                    break;

                case interType.easeIn2:
                    result = EaseIn2(v1, v2, t);
                    break;

                case interType.easeIn3:
                    result = EaseIn3(v1, v2, t);
                    break;

                case interType.easeOut1:
                    result = EaseOut1(v1, v2, t);
                    break;

                case interType.easeOut2:
                    result = EaseOut2(v1, v2, t);
                    break;

                case interType.easeOut3:
                    result = EaseOut3(v1, v2, t);
                    break;

                case interType.easeInOut1:
                    result = EaseInOut1(v1, v2, t);
                    break;

                case interType.easeInOut2:
                    result = EaseInOut2(v1, v2, t);
                    break;

                case interType.easeInOut3:
                    result = EaseInOut3(v1, v2, t);
                    break;

                case interType.easeInCircular:
                    result = EaseInCircular(v1, v2, t);
                    break;

                case interType.easeOutCircular:
                    result = EaseOutCircular(v1, v2, t);
                    break;

                case interType.easeInOutCircular:
                    result = EaseInOutCircular(v1, v2, t);
                    break;

                case interType.easeInBounce1:
                    result = EaseInBounce1(v1, v2, t);
                    break;

                case interType.easeInBounce2:
                    result = EaseInBounce2(v1, v2, t);
                    break;

                case interType.easeInBounce3:
                    result = EaseInBounce3(v1, v2, t);
                    break;

                case interType.easeOutBounce1:
                    result = EaseOutBounce1(v1, v2, t);
                    break;

                case interType.easeOutBounce2:
                    result = EaseOutBounce2(v1, v2, t);
                    break;

                case interType.easeOutBounce3:
                    result = EaseOutBounce3(v1, v2, t);
                    break;

                case interType.easeInOutBounce1:
                    result = EaseInOutBounce1(v1, v2, t);
                    break;

                case interType.easeInOutBounce2:
                    result = EaseInOutBounce2(v1, v2, t);
                    break;

                case interType.easeInOutBounce3:
                    result = EaseInOutBounce3(v1, v2, t);
                    break;

                default: // unity lerp
                    result = Vector3.Lerp(v1, v2, t);
                    break;
            }

            return result;
        }

        // Interpolates using the 2 provided points with speed control.
        public static Vector3 InterpolateWithSpeedControl(interType type, Vector3 v1, Vector3 v2, float t)
        {
            return InterpolateWithSpeedControl(type, v1, v1, v2, v2, t);
        }

        // Interpolations usin 4 points with speed control.
        // v1 and v2 are the main points, while v0 and v3 are control points for certain functions.
        public static Vector3 InterpolateWithSpeedControl(interType type, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, float t)
        {
            // STEP 1 - Calculate sample times and sample points.
            // Calculates the samples - the 11 sample points (0 - 1, with 0.1 inc).
            const int SAMPLE_MAX = 11;
            float[] sampleTimes = new float[SAMPLE_MAX];
            Vector3[] samplePoints = new Vector3[SAMPLE_MAX];

            // The sample increment.
            float sampleInc = 1.0F / (SAMPLE_MAX - 1);

            // Calculates the samples points.
            for (int i = 0; i < samplePoints.Length; i++)
            {
                // Calculate the sample time.
                sampleTimes[i] = sampleInc * i;

                // Calculate the point.
                samplePoints[i] = Interpolate(type, v0, v1, v2, v3, sampleTimes[i]);
            }

            // Step 2 - Calculate pairwise distances.
            float[] sampleDists = new float[SAMPLE_MAX];

            // First distance is 0.
            sampleDists[0] = 0.0F;

            // Calculates the sample distances.
            for(int i = 1; i < sampleDists.Length; i++)
            {
                // Calculate the distance.
                float dist = Vector3.Distance(samplePoints[i - 1], samplePoints[i]);

                // Save the distances.
                sampleDists[i] = dist;
            }

            // Step 3 - Calculate the distance along the curve.
            float[] distsOnCurve = new float[SAMPLE_MAX];

            // First curve dist is 0.
            distsOnCurve[0] = 0.0F;

            // Calculate the rest of the curve distances.
            for(int i = 1; i < distsOnCurve.Length; i++)
            {
                // Calculate the distance.
                float dist = distsOnCurve[i - 1] + sampleDists[i];

                // Save the distance.
                distsOnCurve[i] = dist;
            }

            // Step 4 - Use t to find the line segment the point falls on.

            // New - the user provides the t-value, so check the sampleTime array to solve the distance...
            // for the equation: Speed = Distance / Time
            
            // The index taken from sample times.
            int sampleTimesIndex = -1;

            // Clamps the t-value.
            float tClamped = Mathf.Clamp01(t);
            
            // Find sample time the t-value falls into.
            for (int i = 0; i < sampleTimes.Length; i++)
            {
                // The end point has been found for sample times.
                if(tClamped <= sampleTimes[i])
                {
                    sampleTimesIndex = i;
                    break;
                }

            }

            // Checks sample times value.
            // 0, so just use 1 as the end index (0 will be the start index).
            if (sampleTimesIndex <= 0)
            {
                sampleTimesIndex = 1;
            }
            // >= Length, so just put at end of the curve (list length - 1).
            else if (sampleTimesIndex >= sampleTimes.Length)
            {
                sampleTimesIndex = sampleTimes.Length - 1;
            }


            // Step 5 - Calculate position by lerping between the two ends of the line segment.

            // Calculates the t-value between the start and end point of the curve segment.
            float curveT = Mathf.InverseLerp(
                sampleTimes[sampleTimesIndex - 1], 
                sampleTimes[sampleTimesIndex],
                tClamped);


            // Interpolates between the positions that curve T falls between.
            Vector3 finalPos = Vector3.Lerp(samplePoints[sampleTimesIndex - 1], samplePoints[sampleTimesIndex], curveT);

            // Returns the final position.
            return finalPos;
        }

        // Interpolate (at a fixed speed) - uses distance travelled to find 'T'.
        // This only really works if you're using lerp, bezier, or catmull-rom
        // If 'loop' is true, then the calculation treats the points as a loop, rather than having a start and end. 
        // 'samples' is used to determine how many points are sampled for curves.
        public static Vector3 InterpolateAtFixedSpeed(interType type, List<Vector3> points, float distance, bool loop, int samples = 10)
        {
            // Copies the points for the calculation.
            List<Vector3> pathPoints = new List<Vector3>(points);

            // The length of the whole interpolation set.
            float pathLengthTotal = 0.0F;

            // If the interpolation should loop, add the first point to the end of the list.
            if (loop)
                pathPoints.Add(points[0]);

            // The distances between points.
            List<float> pointDists = new List<float>();

            // The summed distances between points.
            List<float> pointDistSums = new List<float>();

            // The first distance is 0.
            pointDists.Add(0.0F);
            pointDistSums.Add(0.0F);

            // Checks the interpolation type.
            switch (type)
            {
                case interType.lerp: // Lerp/Default
                default:

                    // Calculates the distance between each point, and sums them together.
                    for (int i = 1; i < pathPoints.Count; i++)
                    {
                        // Calculates the distance.
                        float dist = Vector3.Distance(pathPoints[i - 1], pathPoints[i]);

                        // Adds to the total path length.
                        pathLengthTotal += dist;

                        // Add to the point distance, and the summed point distances.
                        pointDists.Add(dist);
                        pointDistSums.Add(pointDistSums[pointDistSums.Count - 1] + dist);
                    }

                    break;

                case interType.bezier: // Bezier
                case interType.catmullRom: // Catmull-Rom

                    
                    // Checks the type for calculations
                    // This is VERY inefficient since the samples are calculated twice.
                    // But since this is just a test, it's fine.
                    switch(type)
                    {
                        case interType.bezier: // Bezier curve.
                            pathPoints = CalculateBezierSamples(points, loop);
                            pointDists = CalculateBezierPointDistances(points, loop);

                            break;

                        case interType.catmullRom: // Catmull-rom curve.
                            pathPoints = CalculateCatmullRomSamples(points, loop);
                            pointDists = CalculateCatmullRomPointDistances(points, loop);

                            break;
                    }

                    // This should be 0.
                    pointDistSums[0] = pointDists[0];

                    // Sum the distances together.
                    for (int i = 1; i < pointDists.Count; i++)
                    {
                        // Sum the path length total, and ad to the point dist sums.
                        pathLengthTotal += pointDists[i];
                        pointDistSums.Add(pointDistSums[i - 1] + pointDists[i]);
                    }

                    break;  
            }

            // Puts the distance within the bounds of the interpolation.
            float distClamped = distance - Mathf.Floor(distance / pathLengthTotal) * pathLengthTotal;

            // If the distance is negative, calculate the positive distance from it.
            if (distClamped < 0)
                distClamped = pathLengthTotal - distClamped;

            // Clamps the distance within the path length total.
            distClamped = Mathf.Clamp(distClamped, 0, pathLengthTotal);


            // The end index of the path points.
            // By default, it's the end of the path.
            int endIndex = pathPoints.Count - 1;

            // Finds the points on the path the requested distance fall between.
            for(int i = 0; i < pathPoints.Count; i++)
            {
                // Found the path points.
                if (distClamped < pointDistSums[i])
                {
                    endIndex = i;
                    break;
                }
            }

            // Calculates the t-value between the two points.
            float t = Mathf.InverseLerp(pointDistSums[endIndex - 1], pointDistSums[endIndex], distClamped);

            // Calculates the resulting position.
            Vector3 resultPos;
            
            // Checks what type of calculation to do.
            switch(type)
            {
                case interType.bezier: // Bezier curve.
                    resultPos = Vector3.Lerp(pathPoints[endIndex - 1], pathPoints[endIndex], t);
                    
                    break;

                case interType.catmullRom: // Catmull-rom curve.
                    resultPos = Vector3.Lerp(pathPoints[endIndex - 1], pathPoints[endIndex], t);

                    break;

                default: // Default.
                    resultPos = Vector3.Lerp(pathPoints[endIndex - 1], pathPoints[endIndex], t);
                    break;

            }            

            // Returns the resulting position.
            return resultPos;
        }
    }
}