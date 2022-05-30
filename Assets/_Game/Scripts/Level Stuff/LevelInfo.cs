using UnityEngine;

[System.Serializable]
public class LevelInfo
{
    public string levelName;
    // [PreviewField(60), HorizontalGroup("A")]
    public Sprite requsetedModelSprite;
    // [PreviewField(60), HorizontalGroup("A")]
    public Texture2D sticker;
    public PairPoint[] intersectedPairPointsForCalculation;
}
