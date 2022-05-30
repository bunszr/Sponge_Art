using UnityEngine;

public class IntersectionInfo
{
    public ElasticBandAbs elasticBandAbs;

    public readonly Vector3 readonlyPointA, readonlyPointB;
    public Vector3 pointA, pointB; // for expand

    public Vector3 MidPoint => (readonlyPointA + readonlyPointB) * .5f;
    public float DistanceAToB => Vector3.Distance(readonlyPointA, readonlyPointB);

    public IntersectionInfo(Vector3 pointA, Vector3 pointB)
    {
        this.readonlyPointA = pointA;
        this.readonlyPointB = pointB;
        this.pointA = pointA;
        this.pointB = pointB;
    }

    public IntersectionInfo(Vector3 pointA, Vector3 pointB, ElasticBandAbs elasticBandAbs)
    {
        this.readonlyPointA = pointA;
        this.readonlyPointB = pointB;
        this.pointA = pointA;
        this.pointB = pointB;
        this.elasticBandAbs = elasticBandAbs;
    }

    public void Expand(float amount)
    {
        PairPoint pair = Utility.Expand(amount, new PairPoint(readonlyPointA, readonlyPointB));
        pointA = pair.pointA;
        pointB = pair.pointB;
    }
}
