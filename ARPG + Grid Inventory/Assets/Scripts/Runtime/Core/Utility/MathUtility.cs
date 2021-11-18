using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    public static float GetAngleWithSign(Vector3 v1, Vector3 v2)
    {
        //int sign = Vector3.Cross(v1, v2).z < 0 ? -1 : 1;

        //return sign;

        var angle = Vector3.Angle(v1, v2);
        var cross = Vector3.Cross(v1, v2);
        if (cross.y < 0) angle = -angle;

        return angle;
    }

    public static int GetAngleSign(Vector3 v1, Vector3 v2)
    {
        int sign;

        if (Vector3.Cross(v1, v2).y < 0)
            sign = -1;
        else
            sign = 1;

        return sign;
    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        var value = a - b < 0 ? -(a - b) : a - b;

        return value <= threshold;
    }

    public static Quaternion RotationFromVector(Transform transform, Vector3 direction)
    {
        var angle = Vector3.Angle(transform.forward, direction);
        var axis = transform.up;

        var q = new Quaternion
        {
            x = axis.x * Mathf.Sin(angle / 2),
            y = axis.y * Mathf.Sin(angle / 2),
            z = axis.z * Mathf.Sin(angle / 2),
            w = Mathf.Cos(angle / 2)
        };
        
        return q;
    }

    public static float ManhattanDistance(Vector3 v1, Vector3 v2)
    {
        return Mathf.Abs(v1.x - v2.x) + Mathf.Abs(v1.y - v2.y) + Mathf.Abs(v1.z - v2.z);
    }
}
