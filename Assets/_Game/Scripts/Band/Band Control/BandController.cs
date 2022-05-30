using UnityEngine;
using System.Linq;
using UniRx;

public class BandController : MonoBehaviour, IState
{
    const float minDistanceForEffectedScaleReducer = 1;
    const float scaleReducerFactor = -.25f;

    IntersectionInfoSo intersectionInfoSo;
    InputData inputData;
    bool hasActive = true;

    [SerializeField] State nextState = State.DecisionMechanism_A;
    [SerializeField] float bandScaleXFactor = 2;
    [SerializeField] ElasticBandPooler elasticBandPooler;

    [HideInInspector] public ElasticBand CurrElasticBand { get; private set; }

    private void Start()
    {
        inputData = SOHolder.Ins.importants.inputData;
        intersectionInfoSo = SOHolder.Ins.importants.intersectionInfoSo;

        inputData.HasDrag.SkipLatestValueOnSubscribe().Where(x => x == true && hasActive).Subscribe(x => OnHasDragTrueMethod());
        inputData.HasDrag.SkipLatestValueOnSubscribe().Where(x => x == false && hasActive).Subscribe(x => OnHasDragFalseMethod());

        intersectionInfoSo.IntersectionInfoList.ObserveRemove().Subscribe(x => OnCutBandMethod(x.Value));

    }

    public void Enter()
    {
        SOHolder.Ins.events.onState_BandControllerEnter.Raise();
        hasActive = true;
    }

    public void Exit()
    {
        GameManager.Ins.onCustomUpdate -= CustomUpdate;
        hasActive = false;
    }

    void CustomUpdate()
    {
        Vector3 position = (inputData.pressedPosition + inputData.position) * .5f;
        position.y = .078f;
        CurrElasticBand.transform.position = position;
        CurrElasticBand.transform.rotation = Quaternion.FromToRotation(Vector3.right, inputData.Direction);

        CurrElasticBand.SetScaleX(inputData.DirectionVector.magnitude * bandScaleXFactor);
    }

    public void OnHasDragTrueMethod()
    {
        CurrElasticBand = elasticBandPooler.elasticBandPool.Rent();

        CurrElasticBand.transform.position = inputData.pressedPosition;
        CurrElasticBand.SetScaleX(0);
        CurrElasticBand.transform.rotation = Quaternion.FromToRotation(Vector3.right, inputData.Direction);

        GameManager.Ins.onCustomUpdate += CustomUpdate;
    }

    public void OnHasDragFalseMethod()
    {
        GameManager.Ins.onCustomUpdate -= CustomUpdate;

        if (inputData.Direction == Vector3.zero) // if one click then revert back
        {
            elasticBandPooler.elasticBandPool.Return(CurrElasticBand);
            return;
        }

        if (intersectionInfoSo.LineIsInsadeOfOutlinePlaneMesh() == false) FailureBand();
        else SuccessfullBand();
    }

    public void SuccessfullBand()
    {
        if (CurrElasticBand == null) return;

        PairPoint expandedPairPoint = PairPoint.Expand(100, new PairPoint(inputData.pressedPosition, inputData.position));
        PairPoint intersectedPairPoint = intersectionInfoSo.GetIntersectionPairPoint(expandedPairPoint);

        IntersectionInfo intersectionInfo = new IntersectionInfo(intersectedPairPoint.pointA, intersectedPairPoint.pointB, CurrElasticBand);

        CurrElasticBand.transform.position = intersectionInfo.MidPoint;

        // if intersect of two points distance less than minDistanceForEffectedScaleReducer variable. Then shrink band. To prevent bad visual
        float percent = 1f - Mathf.InverseLerp(0, minDistanceForEffectedScaleReducer, intersectionInfo.DistanceAToB);
        float adder = percent * scaleReducerFactor;

        CurrElasticBand.SetScaleX(intersectionInfo.DistanceAToB * bandScaleXFactor + adder);

        CurrElasticBand = null;
        intersectionInfoSo.IntersectionInfoList.Add(intersectionInfo);
        GameManager.Ins.stateMachine.ChangeState(nextState);
    }

    public void FailureBand()
    {
        SOHolder.Ins.events.elasticBandFailGameEvent.Raise();
        elasticBandPooler.elasticBandPool.Return(CurrElasticBand);
    }

    void OnCutBandMethod(IntersectionInfo intersectionInfo)
    {
        elasticBandPooler.elasticBandPool.Return(intersectionInfo.elasticBandAbs.GetComponent<ElasticBand>());
    }
}