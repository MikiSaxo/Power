using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [SerializeField] private List<Cell> _allCells = new List<Cell>();

    public Troop CurrentTroopSelected { get; set; }
    public Cell CurrentCellSelected { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateAllCells(bool state)
    {
        foreach (var cell in _allCells)
        {
            cell.ForceUpdateViewCell(state);
        }
    }

    public void UpdateTroopSelected(Troop newTroop)
    {
        CurrentTroopSelected = newTroop;
    }

    public void CheckMovementTroop(Cell newCell)
    {
        if (CurrentTroopSelected != null)
        {
            ResetAllCells();
            CurrentTroopSelected.MoveToNewCell(newCell);
            CurrentTroopSelected = null;
        }
    }

    public void ResetAllCells()
    {
        foreach (var cell in _allCells)
        {
            cell.ResetCell();
        }
    }

    public void ResetAllSelected()
    {
        if(CurrentTroopSelected != null)
            CurrentTroopSelected.DeselectTroop();
            
        CurrentTroopSelected = null;
        CurrentCellSelected = null;
    }
}