using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;

[CreateAssetMenu(fileName = "IntersectionInfoSo", menuName = "Sponge_Art/IntersectionInfoSo", order = 0)]
public class IntersectionInfoSo : ScriptableObject
{
    CustomMesh customMesh;
    Vector2[] outlinePointsXZ;

    List<Vector3> TEMP_intersectedPoints = new List<Vector3>();

    public IntersectionInfo CurrIntersectionInfo { get; private set; }

    public ReactiveCollection<IntersectionInfo> IntersectionInfoList;

    public void Init(Mesh mesh, Vector3[] _outlinePoints)
    {
        customMesh = new CustomMesh(mesh);
        this.outlinePointsXZ = _outlinePoints.Select(x => x.ToXZ()).ToArray();

        IntersectionInfoList = new ReactiveCollection<IntersectionInfo>();

        IntersectionInfoList.ObserveAdd().Subscribe(x => SOHolder.Ins.events.elasticBandDoneGameEvent.Raise());
        IntersectionInfoList.ObserveRemove().Subscribe(x => SOHolder.Ins.events.onCutBand.Raise());
    }

    public PairPoint GetIntersectionPairPoint(PairPoint pairPoint)
    {
        TEMP_intersectedPoints.Clear();
        for (int i = 0; i < outlinePointsXZ.Length; i++)
        {
            Vector2 intersection;
            int nextIndex = (i + 1) % outlinePointsXZ.Length;

            if (Utility.LineSegmentsIntersection(outlinePointsXZ[i], outlinePointsXZ[nextIndex], pairPoint.pointA.ToXZ(), pairPoint.pointB.ToXZ(), out intersection))
                TEMP_intersectedPoints.Add(new Vector3(intersection.x, 0, intersection.y));
        }

        if (TEMP_intersectedPoints.Count < 2) Debug.LogError("There is one intersect");
        if (TEMP_intersectedPoints.Count > 2) Debug.LogError("Outline is concav. Outline should be convex");

        return new PairPoint(TEMP_intersectedPoints[0], TEMP_intersectedPoints[1]);
    }

    public bool LineIsInsadeOfOutlinePlaneMesh()
    {
        NativeArray<bool> result = new NativeArray<bool>(1, Allocator.TempJob);
        Job_InsadeOfMesh job_InsadeOfMesh = new Job_InsadeOfMesh()
        {
            verticesXZ = customMesh.n_verticesXZ,
            triangles = customMesh.n_triangles,
            inputPositionXZ = SOHolder.Ins.importants.inputData.position.ToXZ(),
            inputPressedPositionXZ = SOHolder.Ins.importants.inputData.pressedPosition.ToXZ(),
            result = result
        };

        job_InsadeOfMesh.ScheduleParallel(customMesh.n_triangles.Length / 3, 1, default).Complete();
        bool r = result[0];
        result.Dispose();
        return r;
    }

    public class CustomMesh
    {
        public NativeArray<float2> n_verticesXZ;
        public NativeArray<int> n_triangles;

        public CustomMesh(Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;
            int[] triangle = mesh.triangles;

            n_verticesXZ = new NativeArray<float2>(vertices.Length, Allocator.Persistent);
            n_triangles = new NativeArray<int>(triangle.Length, Allocator.Persistent);

            NativeArray<float2>.Copy(vertices.Select(x => new float2(x.x, x.z)).ToArray(), n_verticesXZ);
            n_triangles.CopyFrom(triangle);

            GameManager.Ins.onDestroy += OnCustomDestroy;
        }

        void OnCustomDestroy()
        {
            n_verticesXZ.Dispose();
            n_triangles.Dispose();
        }
    }
}