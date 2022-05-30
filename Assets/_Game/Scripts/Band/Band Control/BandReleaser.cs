using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using UniRx;
using Unity.Linq;
using Unity.Collections;
using UniRx.Toolkit;

public class BandReleaser : MonoBehaviour, IState
{
    BendingModel bendingModel;
    Bender bender;
    NativeArray<Job_Bend.BendInfo> bendInfos;

    CuttingElasticBand CurrCuttingBand { get; set; }

    BandPooler bandPooler;
    [SerializeField] CuttingElasticBand cuttingElasticBandPrefab;

    public float cutAnimDuration = .3f;

    private void Start()
    {
        bandPooler = new BandPooler(cuttingElasticBandPrefab, transform);
        gameObject.Children().ForEach(go => bandPooler.Return(go.GetComponent<CuttingElasticBand>()));

        bendingModel = FindObjectOfType<BendingModel>();
        bender = FindObjectOfType<Bender>();

        SOHolder.Ins.events.onMakeSticker.RegisterListener(CutAllBandSameTime);

    }

    private void OnDestroy()
    {
        SOHolder.Ins.events.onMakeSticker.UnregisterListener(CutAllBandSameTime);
    }

    public void Enter()
    {
        Invoke("CutAllBandAsRecursive", .5f);
        SOHolder.Ins.events.onReleaseAllBandStartGameEvent.Raise();
    }

    public void Exit()
    {
    }

    // [Button]
    void CutAllBandAsRecursive()
    {
        Queue<IntersectionInfo> intersectionInfoQueue = new Queue<IntersectionInfo>(SOHolder.Ins.importants.intersectionInfoSo.IntersectionInfoList.Reverse());

        InitNativeArrayForDeforming();
        CutBandRecursive(intersectionInfoQueue, bendInfos);
    }

    // [Button]
    void CutAllBandSameTime()
    {
        ReactiveCollection<IntersectionInfo> infoList = SOHolder.Ins.importants.intersectionInfoSo.IntersectionInfoList;
        for (int i = infoList.Count - 1; i >= 0; i--)
        {
            RunCutBandAnim(infoList[i], .5f);
            infoList.Remove(infoList[i]);
        }
    }

    void InitNativeArrayForDeforming()
    {
        ReactiveCollection<IntersectionInfo> intersectionInfoList = SOHolder.Ins.importants.intersectionInfoSo.IntersectionInfoList;
        bendInfos = new NativeArray<Job_Bend.BendInfo>(intersectionInfoList.Count, Allocator.Persistent);
        for (int i = 0; i < bendInfos.Length; i++)
            bendInfos[i] = new Job_Bend.BendInfo(intersectionInfoList[i].readonlyPointA, intersectionInfoList[i].readonlyPointB);
    }

    IEnumerator AllCutCompleteEI()
    {
        yield return new WaitForSeconds(.5f);
        SOHolder.Ins.events.onReleaseAllBandDoneGameEvent.Raise();
        GameManager.Ins.stateMachine.ChangeState(State.BandController);
    }

    void CutBandRecursive(Queue<IntersectionInfo> intersectionInfoQueue, NativeArray<Job_Bend.BendInfo> bendInfos)
    {
        int lastIndex = intersectionInfoQueue.Count - 1;
        if (lastIndex == -1)
        {
            bendInfos.Dispose();
            StartCoroutine(AllCutCompleteEI());
            return;
        }

        IntersectionInfo intersectionInfo = intersectionInfoQueue.Dequeue();
        SOHolder.Ins.importants.intersectionInfoSo.IntersectionInfoList.Remove(intersectionInfo);

        RunCutBandAnim(intersectionInfo, 2);
        StartCoroutine(OnRentMethodIE(CurrCuttingBand));

        float yPos = 0;
        DOTween.To(() => yPos, x => yPos = x, .5f, cutAnimDuration)
            .OnUpdate(() =>
            {
                Vector3 pointA = bendInfos[lastIndex].pointA; pointA += Vector3.up * yPos;
                Vector3 pointB = bendInfos[lastIndex].pointB; pointB += Vector3.up * yPos;
                bendInfos[lastIndex] = new Job_Bend.BendInfo(pointA, pointB);
                bendingModel.Bend(bendInfos);
            })
            .OnComplete(() => CutBandRecursive(intersectionInfoQueue, bendInfos));

    }

    public void RunCutBandAnim(IntersectionInfo intersectionInfo, float expand)
    {
        CurrCuttingBand = bandPooler.Rent();
        CurrCuttingBand.transform.position = intersectionInfo.MidPoint;
        CurrCuttingBand.transform.rotation = Quaternion.FromToRotation(Vector3.right, intersectionInfo.readonlyPointB - intersectionInfo.readonlyPointA);

        intersectionInfo.Expand(expand);
        CurrCuttingBand.DOLocalMoveSplinePointPos(0, intersectionInfo.pointA, cutAnimDuration);
        CurrCuttingBand.DOLocalMoveSplinePointPos(2, intersectionInfo.pointB, cutAnimDuration);
    }

    IEnumerator OnRentMethodIE(CuttingElasticBand cuttingElasticBand)
    {
        yield return new WaitForSeconds(cutAnimDuration);
        bandPooler.Return(cuttingElasticBand);
        cuttingElasticBand.ReturnDefaultPos();
    }


    public class BandPooler : ObjectPool<CuttingElasticBand>
    {
        readonly CuttingElasticBand prefab;
        readonly Transform hierarchyParent;

        public BandPooler(CuttingElasticBand prefab, Transform hierarchyParent)
        {
            this.prefab = prefab;
            this.hierarchyParent = hierarchyParent;
        }

        protected override CuttingElasticBand CreateInstance()
        {
            CuttingElasticBand elasticBand = GameObject.Instantiate<CuttingElasticBand>(prefab);
            elasticBand.transform.SetParent(hierarchyParent);
            return elasticBand;
        }
    }
}