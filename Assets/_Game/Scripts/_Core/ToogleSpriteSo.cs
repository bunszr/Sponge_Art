using UnityEngine;

[CreateAssetMenu(fileName = "ToogleSpriteSo", menuName = "Unity Template/ToogleSpriteSo", order = 0)]
public class ToogleSpriteSo : ScriptableObject
{
    // [InfoBox("0 = Passive , 1 = Active ", InfoMessageType.Warning)]
    [Header("0 = Off, 1 = On")]
    public Sprite[] sprites;

    public string prefsKey;

    public int Index
    {
        get => PlayerPrefs.GetInt(prefsKey);
        set => PlayerPrefs.SetInt(prefsKey, value);
    }

    public Sprite CurrSprite => sprites[Index];
}