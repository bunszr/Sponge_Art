using Unity.Burst;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;

[BurstCompile]
public struct Job_InsadeOfMesh : IJobFor
{
    [ReadOnly] public NativeArray<float2> verticesXZ;
    [ReadOnly] public NativeArray<int> triangles;
    [ReadOnly] public float2 inputPositionXZ, inputPressedPositionXZ;
    [NativeDisableParallelForRestriction] public NativeArray<bool> result;

    public void Execute(int index)
    {
        for (int i = 0; i < 20; i++)
        {
            float2 point = math.lerp(inputPressedPositionXZ, inputPositionXZ, i / 20f);
            if (PointInTriangle(verticesXZ[triangles[index * 3 + 0]], verticesXZ[triangles[index * 3 + 1]], verticesXZ[triangles[index * 3 + 2]], point))
            {
                result[0] = true;
            }
        }
    }

    // Author: Sebastian Lague
    public static bool PointInTriangle(float2 a, float2 b, float2 c, float2 p)
    {
        float area = 0.5f * (-b.y * c.x + a.y * (-b.x + c.x) + a.x * (b.y - c.y) + b.x * c.y);
        float s = 1 / (2 * area) * (a.y * c.x - a.x * c.y + (c.y - a.y) * p.x + (a.x - c.x) * p.y);
        float t = 1 / (2 * area) * (a.x * b.y - a.y * b.x + (a.y - b.y) * p.x + (b.x - a.x) * p.y);
        return s >= 0 && t >= 0 && (s + t) <= 1;
    }
}