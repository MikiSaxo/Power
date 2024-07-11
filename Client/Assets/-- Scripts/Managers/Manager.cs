using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public List<Cell> AllCells => _allCells;
    public Cell[] CellHQ_BYRG => _cellHQ_BYRG;

    [Header("----- Bypass -----")]
    [SerializeField] private bool _bypassStart;
    [SerializeField] private GameObject _titleMenu;
    [SerializeField] private GameObject _colorMenu;
    
    [Header("----- Cells -----")]
    // Make _allCells private and add a public getter
    [SerializeField]
    private List<Cell> _allCells = new List<Cell>();

    [SerializeField] private Cell[] _cellHQ_BYRG;

    [Header("----- Player UI -----")] [SerializeField]
    private TMP_Text _textMyColor;

    [SerializeField] private Color[] _colorsText;

    [Header("----- Reserves -----")] [SerializeField]
    private GameObject[] _reserves;

    [SerializeField] private Color[] _colorsReserves;

    [Header("----- Colors -----")] public Color[] PawnColors;


    public ColorsID MyColorID { get; set; }
    public Troop CurrentTroopSelected { get; set; }
    public List<Troop> CurrentMultipleTroopsSelected = new List<Troop>();
    public Cell CurrentCellSelected { get; set; }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_bypassStart)
        {
            _titleMenu.SetActive(false);
            _colorMenu.SetActive(false);
            InitReserve();
            TroopsManager.Instance.InitStartsTroops();
        }
    }

    public void InitReserve()
    {
        ReserveManager.Instance.SetReserveColor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("j'ai appuyé sur o");
            //PlayerIOScript.Instance.Pioconnection.Send("MOVE", 1, 2 ,5 ,10);
        }
    }

    public void SetMyColor(int color)
    {
        MyColorID = (ColorsID)color;
        _textMyColor.text = $"You are {MyColorID}";
        _textMyColor.color = _colorsText[(int)MyColorID];
    }

    public void UpdateAllCells(bool state) // Call for Mega Missile
    {
        foreach (var cell in _allCells)
        {
            cell.ForceUpdateViewCell(state);
        }
    }

    public void UpdateTroopSelected(Troop newTroop)
    {
        ResetAllTroopsAndCells();
        CurrentTroopSelected = newTroop;
    }

    public void UpdateMultipleTroopsSelected(Troop troop)
    {
        if (CurrentTroopSelected != null)
        {
            AddTroopMultipleSelection(troop);
            AddTroopMultipleSelection(CurrentTroopSelected);

            CurrentTroopSelected = null;
            return;
        }

        if (CurrentMultipleTroopsSelected.Contains(troop))
        {
            RemoveTroopMultipleSelection(troop);
        }
        else
        {
            AddTroopMultipleSelection(troop);
        }
    }

    private void AddTroopMultipleSelection(Troop troop)
    {
        troop.IsSelected = true;
        CurrentMultipleTroopsSelected.Add(troop);
        
        CheckFusion();
    }

    private void RemoveTroopMultipleSelection(Troop troop)
    {
        troop.IsSelected = false;
        troop.OnPointerExit();
        CurrentMultipleTroopsSelected.Remove(troop);
        
        CheckFusion();
    }

    private void CheckFusion()
    {
        FusionManager.Instance.ResetFusionUpgrade();
        FusionManager.Instance.CheckFusionUpgradeAvailable(CurrentMultipleTroopsSelected);
    }


    public void CheckMovementMyTroop(Cell newCell)
    {
        if (CurrentTroopSelected != null)
        {
            ResetAllCells();
            CurrentTroopSelected.MoveToNewCell(newCell);
            TroopsManager.Instance.AddNewMyTroopMovement(CurrentTroopSelected.ID, newCell.name,
                CurrentTroopSelected.MyColorID);
            //PlayerIOScript.Instance.Pioconnection.Send("MOVE", CurrentTroopSelected.ID, newCell.name);
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
    
    public void ResetAllTroopsAndCells()
    {
        ResetAllCells();

        if (CurrentTroopSelected != null)
        {
            CurrentTroopSelected.IsSelected = false;
            CurrentTroopSelected.OnPointerExit();
            CurrentTroopSelected = null;
        }
        
        foreach (var troop in CurrentMultipleTroopsSelected)
        {
            troop.IsSelected = false;
            troop.OnPointerExit();
        }

        CurrentMultipleTroopsSelected.Clear();
        FusionManager.Instance.ResetFusionUpgrade();
    }
}