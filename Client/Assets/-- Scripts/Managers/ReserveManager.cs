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
        // if(Input.GetKeyDown(KeyCode.U))
        //     SpawnPowerToReserve(2);
    }

    public void AddAllPower(List<Troop> troops)
    {
        StartCoroutine(AddAllPowerTiming(troops));
    }

    IEnumerator AddAllPowerTiming(List<Troop> troops)
    {
        for (int i = 0; i < 4; i++)
        {
            var countPower = 0;
            foreach (var troop in troops)
            {
                if (troop.ID == i && troop.IsMyCellEnemyColor())
                    countPower++;
            }
            
            if(countPower != 0)
                SpawnPowerToReserve(countPower, i);
            
            yield return new WaitForSeconds(.5f);
        }
    }

    public void SpawnPowerToReserve(int powerCount, int indexColor)
    {
        _powerCount += powerCount;
        StartCoroutine(TimingSpawnPower(powerCount, indexColor));
    }

    IEnumerator TimingSpawnPower(int newPower, int indexColor)
    {
        for (int i = 0; i < newPower; i++)
        {
            GameObject go = Instantiate(_powerPrefab, transform);
            var power = go.GetComponent<Power>();

            power.InitAnim(indexColor, _reserves[indexColor].GetComponent<Reserve>());
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