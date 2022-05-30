using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using UniRx;
using Unity.Linq;

public class FinalState : MonoBehaviour, IState
{
    public void Enter()
    {
        SOHolder.Ins.events.onState_FinalStateEnter.Raise();
    }

    public void Exit()
    {
    }
}