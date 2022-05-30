using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

public class BendingModel : MonoBehaviour
{
    NativeArray<float3> vertices, displacedVertices;

    public BendingModelInfoSo bendingModelInfoSo;

    public float yScale = 2;

    Mesh mesh;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] verticesArray = mesh.vertices;
        Vector3[] normalsArray = mesh.normals;

        vertices = new NativeArray<float3>(verticesArray.Length, Allocator.Persistent);
        displacedVertices = new NativeArray<float3>(verticesArray.Length, Allocator.Persistent);

        for (int i = 0; i < verticesArray.Length; i++)
        {
            vertices[i] = verticesArray[i];
            displacedVertices[i] = verticesArray[i];
        }
    }

    private void OnDisable()
    {
        vertices.Dispose();
        displacedVertices.Dispose();
    }

    public void Bend(NativeArray<Job_Bend.BendInfo> bendInfos)
    {
        Job_Bend bendJob = new Job_Bend()
        {
            originalVertices = vertices,
            displacedVertices = displacedVertices,
            force = bendingModelInfoSo.force,
            sqrDisFactor = bendingModelInfoSo.sqrDisFactor,
            yScale = yScale,
            bendInfos = bendInfos
        };

        bendJob.ScheduleParallel(vertices.Length, 128, default).Complete();
        mesh.SetVertices(displacedVertices);
    }
}