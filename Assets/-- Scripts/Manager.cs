using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [Header("----- Cells -----")]
    [SerializeField] private List<Cell> _allCells = new List<Cell>();
    [SerializeField] private Cell[] _cellHQ_BYRG;
    
    [Header("----- Troops -----")]
    [SerializeField] private GameObject _troopsParent;
    [SerializeField] private GameObject _troopPrefab;
    [SerializeField] private TroopInfos[] _troopAllInfos;
    [SerializeField] private int[] _troopStartInfos;

    [Header("----- Player UI -----")]
    [SerializeField] private TMP_Text _textMyColor;
    [SerializeField] private Color[] _colorsText;

    [Header("----- Reserves -----")]
    [SerializeField] private GameObject[] _reserves;
    [SerializeField] private Color[] _colorsReserves;

    [Header("----- Colors -----")] 
    public Color[] PawnColors;
    

    public Colors MyColor { get; set; }
    public Troop CurrentTroopSelected { get; set; }
    public Cell CurrentCellSelected { get; set; }

    public List<GameObject> AllTroopObj { get; set; } = new List<GameObject>();
    public List<Troop> AllTroop { get; set; } = new List<Troop>();

    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitTroops();

        SetMyColor(Colors.Blue);
        
        SetReserve();
    }

    private void InitTroops()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < _troopStartInfos.Length; i++)
            {
                GameObject go = Instantiate(_troopPrefab, _troopsParent.transform);
                go.GetComponent<Troop>().InitTroop(_troopAllInfos[_troopStartInfos[i]], j, _cellHQ_BYRG[j], i);
                AddNewTroop(go);
            }
        }
    }

    public void AddNewTroop(GameObject newTroop)
    {
        AllTroopObj.Add(newTroop);
        AllTroop.Add(newTroop.GetComponent<Troop>());
    }

    private void SetMyColor(Colors color)
    {
        MyColor = color;
        _textMyColor.text = $"You are {MyColor}";
        _textMyColor.color = _colorsText[(int)MyColor];
    }

    private void SetReserve()
    {
        // Prévoir un script réserve ou une list/dico pour savoir quelle réserve appartient à quelle couleur
        _reserves[0].GetComponentInChildren<Image>().color = _colorsReserves[(int)MyColor];
        
        List<Colors> availableColors = new List<Colors> { Colors.Blue, Colors.Yellow, Colors.Red, Colors.Green };
        availableColors.Remove(MyColor);

        for (int i = 1; i < _reserves.Length; i++)
        {
            var colorIndex = i - 1;
            _reserves[i].GetComponentInChildren<Image>().color = _colorsReserves[(int)availableColors[colorIndex]];
        }
    }

    public void RecenterTroops()
    {
        foreach (var troop in AllTroopObj)
        {
            troop.GetComponent<Troop>().ArrivedToNewCell();
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
        if (CurrentTroopSelected != null)
            CurrentTroopSelected.DeselectTroop();

        CurrentTroopSelected = null;
        CurrentCellSelected = null;
    }

    public GameObject[] GetReserves()
    {
        return _reserves;
    }
}