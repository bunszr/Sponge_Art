using UnityEngine;
using DG.Tweening;

public class ElasticBand : ElasticBandAbs
{
    private void Start()
    {
        SOHolder.Ins.events.elasticBandDoneGameEvent.RegisterListener(ScaleAnim);
    }

    private void OnDestroy()
    {
        SOHolder.Ins.events.elasticBandDoneGameEvent.UnregisterListener(ScaleAnim);
    }

    public void SetScaleX(float x)
    {
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    void ScaleAnim()
    {
        if (gameObject.activeSelf)
        {
            transform.DOScale(transform.localScale * .9f, SOHolder.Ins.variables.deformeAllMeshAnimDurationFV.value / 2f).SetEase(Ease.Flash, 2);
        }
    }
}