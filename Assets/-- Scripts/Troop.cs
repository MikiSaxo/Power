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
    [SerializeField] private Colors _myColor;

    [Header("--- Highlight ---")] 
    [SerializeField] private Image _imgHighlighted;
    [SerializeField] private Color[] _highlightedColors;

    [Header("--- Timings ---")] 
    [SerializeField] private float _enter = .5f;
    [SerializeField] private float _exit = .5f;
    [SerializeField] private float _click = .5f;

    public float _cellDistance;
    public float _cellDistanceMax = .4f;

    public bool IsSelected { get; set; }

    private Cell _lastCell;
    private bool _isAtStart;

    private void Start()
    {
    }

    public void InitTroop(TroopInfos troopInfos, int colorIndex, Cell startCell, int indexPosCell)
    {
        // print(troopInfos.name);
        _troopInfos = troopInfos;
        _troopImg.sprite = troopInfos.TroopSprite;
        _troopImg.color = Manager.Instance.PawnColors[colorIndex];
        CurrentCell = startCell;
        gameObject.transform.position = CurrentCell._startPoints[indexPosCell].position;
        _myColor = (Colors)colorIndex;
        
        _troopImg.SetNativeSize();

        if (_myColor == Colors.Red || _myColor == Colors.Green)
            _troopImg.transform.localScale = new Vector3(-1, 1, 1);

        _isAtStart = true;
    }
    

    private void SelectTroop()
    {
        if (_troopInfos.TroopsType == TroopsType.MegaMissile)
        {
            Manager.Instance.UpdateAllCells(true);
        }
        else
        {
            Manager.Instance.UpdateTroopSelected(this);
            CurrentCell.IsBlocked = true;
            CurrentCell.UpdateAllNeighbor(true, _troopInfos.MovementRange, _troopInfos.CanCrossSea);
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
        
        _lineConnector.LinkLineRenderer(CurrentCell.gameObject.transform.position,
            newCell.gameObject.transform.position);

        _isAtStart = false;
        _lastCell = CurrentCell;
        CurrentCell = newCell;

        var distance = Vector3.Distance(gameObject.transform.position, CurrentCell.gameObject.transform.position);
        int nbJump = (int)distance;
        if (nbJump < 1)
            nbJump = 1;

        gameObject.transform.DOJump(CurrentCell.gameObject.transform.position, distance * .1f, nbJump, .25f * nbJump)
            .OnComplete(Manager.Instance.RecenterTroops);
        IsSelected = false;
        OnPointerExit();
        
        OrdersManager.Instance.UpdateOrdersLeft(true);
    }

    public void ArrivedToNewCell()
    {
        if (_isAtStart) return;

        _cellDistance = Vector3.Distance(gameObject.transform.position, CurrentCell.gameObject.transform.position);

        if (_cellDistance > _cellDistanceMax)
        {
            Vector2 force = CurrentCell.gameObject.transform.position - gameObject.transform.position;
            gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            // _lineConnector.LinkLineRenderer(_lastCell.gameObject.transform.position, gameObject.transform.position);
        }
    }

    private void GoToOldCell()
    {
        ResetLineConnector();
        
        CurrentCell = _lastCell;
        _lastCell = null;
        gameObject.transform.DOKill();
        gameObject.transform.DOMove(CurrentCell.gameObject.transform.position, 1f);

        IsSelected = false;
        OnPointerExit();

        OrdersManager.Instance.UpdateOrdersLeft(false);
    }

    public void OnPointerClick()
    {
        if (Manager.Instance.MyColor != _myColor) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!OrdersManager.Instance.CanAddNewOrder()) return;
            
            PointerLeftClick();
        }
        else
        {
            PointerRightClick();
        }
    }

    private void PointerLeftClick()
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

    private void PointerRightClick()
    {
        if (_lastCell == null) return;
        
        GoToOldCell();
    }

    public void OnPointerEnter()
    {
        if (IsSelected) return;
        if (Manager.Instance.MyColor != _myColor) return;

        _imgHighlighted.DOColor(_highlightedColors[1], _enter);
    }

    public void OnPointerExit()
    {
        if (IsSelected) return;
        if (Manager.Instance.MyColor != _myColor) return;

        _imgHighlighted.DOColor(_highlightedColors[0], _exit);
    }

    public bool IsMyCellEnemyColor()
    {
        if (CurrentCell.CellColor == Colors.Neutral)
            return false;
        
        return CurrentCell.CellColor != _myColor;
    }
    
    public void ResetLineConnector()
    {
        _lineConnector.ResetLine();
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