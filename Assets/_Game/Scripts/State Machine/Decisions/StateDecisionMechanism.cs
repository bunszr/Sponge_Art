using UnityEngine;
using System.Linq;
using UniRx;

public class StateDecisionMechanism : IState
{
    ISelectableState _selectableState;

    protected ReactiveCollection<IntersectionInfo> IntersectionInfoList;
    protected PairPoint[] levelInfoPairPoints;

    public StateDecisionMechanism(ISelectableState _selectableState)
    {
        this._selectableState = _selectableState;
    }

    public void Enter()
    {
        IntersectionInfoList = SOHolder.Ins.importants.intersectionInfoSo.IntersectionInfoList;
        levelInfoPairPoints = SOHolder.Ins.importants.levelInfoManager.CurrLevelInfo.intersectedPairPointsForCalculation;

        GameManager.Ins.stateMachine.ChangeState(_selectableState.GetState(CalculateAccuracy()));
    }

    public void Exit() { }

    float CalculateAccuracy()
    {
        float accuracy = 0;
        foreach (var pairLevel in levelInfoPairPoints)
        {
            Vector3 rhs = (pairLevel.pointA - pairLevel.pointB).normalized;
            IntersectionInfo[] approximatelyParallelLines = IntersectionInfoList
                .Where(x => Mathf.Abs(Vector3.Dot(rhs, (x.pointA - x.pointB).normalized)) > .7f)
                .ToArray();

            if (approximatelyParallelLines.Length != 0)
            {
                Vector3 closestMidPoint = approximatelyParallelLines.OrderBy(x => Vector3.Distance(pairLevel.MidPoint, x.MidPoint)).FirstOrDefault().MidPoint;
                accuracy += Mathf.InverseLerp(1, 0, Vector3.Distance(closestMidPoint, pairLevel.MidPoint));
            }
        }

        accuracy = accuracy / levelInfoPairPoints.Length;
        SOHolder.Ins.variables.currAccuracyFV.value = accuracy;
        return accuracy;
    }
}