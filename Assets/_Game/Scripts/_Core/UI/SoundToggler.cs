using UnityEngine;
using UnityEngine.UI;

public class SoundToggler : MonoBehaviour
{
    [SerializeField] ToogleSpriteSo soundToogleSpriteSO;

    private void Start()
    {
        Image image = GetComponent<Image>();
        AudioListener.pause = soundToogleSpriteSO.Index == 0;
        image.sprite = soundToogleSpriteSO.CurrSprite;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            soundToogleSpriteSO.Index = 1 - soundToogleSpriteSO.Index;
            AudioListener.pause = soundToogleSpriteSO.Index == 0;
            image.sprite = soundToogleSpriteSO.CurrSprite;
        });
    }
}