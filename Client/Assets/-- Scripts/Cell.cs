using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [field: SerializeField] public int ID { get; set; }
    [field: SerializeField] public ColorsID CellColorID { get; set; }
    [SerializeField] private bool _isSea;
    [SerializeField] private bool _isFullLand;
    [field: SerializeField] public List<Cell> Neighbor { get; set; } = new List<Cell>();

    [Header("---- Prefab")]
    [SerializeField] private Image _img;
    [SerializeField] private Color[] _colors;


    [Header("--- Timings ---")] 
    [SerializeField] private float _enter = .5f;
    [SerializeField] private float _exit = .5f;
    // [SerializeField] private float _click = .5f;
    
    // [Header("--- Start Point HQ ---")] 
    [field: SerializeField] public Transform[] StartPointsHQ { get; set; }

    public bool IsBlocked { get; set; }
    public bool IsCellReserve => _isReserve;

    private bool _isActive;
    private bool _isReserve;
    
    public void InitCellReserve(Cell cell, ColorsID colorsID)
    {
        Neighbor.Add(cell);
        CellColorID = colorsID;
        _isReserve = true;
    }

    public void UpdateViewCell(bool state, bool canCrossSea)
    {
        if (canCrossSea == false && _isSea == true)
        {
            return;
        }

        if (canCrossSea == true && _isFullLand == true)
        {
            return;
        }

        if (IsBlocked)
        {
            return;
        }

        ForceUpdateViewCell(state);
    }

    public void ForceUpdateViewCell(bool state)
    {
        if(_isReserve) return;
        
        if (state)
        {
            _img.DOKill();
            _img.DOFade(1, 0.5f);
        }
        else
        {
            _img.DOKill();
            _img.DOFade(0, 0.1f);
        }

        _isActive = state;
    }

    public void UpdateAllNeighbor(bool state, int time, bool canCrossSea)
    {
        if (canCrossSea == false && _isSea == true)
        {
            return;
        }

        if (canCrossSea == true && _isFullLand == true)
        {
            return;
        }
        
        if (!IsBlocked)
            UpdateViewCell(state, canCrossSea);

        time--;
        if (time > 0)
        {
            foreach (var cell in Neighbor)
            {
                cell.UpdateAllNeighbor(state, time, canCrossSea);
            }
        }
        else
        {
            foreach (var cell in Neighbor)
            {
                cell.UpdateViewCell(state, canCrossSea);
            }
        }
    }

    public void OnPointerClick()
    {
        if (!_isActive)
        {
            Manager.Instance.ResetAllTroopsAndCells();
            return;
        }

        Manager.Instance.CheckMovementMyTroop(this);
    }

    public void OnPointerEnter()
    {
        if (!_isActive) return;

        _img.DOKill();
        _img.DOColor(_colors[1], _enter);
    }

    public void OnPointerExit()
    {
        if (!_isActive || IsBlocked) return;

        _img.DOKill();
        _img.DOColor(_colors[0], _exit);
    }

    public void ResetCell()
    {
        IsBlocked = false;
        _isActive = false;

        _img.DOKill();
        _img.DOColor(_colors[3], _exit);
    }

    // private void OnDrawGizmos()
    // {
    //     foreach (var cell in Neighbor)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawLine(gameObject.transform.position, cell.gameObject.transform.position);
    //     }
    // }
}

public enum ColorsID
{
    Neutral = -1,
    Blue = 0,
    Yellow = 1,
    Red = 2,
    Green = 3,
}