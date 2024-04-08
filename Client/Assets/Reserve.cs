using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reserve : MonoBehaviour
{
    [SerializeField] private GameObject _powerPrefab;
    [SerializeField] private Transform _grid;
    [SerializeField] private GameObject _troopPrefab;
    [SerializeField] private Transform _gridTroops;
    [SerializeField] private TroopInfos[] _troops;

    private List<GameObject> _powerList = new List<GameObject>();
    private List<GameObject> _troopList = new List<GameObject>();

    public void AddNewPower(int colorIndex)
    {
        GameObject go = Instantiate(_powerPrefab, _grid.transform);
        go.GetComponent<Power>().InitReserve(colorIndex);
        
        _powerList.Add(go);
    }

    public void AddNewUnit(int index)
    {
        GameObject go = Instantiate(_troopPrefab, _gridTroops.transform);
        go.GetComponent<Troop>().InitTroopReserve(_troops[index], 0, transform);
     
        _troopList.Add(go);
    }

    public void RemovePower(int nb)
    {
        for (int i = nb-1; i >= 0; i--)
        {
            var power = _powerList[i];
            _powerList.RemoveAt(i);
            Destroy(power);
        }
    }
}
