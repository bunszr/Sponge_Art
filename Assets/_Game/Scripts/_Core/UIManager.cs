using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Transform gamePanel;
    public Transform winPanel;

    public Text levelNameText;
    public Image requestedImage;
    public Text bandCountText;

    public Image accuracyImageSlider;
    public Text accuracyText;

    public Text moneyText;

    private void Start()
    {
        SOHolder.Ins.importants.intersectionInfoSo.IntersectionInfoList.ObserveCountChanged(true).Subscribe(x => PrintRemainingBandCount(x));
        requestedImage.sprite = SOHolder.Ins.importants.levelInfoManager.CurrLevelInfo.requsetedModelSprite;
        levelNameText.text = "Level " + (SOHolder.Ins.importants.levelManager.LevelSceneInfo.LevelIndex + 1);

        SOHolder.Ins.events.onState_FinalStateEnter.RegisterListener(OnEnterFinalStateMethod);

        // SOHolder.Ins.importants.moneyInfo.moneyReactive.SubscribeToText(moneyText);
    }

    private void OnDestroy()
    {
        SOHolder.Ins.events.onState_FinalStateEnter.UnregisterListener(OnEnterFinalStateMethod);
    }

    public void PrintRemainingBandCount(int count)
    {
        count = SOHolder.Ins.importants.levelInfoManager.CurrLevelInfo.intersectedPairPointsForCalculation.Length - SOHolder.Ins.importants.intersectionInfoSo.IntersectionInfoList.Count;
        bandCountText.text = count.ToString();
    }

    public void OnEnterFinalStateMethod()
    {
        winPanel.gameObject.SetActive(true);
        accuracyImageSlider.DOFillAmount(SOHolder.Ins.variables.currAccuracyFV.value, 1).OnUpdate(() =>
        {
            int intAccuracy = Mathf.CeilToInt(accuracyImageSlider.fillAmount * 100f);
            accuracyText.text = "%" + intAccuracy;
        });
    }
}
