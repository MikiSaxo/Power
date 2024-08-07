using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopsManager : MonoBehaviour
{
    public static TroopsManager Instance { get; private set; }

    public List<GameObject> AllTroopObj { get; set; } = new List<GameObject>();
    public List<Troop> AllTroop { get; set; } = new List<Troop>();

    public int TroopCountID => _countID;

    [Header("----- Troops -----")] [SerializeField]
    private GameObject _troopsParent;

    [SerializeField] private GameObject _troopPrefab;
    [SerializeField] private TroopInfos[] _troopAllInfos;
    [SerializeField] private TroopsType[] _troopStartInfos;
    [SerializeField] private Transform[] _troopReserveParent;

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
    }
    
    public void InitStartsTroops()
    {
        StartCoroutine(InitTroops());
    }

    private IEnumerator InitTroops()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < _troopStartInfos.Length; i++)
            {
                InstantiateNewTroop(_troopStartInfos[i], j, _cellHQ_BYRG[j], false, i, true);
                yield return new WaitForSeconds(.1f);
            }
        }
    }

    public void InstantiateNewTroop(TroopsType troopType, int colorIndex, Cell cell, bool isReserve = false,
        int indexPosCell = 0, bool isStart = false)
    {
        GameObject go = Instantiate(_troopPrefab, _troopsParent.transform);
        
        // print($"----- Init : {troopType}");

        go.GetComponent<Troop>().InitTroop(
            _troopAllInfos[(int)troopType],
            colorIndex,
            cell,
            _countID);

        AddNewTroopList(go);
        _countID++;

        if (isReserve)
        {
            go.transform.SetParent(_troopReserveParent[colorIndex]);
        }
        else
        {
            if (isStart)
                go.transform.position = cell.StartPointsHQ[indexPosCell].position;
            else
                go.transform.position = cell.transform.position;
        }

        go.GetComponent<Troop>().ArrivedToNewCell();
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
            PlayerIOScript.Instance.Pioconnection.Send("MoveTroop", troop.TroopID, troop.CellName,
                (int)troop.TroopColorID);
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
        ReserveManager.Instance.AddAllPower(AllTroop);
        yield return new WaitForSeconds(_timeMoveTroop);
        RecenterTroops();
        ResetMovTroops();
        _allTroopsMovements.Clear();
        _myTroopsLastMovements.Clear();
        _myTroopsMovements.Clear();
    }

    public void RecenterTroops()
    {
        foreach (var troop in AllTroop)
        {
            troop.ArrivedToNewCell();
        }
    }

    public void ResetMovTroops()
    {
        foreach (var troop in AllTroop)
        {
            troop.HasMoved = false;
            troop.ResetLineConnector();
        }
    }
}