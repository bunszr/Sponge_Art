using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public static class Utility
{
    public static Vector3 ToRound(this Vector3 v3, float mul) => new Vector3(RoundTo(v3.x, mul), RoundTo(v3.y, mul), RoundTo(v3.z, mul));
    public static float RoundTo(this float value, float mul = 1) => Mathf.Round(value / mul) * mul;
    public static Vector3 RotateXYPlane(this Vector3 vector) => new Vector3(vector.y, -vector.x, 0);
    public static Vector3 RotateXZPlane(this Vector3 vector) => new Vector3(vector.z, 0, -vector.x);
    public static Vector3 RotateYZPlane(this Vector3 vector) => new Vector3(0, vector.z, -vector.y);
    public static Vector2 ToXZ(this Vector3 vector) => new Vector2(vector.x, vector.z);

    public static float FindDegree(Vector3 v, DegreeSpace space = DegreeSpace.xy)
    {
        float angle = space == DegreeSpace.xy ? Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg : Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        return angle;
    }

    public static PairPoint Expand(float amount, PairPoint pairPoint)
    {
        Vector3 midPoint = (pairPoint.pointA + pairPoint.pointB) * .5f;
        float magnitudeBetweenMidAndPoint = Vector3.Distance(pairPoint.pointA, midPoint);
        Vector3 dirVector = (pairPoint.pointB - midPoint).normalized;
        pairPoint.pointA = midPoint - dirVector * (magnitudeBetweenMidAndPoint + amount);
        pairPoint.pointB = midPoint + dirVector * (magnitudeBetweenMidAndPoint + amount);
        return pairPoint;
    }

    public static void Foreach<T>(this IEnumerable<T> aaa, Action<int, T> action)
    {
        int i = 0;
        foreach (var item in aaa) action(++i, item);
    }

    public static void Foreach<T>(this IEnumerable<T> aaa, Action<T> action)
    {
        foreach (var item in aaa) action(item);
    }


    public static Vector3 GetClosestPoint(Vector3 point, List<Vector3> pointList)
    {
        float closestDst = float.MaxValue;
        Vector3 closestPoint = Vector3.zero;
        for (int i = 0; i < pointList.Count; i += 2)
        {
            float sqrDst = Vector3.Distance(point, pointList[i]);
            if (sqrDst < closestDst)
            {
                closestDst = sqrDst;
                closestPoint = pointList[i];
            }
        }
        return closestPoint;
    }

    public static Vector3 PlaneRaycast(Vector3 inNormal, Vector3 inPoint)
    {
        Plane plane = new Plane(inNormal, inPoint);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
            return ray.GetPoint(rayDistance);
        return Vector3.zero;
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2) => (value - from1) / (to1 - from1) * (to2 - from2) + from2;

    public static Vector3 GetRandomPointInBounds(this Bounds bounds, float y = 0)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            y,
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
    }

    public static Vector3 GetRandomPointInBounds(this Bounds bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
    }

    public static Vector3 ClampedPointInBounds(this Vector3 v, Bounds bounds)
    {
        v.x = Mathf.Clamp(v.x, bounds.min.x, bounds.max.x);
        v.y = Mathf.Clamp(v.y, bounds.min.y, bounds.max.y);
        v.z = Mathf.Clamp(v.z, bounds.min.z, bounds.max.z);
        return v;
    }

    public static Vector3 GetPercentOfPointInBounds(this Vector3 v, Bounds bounds)
    {
        v.x = Mathf.InverseLerp(bounds.min.x, bounds.max.x, v.x);
        v.y = Mathf.InverseLerp(bounds.min.y, bounds.max.y, v.y);
        v.z = Mathf.InverseLerp(bounds.min.z, bounds.max.z, v.z);
        return v;
    }

    public static Vector3 GetPointWitPercentInBounds(Vector3 percent, Bounds bounds)
    {
        Vector3 v = Vector3.zero;
        v.x = Mathf.Lerp(bounds.min.x, bounds.max.x, percent.x);
        v.y = Mathf.Lerp(bounds.min.y, bounds.max.y, percent.y);
        v.z = Mathf.Lerp(bounds.min.z, bounds.max.z, percent.z);
        return v;
    }

    public static void ToggleActivation(this GameObject gameObject, float duration)
    {
        GameManager.Ins.StartCoroutine(ToggleActivationIE(gameObject, duration));
    }

    static IEnumerator ToggleActivationIE(GameObject gameObject, float duration)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public static Tween TurnAround(this Transform transform, Vector3 pivot, Vector3 axis, int loops = 1, float angleStep = 100, float duration = 1)
    {
        float angle = 0;
        return DOTween.To(() => angle, x => angle = x, 360, duration).SetLoops(loops).OnUpdate(() =>
        {
            transform.RotateAround(pivot, axis, angleStep * Time.deltaTime);
        });
    }


    public static void Shuffle<T>(System.Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    public enum DegreeSpace { xy, xz }

    // https://github.com/setchi/Unity-LineSegmentsIntersection/blob/master/Assets/LineSegmentIntersection/Scripts/Math2d.cs
    public static bool LineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

        if (d == 0.0f)
        {
            return false;
        }

        var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }

        intersection.x = p1.x + u * (p2.x - p1.x);
        intersection.y = p1.y + u * (p2.y - p1.y);

        return true;
    }
}