using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class DecisionMechanism_A : ISelectableState
{
    public State GetState(float accuracy)
    {
        State nextState = State.BandController;

        ReactiveCollection<IntersectionInfo> IntersectionInfoList = SOHolder.Ins.importants.intersectionInfoSo.IntersectionInfoList;
        PairPoint[] levelInfoPairPoints = SOHolder.Ins.importants.levelInfoManager.CurrLevelInfo.intersectedPairPointsForCalculation;

        float stepPercent = 1f / levelInfoPairPoints.Length;
        float mustBePercent = stepPercent * IntersectionInfoList.Count;

        if (accuracy <= mustBePercent - .15f) nextState = State.BandReleaser;

        if (IntersectionInfoList.Count == levelInfoPairPoints.Length)
        {
            if (accuracy >= SOHolder.Ins.variables.minAccuracyFV.value) nextState = State.StickerState;
            else nextState = State.BandReleaser;
        }

        return nextState;
    }
}