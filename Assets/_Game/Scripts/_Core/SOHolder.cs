using UnityEngine;
using AdditiveSceneHelper;

[CreateAssetMenu(fileName = "GameEventHolder", menuName = "Sponge_Art/GameEventHolder", order = 0)]
public class SOHolder : SingletonScriptableObject<SOHolder>
{
    public GameEventHolder events;
    public ImportantHolder importants;
    public VariableHolder variables;

    [System.Serializable]
    public class GameEventHolder
    {
        public GameEvent onPressedNextLevelButton;
        public GameEvent onPressedRestartLevelButton;
        public GameEvent onClickAndLevelStart;

        public GameEvent elasticBandDoneGameEvent;
        public GameEvent elasticBandFailGameEvent;

        public GameEvent onReleaseAllBandStartGameEvent;
        public GameEvent onReleaseAllBandDoneGameEvent;

        public GameEvent onCutBand;

        public GameEvent onMakeSticker;

        public GameEvent onState_BandControllerEnter;
        public GameEvent onState_FinalStateEnter;
    }

    [System.Serializable]
    public class ImportantHolder
    {
        public InputData inputData;
        public IntersectionInfoSo intersectionInfoSo;
        public LevelInfoManager levelInfoManager;
        public LevelManager levelManager;
        public MoneyInfo moneyInfo;
    }

    [System.Serializable]
    public class VariableHolder
    {
        public FloatVariable minAccuracyFV;
        public FloatVariable currAccuracyFV;
    }
}