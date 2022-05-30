using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] bool isDebugSDK = true;

    public event System.Action onCustomUpdate;
    public event System.Action onDestroy; // For scriptable object

    public GameObject selectGameObjectOnStart;

    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public MeshFilter bendingModelFilter;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = FindObjectOfType<StateMachine>();
    }

    private void Start()
    {
        bendingModelFilter = FindObjectOfType<BendingModel>().GetComponent<MeshFilter>();

        SDKLevelStart();
        SOHolder.Ins.events.onPressedNextLevelButton.RegisterListener(SDKLevelCompleted);
        SOHolder.Ins.events.onPressedRestartLevelButton.RegisterListener(SDKLevelFailed);

        onDestroy += () => SOHolder.Ins.events.onPressedNextLevelButton.UnregisterListener(SDKLevelCompleted);
        onDestroy += () => SOHolder.Ins.events.onPressedRestartLevelButton.UnregisterListener(SDKLevelFailed);

        onCustomUpdate += TapToPlay;

#if UNITY_EDITOR
        if (selectGameObjectOnStart != null) UnityEditor.Selection.activeGameObject = selectGameObjectOnStart;
#endif
    }

    private void Update()
    {
        onCustomUpdate?.Invoke();
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }

    public void TapToPlay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SOHolder.Ins.events.onClickAndLevelStart.Raise();
            onCustomUpdate -= TapToPlay;
        }
    }

    void SDKLevelStart()
    {
        DebugSDK("Level Start = " + SOHolder.Ins.importants.levelManager.LevelSceneInfo.LevelIndex);
    }

    void SDKLevelCompleted()
    {
        DebugSDK("Win = " + SOHolder.Ins.importants.levelManager.LevelSceneInfo.LevelIndex);
    }

    void SDKLevelFailed()
    {
        DebugSDK("Fail = " + SOHolder.Ins.importants.levelManager.LevelSceneInfo.LevelIndex);
    }

    void DebugSDK(string message)
    {
        if (isDebugSDK) Debug.Log(message);
    }
}