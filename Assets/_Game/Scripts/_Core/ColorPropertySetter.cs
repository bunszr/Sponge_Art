using UnityEngine;

public class ColorPropertySetter : MonoBehaviour
{
    public Color color;

    private void Awake()
    {
        OnValidate();
    }

    void OnValidate()
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        Renderer renderer = GetComponent<Renderer>();
        propertyBlock.SetColor("_BaseColor", color);
        renderer.SetPropertyBlock(propertyBlock);
    }
}