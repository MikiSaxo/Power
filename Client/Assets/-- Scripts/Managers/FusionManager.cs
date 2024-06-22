using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionManager : MonoBehaviour
{
    public static FusionManager Instance;

    [SerializeField] private GameObject _fusionPrefab;
    [SerializeField] private UpgradeInfos[] _fusionInfos;

    private readonly List<Upgrade> _fusions = new List<Upgrade>();

    public Troop CurrentTroopFusion { get; set; }
    
    private List<Troop> _troopsSaved = new List<Troop>();

    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < _fusionInfos.Length; i++)
        {
            GameObject go = Instantiate(_fusionPrefab, transform);
            var upgrade = go.GetComponent<Upgrade>();
            upgrade.Init(_fusionInfos[i].Icon, _fusionInfos[i].Value, _fusionInfos[i].TroopType);
            _fusions.Add(upgrade);
        }
    }

    public void CheckFusionUpgradeAvailable(List<Troop> troops)
    {
        if (troops.Count == 2)
        {
            var firstTroopType = troops[0].TroopType;
            if (troops.TrueForAll(troop => troop.TroopType == firstTroopType))
            {
                if (firstTroopType == TroopsType.Soldier || firstTroopType == TroopsType.Tank || firstTroopType == TroopsType.Fighter || firstTroopType == TroopsType.Destroyer)
                {
                    int index = (int)firstTroopType;
                    if (index < _fusions.Count)
                    {
                        _troopsSaved = troops;
                        CurrentTroopFusion = troops[0];
                        _fusions[index].UpdateBtn(true);
                    }
                }
            }
        }
    }
    
    public void ResetFusionUpgrade()
    {
        foreach (var fusion in _fusions)
        {
            fusion.UpdateBtn(false);
        }
        CurrentTroopFusion = null;
        _troopsSaved.Clear();
    }

    public void FusionHasBeenDone()
    {
        foreach (var troop in _troopsSaved)
        {
            TroopsManager.Instance.RemoveTroopList(troop);
            Destroy(troop.gameObject);
        }
        
        ResetFusionUpgrade();
    }
}
