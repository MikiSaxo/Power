using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : MonoBehaviour
{
    [field: SerializeField] public Cell CurrentCell { get; set; }

    private void Start()
    {
        CurrentCell.UpdateAllNeighbor(true);
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
