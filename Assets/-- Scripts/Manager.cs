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

    [SerializeField] private GameObject _troopsParent;
    [SerializeField] private GameObject _troopPrefab;
    [SerializeField] private TroopInfos[] _troopAllInfos;
    [SerializeField] private int[] _troopStartInfos;
    [SerializeField] private Cell[] _cellHQ_BYRG;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var index in _troopStartInfos)
        {
            GameObject go = Instantiate(_troopPrefab, _troopsParent.transform);
            go.GetComponent<Troop>().InitTroop(_troopAllInfos[index], 0, _cellHQ_BYRG[0]);
        }
    }


    // Call for Mega Missile
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