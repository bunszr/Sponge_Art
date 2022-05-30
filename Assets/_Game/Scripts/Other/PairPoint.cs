using UnityEngine;

[System.Serializable]
public struct PairPoint
{
    public Vector3 pointA;
    public Vector3 pointB;

    public Vector3 MidPoint => (pointA + pointB) * .5f;
    public float Distance => Vector3.Distance(pointA, pointB);

    public PairPoint(Vector3 pointA, Vector3 pointB)
    {
        this.pointA = pointA;
        this.pointB = pointB;
    }

    public static PairPoint Expand(float amount, PairPoint pairPoint)
    {
        return Utility.Expand(amount, pairPoint);
    }
}