using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ReserveManager : MonoBehaviour
{
    public static ReserveManager Instance;

    
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private GameObject _powerPrefab;
    [SerializeField] private GameObject[] _reserves;

    
    private int _powerCount;


    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
            SpawnPowerToReserve(2);
    }

    public void SpawnPowerToReserve(int powerCount)
    {
        _powerCount += powerCount;
        StartCoroutine(TimingSpawnPower(powerCount));
    }

    IEnumerator TimingSpawnPower(int newPower)
    {
        for (int i = 0; i < newPower; i++)
        {
            GameObject go = Instantiate(_powerPrefab, transform);
            var power = go.GetComponent<Power>();
            
            power.InitAnim(1, _reserves[1].GetComponent<Reserve>());
            go.transform.DOPunchScale(Vector3.one, power.TimePunchScale);
            yield return new WaitForSeconds(power.TimePunchScale);
            power.JumpToReserve();
            yield return new WaitForSeconds(power.TimeAfterJump);
        }
        
        _upgradeManager.CheckUpgradeAvailable(_powerCount);
    }

    public void BuyUpgrade(int cost, int index)
    {
        _powerCount -= cost;

        _reserves[0].GetComponent<Reserve>().AddNewUnit(index);
        _reserves[0].GetComponent<Reserve>().RemovePower(cost);

        _upgradeManager.CheckUpgradeAvailable(_powerCount);
    }
}
