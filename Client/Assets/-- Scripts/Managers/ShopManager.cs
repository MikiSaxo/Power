using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject _upgradePrefab;
    [SerializeField] private UpgradeInfos[] _upgradeInfos;

    private readonly List<Upgrade> _upgrades = new List<Upgrade>();

    private void Start()
    {
        for (int i = 0; i < _upgradeInfos.Length; i++)
        {
            GameObject go = Instantiate(_upgradePrefab, transform);
            var upgrade = go.GetComponent<Upgrade>();
            upgrade.Init(_upgradeInfos[i].Icon, _upgradeInfos[i].Value, _upgradeInfos[i].TroopType);
            _upgrades.Add(upgrade);
        }
    }

    public void CheckShopUpgradeAvailable(int nbPower)
    {
        for (int i = 0; i < _upgradeInfos.Length; i++)
        {
            _upgrades[i].UpdateBtn(nbPower >= _upgradeInfos[i].Value);
        }
    }
}
