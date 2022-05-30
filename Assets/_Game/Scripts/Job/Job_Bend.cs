using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct Job_Bend : IJobFor
{
    [ReadOnly] public NativeArray<float3> originalVertices;
    [WriteOnly] public NativeArray<float3> displacedVertices;

    [ReadOnly] public float force, sqrDisFactor, yScale;
    [ReadOnly] public NativeArray<BendInfo> bendInfos;

    public void Execute(int index)
    {
        float3 velocityXYZ = float3.zero;
        float3 vertex = originalVertices[index];
        float3 vertexPosX0Z = new float3(vertex.x, 0, vertex.z);

        for (int b = 0; b < bendInfos.Length; b++)
        {
            float3 lineMidPoint = (bendInfos[b].pointA + bendInfos[b].pointB) * .5f;
            float3 dirFromMidPointToVertex = math.normalizesafe(vertex - lineMidPoint);

            float3 closestPointOnLineSegment = ClosestPointOnLineSegment(vertexPosX0Z, bendInfos[b].pointA, bendInfos[b].pointB);
            float sqrDistanceToClosest = math.distancesq(closestPointOnLineSegment, vertexPosX0Z);

            float attenuatedForce = force / (1f + sqrDistanceToClosest * sqrDisFactor);

            velocityXYZ += dirFromMidPointToVertex * attenuatedForce;

            float distance = math.distance(vertex, lineMidPoint);
            velocityXYZ = ClampMagnitude(velocityXYZ, distance - .1f);
        }
        velocityXYZ.y = math.clamp(velocityXYZ.y, -yScale, yScale);

        displacedVertices[index] = originalVertices[index] - velocityXYZ;
    }

    // Author: Sebastian Lague
    public float3 ClosestPointOnLineSegment(float3 p, float3 a, float3 b)
    {
        float3 aB = b - a;
        float3 aP = p - a;
        float sqrLenAB = math.distancesq(b, a);

        if (sqrLenAB == 0)
            return a;

        float t = math.saturate(math.dot(aP, aB) / sqrLenAB);
        return a + aB * t;
    }

    public float3 ClampMagnitude(float3 vector, float maxLength)
    {
        if (math.lengthsq(vector) > maxLength * maxLength)
            return math.normalizesafe(vector) * maxLength;
        return vector;
    }

    public struct BendInfo
    {
        public readonly float3 pointA, pointB;

        public BendInfo(float3 pointA, float3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }
}