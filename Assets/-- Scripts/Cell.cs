using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [field: SerializeField] public Colors CellColor { get; set; }
    [SerializeField] private bool _isSea;
    [SerializeField] private bool _isFullLand;
    [field: SerializeField] public List<Cell> Neighbor { get; set; } = new List<Cell>();

    [Header("---- Prefab")] [SerializeField] private Image _img;

    [SerializeField] private Color[] _colors;

    [Header("--- Timings ---")]
    [SerializeField] private float _enter = .5f;
    [SerializeField] private float _exit = .5f;
    [SerializeField] private float _click = .5f;
    
    public bool IsSelected { get; set; }

    private bool _isActive;
    private bool _isTreated;
    

    public void UpdateViewCell(bool state, bool canCrossSea)
    {
        if (canCrossSea == false && _isSea == true)
            return;
        if (canCrossSea == true && _isFullLand == true)
            return;

        ForceUpdateViewCell(state);
    }

    public void ForceUpdateViewCell(bool state)
    {
        StartCoroutine(WaitForceUpdateViewCell(state));
    }

    IEnumerator WaitForceUpdateViewCell(bool state)
    {
        yield return new WaitForSeconds(.1f);
        
        if (state)
        {
            _img.DOFade(1, 0.5f);
        }
        else
        {
            _img.DOFade(0, 0.1f);
        }

        _isActive = state;
    }

    public void UpdateAllNeighbor(bool state, int time, bool canCrossSea, bool stateMe)
    {
        if (canCrossSea == false && _isSea == true)
            return;
        if (canCrossSea == true && _isFullLand == true)
            return;

        if (!_isTreated)
        {
            UpdateViewCell(stateMe, canCrossSea);
            _isTreated = true;
        }

        time--;
        if (time > 0)
        {
            foreach (var cell in Neighbor)
            {
                cell.UpdateAllNeighbor(state, time, canCrossSea, state);
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
            //Manager.Instance.ResetAllSelected();
            return;
        }

        Manager.Instance.CheckMovementTroop(this);

        // if (!IsSelected)
        // {
        //     IsSelected = true;
        //     _img.DOColor(_colors[2], _click);
        // }
        // else
        // {
        //     IsSelected = false;
        //     OnPointerExit();
        // }
    }

    public void OnPointerEnter()
    {
        if (!_isActive || IsSelected) return;

        _img.DOColor(_colors[1], _enter);
    }

    public void OnPointerExit()
    {
        if (!_isActive || IsSelected) return;

        _img.DOColor(_colors[0], _exit);
    }

    private void OnDrawGizmos()
    {
        foreach (var cell in Neighbor)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(gameObject.transform.position, cell.gameObject.transform.position);
        }
    }
}

public enum Colors
{
    Neutral = -1,
    Blue = 0,
    Yellow = 1,
    Red = 2,
    Green = 3,
}