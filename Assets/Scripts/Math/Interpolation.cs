﻿using System.Collections;
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

        // SELF DEFINED
        // LERP - linear interpolation (standard)
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


        // Interpolates using the 2 provided points.
        public static Vector3 InterpolateByType(interType type, Vector3 v1, Vector3 v2, float t)
        {
            return InterpolateByType(type, v1, v1, v2, v2, t);
        }

        // Interpolate by Type
        // If bezier or catmull-rom are selected, v0 and v3 will be used accordingly.
        // If an interpolation method with 2 points are selected, v1 and v2 will be used.
        public static Vector3 InterpolateByType(interType type, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, float t)
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
    }
}