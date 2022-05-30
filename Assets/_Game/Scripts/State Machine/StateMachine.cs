using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public IState CurrState { get; private set; }

    [SerializeField] UnityEngine.Object firstState;
    [SerializeField] bool logCurrState = false;

    private void Awake()
    {
        ChangeState(((GameObject)firstState).GetComponent<IState>());
    }

    public void ChangeState(IState _state)
    {
        if (CurrState != null)
        {
            CurrState.Exit();
        }
        CurrState = _state;
        PrintCurrState();
        CurrState.Enter();

    }

    public void ChangeState(State state)
    {
        ChangeState(GetStateWithEnum(state));
    }

    IState GetStateWithEnum(State state)
    {
        switch (state)
        {
            // case State.AccuracyCalculator: return FindObjectOfType<AccuracyCalculator>();
            case State.DecisionMechanism_A: return new StateDecisionMechanism(new DecisionMechanism_A());
            case State.BandReleaser: return FindObjectOfType<BandReleaser>();
            case State.StickerState: return FindObjectOfType<StickerState>();
            case State.BandController: return FindObjectOfType<BandController>();
            case State.FinalState: return FindObjectOfType<FinalState>();
        }
        return null;
    }

    private void PrintCurrState()
    {
        if (logCurrState) Debug.Log("Enter: " + CurrState.GetType().Name);
    }
}

public enum State { DecisionMechanism_A, BandReleaser, StickerState, BandController, FinalState }