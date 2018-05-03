using UnityEngine;

public static class Bezier
{

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t); 
        // Interpolates between two vectors; Connects p0 and p2 through t
        // In order to include the middle point, we must interpolate more than once
        // This can be writen as a quadratic formula instead: B(t) = (1 - t) P0 + tP1
        // Or B(t) = (1 - t) ^ 2 + 2(1- t)t * P1 + t^2 * P2
        // For cubic curves, we need a forth point so the formula goes another step
        // B(t) = (1 - t)^3 P0 + 3(1 - t)^2 t P1 + 3(1 - t) t^2 P2 + t^3 P3

        t = Mathf.Clamp01(t);
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return
            p0 * (omt2 * omt) +
            p1 * (3f * omt2 * t) +
            p2 * (3f * omt * t2) +
            p3 * (t2 * t);
    }
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // Calculate the first derivative of the quadratic formula
        // This produces lines tangent to the curve; speed that we move along the curve
        // We do not need the second derivate because there will be no t, and the curves would have a constant acceleration
        t = Mathf.Clamp01(t);
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return
            (p1 - p0) * 3f * omt2 +
            (p2 - p1) * 6f * omt * t +
            (p3 - p2) * 3f * t2;
    }

}
