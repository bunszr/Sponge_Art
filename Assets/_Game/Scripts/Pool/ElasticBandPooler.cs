using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Toolkit;

public class ElasticBandPooler : MonoBehaviour
{
    [SerializeField] ElasticBand elasticBandPrefab;
    public ElasticBandPool elasticBandPool;
    void Start()
    {
        elasticBandPool = new ElasticBandPool(elasticBandPrefab, transform);
        for (int i = 0; i < transform.childCount; i++) elasticBandPool.Return(transform.GetChild(i).GetComponent<ElasticBand>());
    }

    public class ElasticBandPool : ObjectPool<ElasticBand>
    {
        readonly ElasticBand prefab;
        readonly Transform hierarchyParent;

        public ElasticBandPool(ElasticBand prefab, Transform hierarchyParent)
        {
            this.prefab = prefab;
            this.hierarchyParent = hierarchyParent;
        }

        protected override ElasticBand CreateInstance()
        {
            ElasticBand elasticBand = GameObject.Instantiate<ElasticBand>(prefab);
            elasticBand.transform.SetParent(hierarchyParent);
            return elasticBand;
        }
    }
}