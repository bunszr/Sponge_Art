using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Dreamteck.Splines;
using DG.Tweening;

public class CuttingElasticBand : ElasticBandAbs
{
    Vector3[] defaultSplinePointPosition;
    Vector3 defaultPos;

    private void Start()
    {
        defaultSplinePointPosition = splineComputer.GetPoints(SplineComputer.Space.Local).Select(x => x.position).ToArray();
        defaultPos = transform.position;
    }

    public void ReturnDefaultPos()
    {
        transform.rotation = Quaternion.identity;
        transform.position = defaultPos;
        for (int i = 0; i < defaultSplinePointPosition.Length; i++)
        {
            splineComputer.SetPointPosition(i, defaultSplinePointPosition[i], SplineComputer.Space.Local);
        }
    }

    public Tween DOLocalMoveSplinePointPos(int index, Vector3 worldPos, float duration)
    {
        Vector3 localPosition = splineComputer.GetPointPosition(index, Dreamteck.Splines.SplineComputer.Space.Local);
        Vector3 endPos = transform.InverseTransformPoint(worldPos);
        return DOTween.To(() => localPosition, x => localPosition = x, endPos, duration).OnUpdate(() =>
        {
            splineComputer.SetPointPosition(index, localPosition, Dreamteck.Splines.SplineComputer.Space.Local);
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(splineComputer.GetPointPosition(0), .3f);
        Gizmos.DrawSphere(splineComputer.GetPointPosition(2), .3f);
    }
}