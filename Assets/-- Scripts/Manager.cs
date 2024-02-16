using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    [SerializeField] private TMP_Text _textOrderLeft;

    public Colors MyColor { get; set; }
    public int OrdersDone { get; private set; }
    public Troop CurrentTroopSelected { get; set; }
    public Cell CurrentCellSelected { get; set; }

    private List<GameObject> _allTroopObj = new List<GameObject>();
    
    const int MaxOrdersDone = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < _troopStartInfos.Length; i++)
            {
                GameObject go = Instantiate(_troopPrefab, _troopsParent.transform);
                go.GetComponent<Troop>().InitTroop(_troopAllInfos[_troopStartInfos[i]], j, _cellHQ_BYRG[j], i);
                _allTroopObj.Add(go);
            }
        }

        SetMyColor(Colors.Blue);
    }

    public void SetMyColor(Colors color)
    {
        MyColor = color;
        _textMyColor.text = $"You are {MyColor}";
        _textMyColor.color = _colorsText[(int)MyColor];
    }

    public void RecenterTroops()
    {
        foreach (var troop in _allTroopObj)
        {
            troop.GetComponent<Troop>().ArrivedToNewCell();
        }
    }

    public bool CanAddNewOrder()
    {
        if (OrdersDone >= MaxOrdersDone)
        {
            FeedbackCantDoMoreOrders();
            return false;
        }
        
        return true;
    }

    public void UpdateOrdersLeft(bool isAdding)
    {
        if (isAdding)
            OrdersDone++;
        else
            OrdersDone--;

        if (OrdersDone < 0)
            OrdersDone = 0;
        
        _textOrderLeft.text = $"Orders Left : {OrdersDone}/5";
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
    
    public void FeedbackCantDoMoreOrders()
    {
        _textOrderLeft.gameObject.transform.DOComplete();
        _textOrderLeft.DOComplete();
        
        _textOrderLeft.gameObject.transform.DOPunchPosition(Vector3.one * 5, 1);
        _textOrderLeft.gameObject.transform.DOPunchRotation(Vector3.one, 1);
        _textOrderLeft.DOColor(Color.red, .25f).SetLoops(2, LoopType.Yoyo);
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