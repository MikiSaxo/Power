using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Troop : MonoBehaviour
{
    [SerializeField] private TroopInfos _troopInfos;
    [field: SerializeField] public Cell CurrentCell { get; set; }
    
    [SerializeField] private Image _imgHighlighted;
    [SerializeField] private Color[] _colors;
    
    [Header("--- Timings ---")]
    [SerializeField] private float _enter = .5f;
    [SerializeField] private float _exit = .5f;
    [SerializeField] private float _click = .5f;

    public bool IsSelected { get; set; }

    private void Start()
    {
       
    }

    private void SelectTroop()
    {
        if (_troopInfos.TroopsType == TroopsType.MegaMissile)
        {
            Manager.Instance.UpdateAllCells(true);
        }
        else
        {
            CurrentCell.IsBlocked = true;
            CurrentCell.UpdateAllNeighbor(true, _troopInfos.MovementRange, _troopInfos.CanCrossSea);
            Manager.Instance.UpdateTroopSelected(this);
        }
    }

    public void DeselectTroop()
    {
        Manager.Instance.ResetAllCells();
        Manager.Instance.UpdateTroopSelected(null);
        IsSelected = false;
        OnPointerExit();
    }

    public void MoveToNewCell(Cell newCell)
    {
        CurrentCell = newCell;
        gameObject.transform.DOJump(CurrentCell.gameObject.transform.position, 1, 1, 1f);
        IsSelected = false;
        OnPointerExit();
    }
    
    public void OnPointerClick()
    {
        if (!IsSelected)
        {
            IsSelected = true;
            _imgHighlighted.DOColor(_colors[2], _click);
            SelectTroop();
        }
        else
        {
            IsSelected = false;
            OnPointerExit();
            DeselectTroop();
        }
    }

    public void OnPointerEnter()
    {
        if (IsSelected) return;

        _imgHighlighted.DOColor(_colors[1], _enter);
    }

    public void OnPointerExit()
    {
        if (IsSelected) return;

        _imgHighlighted.DOColor(_colors[0], _exit);
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
