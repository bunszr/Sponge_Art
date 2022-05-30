using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UniRx;

public class BenderWithControlPoints : MonoBehaviour
{
    BendingModel bendingModel;

    public float bendForce = 20f;
    [Range(0, 1f)] public float damping = 5f;

    public float force = 10f;
    public float sqrDisFactor = 20f;

    public Transform holderOfControlPoints;

    [Header("To copy from this to LevelInfoManager (For Accuracy Calculation)")]
    public PairPoint[] pairPoints;


    private void Start()
    {
        bendingModel = FindObjectOfType<BendingModel>();
    }

    private void Update()
    {
        int childIndex = 0;
        NativeArray<Job_Bend.BendInfo> bendInfos = new NativeArray<Job_Bend.BendInfo>(holderOfControlPoints.childCount / 2, Allocator.TempJob);
        pairPoints = new PairPoint[bendInfos.Length];
        for (int i = 0; i < bendInfos.Length; i++)
        {
            Vector3 pointA = bendingModel.transform.InverseTransformPoint(holderOfControlPoints.GetChild(childIndex++).position);
            Vector3 pointB = bendingModel.transform.InverseTransformPoint(holderOfControlPoints.GetChild(childIndex++).position);
            bendInfos[i] = new Job_Bend.BendInfo(pointA, pointB);
            pairPoints[i] = SOHolder.Ins.importants.intersectionInfoSo.GetIntersectionPairPoint(new PairPoint(pointA, pointB));
        }

        bendingModel.Bend(bendInfos);
        bendInfos.Dispose();
    }
}