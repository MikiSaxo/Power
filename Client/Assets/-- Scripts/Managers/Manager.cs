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
    
    [Header("----- Cells -----")]
    // Make _allCells private and add a public getter
    [SerializeField] private List<Cell> _allCells = new List<Cell>();
    [SerializeField] private Cell[] _cellHQ_BYRG;
    
    [Header("----- Player UI -----")]
    [SerializeField] private TMP_Text _textMyColor;
    [SerializeField] private Color[] _colorsText;

    [Header("----- Reserves -----")]
    [SerializeField] private GameObject[] _reserves;
    [SerializeField] private Color[] _colorsReserves;

    [Header("----- Colors -----")] 
    public Color[] PawnColors;
    

    public ColorsID MyColorID { get; set; }
    public Troop CurrentTroopSelected { get; set; }
    public Cell CurrentCellSelected { get; set; }

    
    private void Awake()
    {
        Instance = this;
    }

    public void InitGame()
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

    private void SetReserve()
    {
        // Prévoir un script réserve ou une list/dico pour savoir quelle réserve appartient à quelle couleur
        _reserves[0].GetComponentInChildren<Image>().color = _colorsReserves[(int)MyColorID];
        
        List<ColorsID> availableColors = new List<ColorsID> { ColorsID.Blue, ColorsID.Yellow, ColorsID.Red, ColorsID.Green };
        availableColors.Remove(MyColorID);

        for (int i = 1; i < _reserves.Length; i++)
        {
            var colorIndex = i - 1;
            _reserves[i].GetComponentInChildren<Image>().color = _colorsReserves[(int)availableColors[colorIndex]];
        }
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
        if (CurrentTroopSelected != null)
        {
            ResetAllCells();
            CurrentTroopSelected.IsSelected = false;
            CurrentTroopSelected.OnPointerExit();
        }
        
        CurrentTroopSelected = newTroop;
    }

    public void CheckMovementMyTroop(Cell newCell)
    {
        if (CurrentTroopSelected != null)
        {
            ResetAllCells();
            CurrentTroopSelected.MoveToNewCell(newCell);
            TroopsManager.Instance.AddNewMyTroopMovement(CurrentTroopSelected.ID, newCell.name, CurrentTroopSelected.MyColorID);
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

    public void ResetAllSelected()
    {
        if (CurrentTroopSelected != null)
            CurrentTroopSelected.DeselectTroop();

        CurrentTroopSelected = null;
        CurrentCellSelected = null;
    }
}