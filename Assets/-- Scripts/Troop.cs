using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : MonoBehaviour
{
    [SerializeField] private TroopInfos _troopInfos;
    [field: SerializeField] public Cell CurrentCell { get; set; }

    private void Start()
    {
        if(_troopInfos.TroopsType == TroopsType.MegaMissile)
            Manager.Instance.UpdateAllCells(true);
        else
            CurrentCell.UpdateAllNeighbor(true, _troopInfos.MovementRange, _troopInfos.CanCrossSea);
    }
}

public enum TroopsType
{
    Soldier = 0,
    Regiment = 1,
    Tank = 2,
    BigTank = 3,
    Fighter = 4,
    Bomber = 5,
    Destroyer = 6,
    Cruiser = 7,
    MegaMissile = 8
}
