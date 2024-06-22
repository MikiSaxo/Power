using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopsManager : MonoBehaviour
{
    public static TroopsManager Instance { get; private set; }

    public List<GameObject> AllTroopObj { get; set; } = new List<GameObject>();
    public List<Troop> AllTroop { get; set; } = new List<Troop>();

    [Header("----- Troops -----")] [SerializeField]
    private GameObject _troopsParent;

    [SerializeField] private GameObject _troopPrefab;
    [SerializeField] private TroopInfos[] _troopAllInfos;
    [SerializeField] private int[] _troopStartInfos;

    [Header("----- Timings -----")] [SerializeField]
    private float _timeMoveTroop = .25f;

    [SerializeField] private float _timeWaitNewColor = .5f;


    private List<TroopsMovements> _myTroopsMovements = new List<TroopsMovements>();
    private List<Troop> _myTroopsLastMovements = new List<Troop>();
    
    private List<TroopsMovements> _allTroopsMovements = new List<TroopsMovements>();
    
    private int _countID;
    private Cell[] _cellHQ_BYRG;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _cellHQ_BYRG = Manager.Instance.CellHQ_BYRG;
        InitTroops();
    }

    private void InitTroops()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < _troopStartInfos.Length; i++)
            {
                InstantiateNewTroop(_troopStartInfos[i], j, _cellHQ_BYRG[j], i, true);
            }
        }
    }

    public void InstantiateNewTroop(int troopInfosIndex, int colorIndex, Cell cell, int indexPosCell = 0, bool isStart = false)
    {
        GameObject go = Instantiate(_troopPrefab, _troopsParent.transform);
        
        go.GetComponent<Troop>().InitTroop(
            _troopAllInfos[troopInfosIndex],
            colorIndex,
            cell,
            _countID,
            indexPosCell,
            isStart);
        
        AddNewTroopList(go);
        _countID++;
    }
    
    public void AddNewTroopList(GameObject newTroop)
    {
        AllTroopObj.Add(newTroop);
        AllTroop.Add(newTroop.GetComponent<Troop>());
    }
    
    public void RemoveTroopList(Troop troop)
    {
        AllTroopObj.Remove(troop.gameObject);
        AllTroop.Remove(troop);
    }

    public void AddNewMyTroopMovement(int troopID, string cellName, ColorsID colorID)
    {
        _myTroopsMovements.Add(new TroopsMovements(troopID, cellName, colorID));
    }

    public void StockMoveTroopS2C(int troopID, string cellName, ColorsID colorID)
    {
        _allTroopsMovements.Add(new TroopsMovements(troopID, cellName, colorID));
    }

    private void MoveTroop(int troopID, string cellName)
    {
        Troop saveTroop = null;
        foreach (var troop in AllTroop)
        {
            if (troop.ID == troopID)
                saveTroop = troop;
        }

        Cell saveCell = null;
        foreach (var cell in Manager.Instance.AllCells)
        {
            if (cell.name == cellName)
                saveCell = cell;
        }

        if (saveCell == null || saveTroop == null)
        {
            Debug.LogWarning($"saveCell or saveTroop is null, cellName: {cellName}, troopID: {troopID}");
            return;
        }

        saveTroop.MoveToNewCell(saveCell);
        saveTroop.ResetLineConnector();

        _myTroopsLastMovements.Add(saveTroop);
    }

    public void MoveMyTroopC2S()
    {
        foreach (var troop in _myTroopsMovements)
        {
            PlayerIOScript.Instance.Pioconnection.Send("MoveTroop", troop.TroopID, troop.CellName, (int)troop.TroopColorID);
        }

        print("All infos send to server");
        PlayerIOScript.Instance.Pioconnection.Send("AllMoveTroopSend");
    }

    public void MoveAllTroops()
    {
        print($"TimingMoveAllTroops : length : {_allTroopsMovements.Count}");

        StartCoroutine(TimingMoveAllTroops());
    }
    
    IEnumerator TimingMoveAllTroops()
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (var mov in _allTroopsMovements)
            {
                if ((int)mov.TroopColorID == i)
                    MoveTroop(mov.TroopID, mov.CellName);
                else
                    continue;

                yield return new WaitForSeconds(_timeMoveTroop);
            }

            yield return new WaitForSeconds(_timeWaitNewColor);
        }

        // Check combat 
        ReserveManager.Instance.AddAllPower(_myTroopsLastMovements);
        yield return new WaitForSeconds(_timeMoveTroop);
        RecenterTroops();
        ResetMovTroops();
        _allTroopsMovements.Clear();
        _myTroopsLastMovements.Clear();
        _myTroopsMovements.Clear();
    }

    public void RecenterTroops()
    {
        foreach (var troop in AllTroopObj)
        {
            troop.GetComponent<Troop>().ArrivedToNewCell();
        }
    }

    public void ResetMovTroops()
    {
        foreach (var troop in AllTroop)
        {
            troop.HasMoved = false;
        }
    }
}