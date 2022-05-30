using UnityEngine;
using AdditiveSceneHelper;

public class FirstLoadingScene : MonoBehaviour
{
    const string firstInitKey = "firstInitKey";

    private void Awake()
    {
        if (PlayerPrefs.GetInt(firstInitKey) == 0)
        {
            PlayerPrefs.SetInt(MoneyInfo.TOTAL_MONEY_KEY, SOHolder.Ins.importants.moneyInfo.defaultMoneyForBuild);
            PlayerPrefs.SetInt(firstInitKey, 1);
        }
        SOHolder.Ins.importants.levelManager.LoadSceneAccordingInfo();
    }
}