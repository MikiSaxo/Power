using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Cell : MonoBehaviour
{
    [field: SerializeField] public Colors CellColor { get; set; }
    [SerializeField] private bool _isSea;
    [SerializeField] private bool _isFullLand;
    [field: SerializeField] public List<Cell> Neighbor { get; set; } = new List<Cell>();

    [Header("---- Prefab")] 
    [SerializeField] private GameObject _img;



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
        _img.SetActive(state);
    }

    public void UpdateAllNeighbor(bool state, int time, bool canCrossSea)
    {
        if (canCrossSea == false && _isSea == true)
            return;
        if (canCrossSea == true && _isFullLand == true)
            return;
        
        print($"{name} : {canCrossSea} / {_isSea}");
        
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