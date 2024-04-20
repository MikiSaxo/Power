using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopsManager : MonoBehaviour
{
    public static TroopsManager Instance { get; private set; }

    [Header("----- Troops -----")] [SerializeField]
    private GameObject _troopsParent;

    [SerializeField] private GameObject _troopPrefab;
    [SerializeField] private TroopInfos[] _troopAllInfos;
    [SerializeField] private int[] _troopStartInfos;
    public List<GameObject> AllTroopObj { get; set; } = new List<GameObject>();
    public List<Troop> AllTroop { get; set; } = new List<Troop>();

    private List<TroopsMovements> _troopsMovements = new List<TroopsMovements>();
    private List<Troop> _troopsLastMovements = new List<Troop>();
    private int _countID;


    [Header("----- Timings -----")]
    [SerializeField] private float _timeMoveTroop = .25f;
    [SerializeField] private float _timeWaitNewColor = .5f;


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

    public void AddNewTroopMovement(int troopID, string cellName)
    {
        _troopsMovements.Add(new TroopsMovements(troopID, cellName));
    }

    public void MoveTroopS2C(int troopID, string cellName)
    {
        var saveTroop = new Troop();
        foreach (var troop in AllTroop)
        {
            if (troop.ID == troopID)
                saveTroop = troop;
        }

        var saveCell = new Cell();
        foreach (var cell in Manager.Instance.AllCells)
        {
            if (cell.name == cellName)
                saveCell = cell;
        }

        saveTroop.MoveToNewCell(saveCell);
        saveTroop.ResetLineConnector();
        
        _troopsLastMovements.Add(saveTroop);
    }

    public void MoveAllTroopsC2S()
    {
        StartCoroutine(TimingMoveAllTroops());
    }

    IEnumerator TimingMoveAllTroops()
    {
        foreach (var mov in _troopsMovements)
        {
            for (int i = 0; i < 4; i++)
            {
                if (mov.TroopID == i)
                    PlayerIOScript.Instance.Pioconnection.Send("MOVE", mov.TroopID, mov.CellName);

                yield return new WaitForSeconds(_timeMoveTroop);
            }

            yield return new WaitForSeconds(_timeWaitNewColor);
        }
        
        ReserveManager.Instance.AddAllPower(_troopsLastMovements);
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