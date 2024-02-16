using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Troop : MonoBehaviour
{
    [field: SerializeField] public Cell CurrentCell { get; set; }
    
    [SerializeField] private LineConnector _lineConnector;
    
    [Header("--- Troops ---")]
    [SerializeField] private TroopInfos _troopInfos;
    [SerializeField] private Image _troopImg;
    [SerializeField] private Color[] _troopColors;
    
    [Header("--- Highlight ---")]
    [SerializeField] private Image _imgHighlighted;
    [SerializeField] private Color[] _highlightedColors;
    
    [Header("--- Timings ---")]
    [SerializeField] private float _enter = .5f;
    [SerializeField] private float _exit = .5f;
    [SerializeField] private float _click = .5f;

    public bool IsSelected { get; set; }

    private Cell _lastCell;

    private void Start()
    {
       InitTroop(_troopInfos,0);
    }

    public void InitTroop(TroopInfos troopInfos, int colorIndex)
    {
        _troopImg.sprite = troopInfos.TroopSprite;
        _troopImg.color = _troopColors[colorIndex];
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
        // _lineConnector.ResetLine();
        // _lineConnector.AddBall(CurrentCell.gameObject.transform.position);
        // _lineConnector.AddBall(newCell.gameObject.transform.position);
        _lineConnector.LinkLineRenderer(CurrentCell.gameObject.transform.position,newCell.gameObject.transform.position);

        _lastCell = CurrentCell;
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
            _imgHighlighted.DOColor(_highlightedColors[2], _click);
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

        _imgHighlighted.DOColor(_highlightedColors[1], _enter);
    }

    public void OnPointerExit()
    {
        if (IsSelected) return;

        _imgHighlighted.DOColor(_highlightedColors[0], _exit);
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
