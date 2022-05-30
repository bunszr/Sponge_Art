using UnityEngine;

[CreateAssetMenu(fileName = "BendingModelInfoSo", menuName = "Sponge_Art/BendingModelInfoSo", order = 0)]
// [InlineEditor(Expanded = true)]
public class BendingModelInfoSo : ScriptableObject
{
    public float force = .85f;
    public float sqrDisFactor = 110f;
}