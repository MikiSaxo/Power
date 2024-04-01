using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EndOfTurnManager : MonoBehaviour
{
    public static EndOfTurnManager Instance;

    [SerializeField] private GameObject _powerPrefab;
    
    private int _powerCount;
    private GameObject[] _reserves;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _reserves = Manager.Instance.GetReserves();
    }

    public void OnEndOfTurnButton()
    {
        OrdersManager.Instance.ResetOrders();

        foreach (var troop in Manager.Instance.AllTroop)
        {
            troop.ResetLineConnector();

            if (troop.IsMyCellEnemyColor())
                _powerCount++;
        }
        
        SpawnPowerToReserve();
    }

    private void SpawnPowerToReserve()
    {
        StartCoroutine(TimingSpawnPower());
    }

    IEnumerator TimingSpawnPower()
    {
        for (int i = 0; i < _powerCount; i++)
        {
            GameObject go = Instantiate(_powerPrefab, transform);
            var power = go.GetComponent<Power>();
            
            power.InitAnim(0, _reserves[0].GetComponent<Reserve>());
            go.transform.DOPunchScale(Vector3.one, power.TimePunchScale);
            yield return new WaitForSeconds(power.TimePunchScale);
            power.JumpToReserve();
            yield return new WaitForSeconds(power.TimeAfterJump);
        }
        
        _powerCount = 0;
    }
}
