using UnityEngine;

public class ElasticBand : ElasticBandAbs
{
    public void SetScaleX(float x)
    {
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}