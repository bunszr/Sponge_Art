using UnityEngine;
using UniRx;

[CreateAssetMenu(fileName = "InputData", menuName = "Sponge_Art/InputData", order = 0)]
public class InputData : ScriptableObject
{
    public Vector3 position;
    public Vector3 pressedPosition;

    [System.NonSerialized] public ReactiveProperty<bool> HasDrag = new ReactiveProperty<bool>();

    public Vector3 Direction => (position - pressedPosition).normalized;
    public Vector3 DirectionVector => (position - pressedPosition);

    public void Init()
    {
        HasDrag = new ReactiveProperty<bool>();
    }
}