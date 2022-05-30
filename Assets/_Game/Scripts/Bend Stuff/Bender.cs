using UnityEngine;
using DG.Tweening;
using UniRx;
using Unity.Collections;

public class Bender : MonoBehaviour
{
    BendingModel bendingModel;
    InputData inputData;

    float defaultSqrDisFactor;
    public float minSqrDisFactor = 80;

    public bool deformeAllMeshWhenAddedBand = true;
    public BendingModelInfoSo bendingModelInfoSo;

    public bool updateEveryFrame = false; // To find a correct, beatiful value

    private void Start()
    {
        inputData = SOHolder.Ins.importants.inputData;
        bendingModel = FindObjectOfType<BendingModel>();
        defaultSqrDisFactor = bendingModel.bendingModelInfoSo.sqrDisFactor;
        SOHolder.Ins.events.elasticBandDoneGameEvent.RegisterListener(Bend);
        SOHolder.Ins.events.elasticBandDoneGameEvent.RegisterListener(DeformeAllMesh);
    }

    private void OnDestroy()
    {
        SOHolder.Ins.events.elasticBandDoneGameEvent.UnregisterListener(Bend);
        SOHolder.Ins.events.elasticBandDoneGameEvent.UnregisterListener(DeformeAllMesh);

        bendingModel.bendingModelInfoSo.sqrDisFactor = defaultSqrDisFactor;
    }


    private void Update()
    {
        if (updateEveryFrame) Bend();
    }

    public void Bend()
    {
        ReactiveCollection<IntersectionInfo> intersectionInfoList = SOHolder.Ins.importants.intersectionInfoSo.IntersectionInfoList;
        NativeArray<Job_Bend.BendInfo> bendInfos = new NativeArray<Job_Bend.BendInfo>(intersectionInfoList.Count, Allocator.TempJob);
        for (int i = 0; i < intersectionInfoList.Count; i++)
        {
            bendInfos[i] = new Job_Bend.BendInfo(intersectionInfoList[i].readonlyPointA, intersectionInfoList[i].readonlyPointB);
        }
        bendingModel.Bend(bendInfos);
        bendInfos.Dispose();
    }

    Tween tween;

    public void DeformeAllMesh()
    {
        if (!deformeAllMeshWhenAddedBand) return;

        if (tween != null && tween.IsPlaying()) tween.Complete(true);

        float sqrDisFactor = minSqrDisFactor;
        tween = DOTween.To(() => sqrDisFactor, x => sqrDisFactor = x, defaultSqrDisFactor, .5f).OnUpdate(() =>
        {
            bendingModel.bendingModelInfoSo.sqrDisFactor = sqrDisFactor;
            Bend();
        });
    }
}