using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionManager : MonoBehaviour
{
    [SerializeField] private GameObject _fusionPrefab;
    [SerializeField] private UpgradeInfos[] _fusionInfos;

    private readonly List<Upgrade> _fusions = new List<Upgrade>();

    private void Start()
    {
        for (int i = 0; i < _fusionInfos.Length; i++)
        {
            GameObject go = Instantiate(_fusionPrefab, transform);
            var upgrade = go.GetComponent<Upgrade>();
            upgrade.Init(_fusionInfos[i].Icon, _fusionInfos[i].Value, i);
            _fusions.Add(upgrade);
        }
    }
}
