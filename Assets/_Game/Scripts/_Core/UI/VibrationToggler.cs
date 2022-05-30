using UnityEngine;
using UnityEngine.UI;

public class VibrationToggler : MonoBehaviour
{
    [SerializeField] ToogleSpriteSo vibrationToogleSpriteSO;

    private void Start()
    {
        Image image = GetComponent<Image>();
        // MMVibrationManager.SetHapticsActive(vibrationToogleSpriteSO.Index == 1);
        image.sprite = vibrationToogleSpriteSO.CurrSprite;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            vibrationToogleSpriteSO.Index = 1 - vibrationToogleSpriteSO.Index;
            // MMVibrationManager.SetHapticsActive(vibrationToogleSpriteSO.Index == 1);
            image.sprite = vibrationToogleSpriteSO.CurrSprite;
        });
    }
}