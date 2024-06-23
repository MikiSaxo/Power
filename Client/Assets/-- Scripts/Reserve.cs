using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Reserve : MonoBehaviour
{
    [SerializeField] private GameObject _powerPrefab;
    [SerializeField] private Transform _grid;
    [SerializeField] private GameObject _troopPrefab;
    [SerializeField] private Transform _gridTroops;
    [SerializeField] private TroopInfos[] _troops;
    [SerializeField] private Image _bgImage;
    [SerializeField] private Cell _myCell;

    private List<GameObject> _powerList = new List<GameObject>();
    private List<GameObject> _troopList = new List<GameObject>();

    public ColorsID MyColorID { get; set; }

    public void InitReserve(ColorsID colorsID, Color color)
    {
        MyColorID = colorsID;
        _bgImage.color = color;
        _myCell.InitCellReserve(Manager.Instance.CellHQ_BYRG[(int)colorsID], colorsID);
    }

    public void AddNewPower(int colorIndex)
    {
        GameObject go = Instantiate(_powerPrefab, _grid.transform);
        go.GetComponent<Power>().InitReserve(colorIndex);

        _powerList.Add(go);
    }

    public void AddNewUnitFromShop(TroopsType troopType)
    {
        // GameObject go = Instantiate(_troopPrefab, _gridTroops.transform);
        // go.GetComponent<Troop>().InitTroopReserve(_troops[(int)troopIndex], 0, _myCell, transform);
        //
        // _troopList.Add(go);
        
        print($"-- Add new unit from shop: {troopType} --");
        
        TroopsManager.Instance.InstantiateNewTroop(troopType, (int)MyColorID, _myCell, true);
    }

    public void RemovePower(int nb)
    {
        for (int i = nb - 1; i >= 0; i--)
        {
            var power = _powerList[i];
            _powerList.RemoveAt(i);
            Destroy(power);
        }
    }
}