using UnityEngine;
using DG.Tweening;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class StickerState : MonoBehaviour, IState
{
    [SerializeField] Transform spongeToSquishYScaleAnimT;
    [SerializeField] Transform pressT;
    [SerializeField] PlayableDirector playableDirector;

    // [Button]
    // Called by signalReceiver component
    public void MakeSticker()
    {
        Renderer renderer = GameManager.Ins.bendingModelFilter.GetComponent<Renderer>();
        renderer.material.SetTexture("_StickerTex", SOHolder.Ins.importants.levelInfoManager.CurrLevelInfo.sticker);
        renderer.material.SetInt("_Visible", 1);
        SOHolder.Ins.events.onMakeSticker.Raise();
    }

    // Called by signalReceiver component
    public void Squish()
    {
        float empty = 0;
        DOTween.To(() => empty, x => empty = x, 1, 1).OnUpdate(() =>
        {
            GameManager.Ins.bendingModelFilter.transform.localScale = spongeToSquishYScaleAnimT.transform.localScale;
        });
    }

    public void Enter()
    {
        playableDirector.stopped += OnStopAnim;
        playableDirector.Play();
    }

    public void Exit() { }

    public void OnStopAnim(PlayableDirector playableDirector)
    {
        GameManager.Ins.stateMachine.ChangeState(State.FinalState);
    }

    // [Button]
    public void JumpThis()
    {
        GameManager.Ins.stateMachine.ChangeState(State.StickerState);
    }
}