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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitTroops();
    }

    private void InitTroops()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < _troopStartInfos.Length; i++)
            {
                GameObject go = Instantiate(_troopPrefab, _troopsParent.transform);
                go.GetComponent<Troop>().InitTroop(_troopAllInfos[_troopStartInfos[i]], j,
                    Manager.Instance.CellHQ_BYRG[j], i, _countID);
                AddNewTroop(go);
                _countID++;
            }
        }
    }

    public void AddNewTroop(GameObject newTroop)
    {
        AllTroopObj.Add(newTroop);
        AllTroop.Add(newTroop.GetComponent<Troop>());
    }

    public void AddNewMyTroopMovement(int troopID, string cellName, Colors color)
    {
        _myTroopsMovements.Add(new TroopsMovements(troopID, cellName, color));
    }

    public void StockMoveTroopS2C(int troopID, string cellName, Colors color)
    {
        _allTroopsMovements.Add(new TroopsMovements(troopID, cellName, color));
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
            PlayerIOScript.Instance.Pioconnection.Send("MoveTroop", troop.TroopID, troop.CellName, (int)troop.TroopColor);
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
                if ((int)mov.TroopColor == i)
                    MoveTroop(mov.TroopID, mov.CellName);
                else
                    continue;

                yield return new WaitForSeconds(_timeMoveTroop);
            }

            yield return new WaitForSeconds(_timeWaitNewColor);
        }

        ReserveManager.Instance.AddAllPower(_myTroopsLastMovements);
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