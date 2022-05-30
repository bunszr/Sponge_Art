using UnityEngine;
using System.Linq;
using UniRx;

public class Outline : MonoBehaviour
{
    [SerializeField] OutlineInfo outlineInfo;
    [SerializeField] Mesh mesh;

    private void Awake()
    {
        SOHolder.Ins.importants.intersectionInfoSo.Init(mesh, outlineInfo.vertices);
    }
}