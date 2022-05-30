using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using UniRx;
using Unity.Linq;
using Dreamteck.Splines;

public class OutlineMaker : MonoBehaviour
{
    [Header("To Copy")]
    public Vector3[] outlinePoints;

    private void Start()
    {
        SplineComputer computer = GetComponent<SplineComputer>();
        SplineUser user = GetComponent<SplineUser>();
        outlinePoints = user.samples.Select(x => new Vector3(x.position.x, 0, x.position.z)).ToArray();
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < outlinePoints.Length; i++)
        {
            float time = i / (float)outlinePoints.Length;
            Gizmos.color = Color.Lerp(Color.black, Color.red, time);
            Gizmos.DrawSphere(outlinePoints[i], .1f);
        }
    }
}