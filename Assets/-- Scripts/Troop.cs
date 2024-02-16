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
        _troopImg.color = _troopColors[colorIndex];
        CurrentCell = startCell;
        gameObject.transform.position = CurrentCell._startPoints[indexPosCell].position;

        _isAtStart = true;
    }

    private void Update()
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
        _lineConnector.LinkLineRenderer(CurrentCell.gameObject.transform.position,newCell.gameObject.transform.position);

        _isAtStart = false;
        _lastCell = CurrentCell;
        CurrentCell = newCell;
        
        var distance = Vector3.Distance(gameObject.transform.position, CurrentCell.gameObject.transform.position);
        int nbJump = (int)distance;
        if (nbJump < 1)
            nbJump = 1;
        
        gameObject.transform.DOJump(CurrentCell.gameObject.transform.position, distance* .1f, nbJump, .25f * nbJump).OnComplete(Manager.Instance.RecenterTroops);
        IsSelected = false;
        OnPointerExit();
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
