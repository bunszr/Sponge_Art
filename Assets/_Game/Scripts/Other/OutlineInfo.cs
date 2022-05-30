using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using UniRx;
using Unity.Linq;

[CreateAssetMenu(fileName = "OutlineInfo", menuName = "Sponge_Art/OutlineInfo", order = 0)]
public class OutlineInfo : ScriptableObject
{
    public Vector3[] vertices;
}