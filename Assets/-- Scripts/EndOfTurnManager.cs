using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EndOfTurnManager : MonoBehaviour
{
    public static EndOfTurnManager Instance;


    private void Awake()
    {
        Instance = this;
    }

    public void OnEndOfTurnButton()
    {
        OrdersManager.Instance.ResetOrders();

        int count = 0;
        foreach (var troop in Manager.Instance.AllTroop)
        {
            troop.ResetLineConnector();

            if (troop.IsMyCellEnemyColor())
                count++;
        }
        
        ReserveManager.Instance.SpawnPowerToReserve(count);
    }

    
}
