using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;

[CreateAssetMenu(fileName = "MoneyInfo", menuName = "Unity Template/MoneyInfo", order = 0)]
public class MoneyInfo : ScriptableObject
{
    public static readonly string TOTAL_MONEY_KEY = "totalMoneyKey";

    public int defaultMoneyForBuild = 300;

    static Queue<Tween> moneyAnimQueue = new Queue<Tween>();

    // public ReactiveProperty<int> moneyReactive { get; private set; }
    static int? money = null;
    public static int Money
    {
        get
        {
            if (money == null)
            {
                money = PlayerPrefs.GetInt(TOTAL_MONEY_KEY);
                GameManager.Ins.onDestroy += () => PlayerPrefs.SetInt(TOTAL_MONEY_KEY, money.Value);
            }
            return money.Value;
        }
        set { money = value; }
    }

    public static void SetMoneyWithAnim(int newMoney)
    {
        moneyAnimQueue.Enqueue(MakeMoneyAnim(newMoney).Pause());
        MoneySquenceExecuter();
    }

    static Tween MakeMoneyAnim(int newMoney)
    {
        return DOTween.To(() => Money, x => Money = x, newMoney, .7f);
    }

    static void MoneySquenceExecuter()
    {
        if (moneyAnimQueue.Count == 0 || moneyAnimQueue.Peek().IsPlaying()) return;

        moneyAnimQueue.Peek().Play().OnComplete(() =>
        {
            moneyAnimQueue.Dequeue();
            MoneySquenceExecuter();
        });
    }
}