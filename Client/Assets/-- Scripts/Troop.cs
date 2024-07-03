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

    [Header("--- Troops ---")] [SerializeField]
    private TroopInfos _troopInfos;

    [SerializeField] private Image _troopImg;

    [Header("--- Highlight ---")] [SerializeField]
    private Image _imgHighlighted;

    [SerializeField] private Color[] _highlightedColors;

    [Header("--- Timings ---")] [SerializeField]
    private float _enter = .5f;

    [SerializeField] private float _exit = .5f;
    [SerializeField] private float _click = .5f;

    public float _cellDistance;
    public float _cellDistanceMax = .4f;

    public bool IsSelected { get; set; }
    public bool HasMoved { get; set; }
    public int ID { get; set; }
    public ColorsID MyColorID { get; private set; }

    public TroopsType TroopType { get; private set; }

    private Cell _lastCell;
    private bool _isAtStart;

    public void InitTroop(TroopInfos troopInfos, int colorIndex, Cell startCell, int id)//, int indexPosCell = 0)
    {
        // print(troopInfos.name);
        _troopInfos = troopInfos;
        _troopImg.sprite = troopInfos.TroopSprite;
        _troopImg.color = Manager.Instance.PawnColors[colorIndex];
        ID = id;
        TroopType = troopInfos.TroopsType;

        CurrentCell = startCell;
        
        // Here for specific location at start
        // if(isStart)
        //     gameObject.transform.position = CurrentCell.StartPointsHQ[indexPosCell].position;
        // else
        //     gameObject.transform.position = CurrentCell.transform.position;
        
        MyColorID = (ColorsID)colorIndex;

        _troopImg.SetNativeSize();

        if (MyColorID == ColorsID.Red || MyColorID == ColorsID.Green)
            _troopImg.transform.localScale = new Vector3(-1, 1, 1);

        _isAtStart = true;
        
        BounceTroop();
    }

    // public void InitTroopReserve(TroopInfos troopInfos, int colorIndex, Cell startCell, Transform reservePos)
    // {
    //     InitTroop(troopInfos, colorIndex, startCell, TroopsManager.Instance.TroopCountID);
    //
    //     gameObject.transform.position = reservePos.position;
    // }

    private void BounceTroop()
    {
        _troopImg.gameObject.transform.DOPunchScale(new Vector3(.3f, .3f, .3f), 1f, 5);
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

            CurrentCell.UpdateAllNeighbor(true, !CurrentCell.IsCellReserve ? _troopInfos.MovementRange : 1,
                _troopInfos.CanCrossSea);
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

        if (HasMoved) return;

        HasMoved = true;
        
        _isAtStart = false;
        _lastCell = CurrentCell;
        CurrentCell = newCell;
        
        StartCoroutine(DrawLineConnector());

        var distance = Vector3.Distance(gameObject.transform.position, CurrentCell.gameObject.transform.position);
        int nbJump = (int)distance;
        if (nbJump < 1)
            nbJump = 1;

        gameObject.transform.DOJump(CurrentCell.gameObject.transform.position, distance * .1f, nbJump, .35f * nbJump)
            .OnComplete(TroopsManager.Instance.RecenterTroops);
        IsSelected = false;
        OnPointerExit();

        if (MyColorID == Manager.Instance.MyColorID)
            OrdersManager.Instance.UpdateOrdersLeft(true);
    }

    private IEnumerator DrawLineConnector()
    {
        float startTime = Time.time;
        while (Time.time - startTime < 5)
        {
            if(_lastCell != null)
                _lineConnector.LinkLineRenderer(_lastCell.gameObject.transform.position, gameObject.transform.position);
            else
            {
                ResetLineConnector();
                yield break;
            }
            yield return null; // Attendre jusqu'à la prochaine frame
        }
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
        if (Manager.Instance.MyColorID != MyColorID) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!OrdersManager.Instance.CanAddNewOrder()) return;
            if (HasMoved) return;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                // Si une autre troupe est déjà sélectionnée
                // if (Manager.Instance.CurrentTroopSelected != null && Manager.Instance.CurrentTroopSelected != this)
                // {
                PointerLeftShiftClick();
                Manager.Instance.UpdateAllCells(false);
                // }
                // else
                // {
                // PointerLeftClick();
                // }
            }
            else
            {
                PointerLeftClick();
            }
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

    private void PointerLeftShiftClick()
    {
        _imgHighlighted.DOColor(_highlightedColors[2], _click);

        if (_troopInfos.TroopsType != TroopsType.MegaMissile)
        {
            Manager.Instance.UpdateMultipleTroopsSelected(this);
        }
    }

    private void PointerRightClick()
    {
        if (_lastCell == null) return;

        HasMoved = false;

        GoToOldCell();
    }

    public void OnPointerEnter()
    {
        if (IsSelected) return;
        if (Manager.Instance.MyColorID != MyColorID) return;

        // if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        // {
        //     if (Manager.Instance.CanAddMultipleTroops() != null)
        //     {
        //         if (Manager.Instance.CanAddMultipleTroops().TroopType != TroopType)
        //             return;
        //     }
        // }

        _imgHighlighted.DOColor(_highlightedColors[1], _enter);
    }

    public void OnPointerExit()
    {
        if (IsSelected) return;
        if (Manager.Instance.MyColorID != MyColorID) return;

        _imgHighlighted.DOColor(_highlightedColors[0], _exit);
    }

    public bool IsCellEnemyColor()
    {
        if (CurrentCell.CellColorID == ColorsID.Neutral)
            return false;

        return CurrentCell.CellColorID != MyColorID;
    }

    public void ResetLineConnector()
    {
        StopCoroutine("DrawLineConnector");
        _lineConnector.ResetLine();
    }
}

public enum TroopsType
{
    Soldier = 0,
    Tank = 1,
    Fighter = 2,
    Destroyer = 3,
    Regiment = 4,
    BigTank = 5,
    Bomber = 6,
    Cruiser = 7,
    MegaMissile = 8
}