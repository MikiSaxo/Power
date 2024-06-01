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

    private List<GameObject> _powerList = new List<GameObject>();
    private List<GameObject> _troopList = new List<GameObject>();

    public ColorsID MyColorID { get; set; }

    public void InitReserve(ColorsID colorsID, Color color)
    {
        MyColorID = colorsID;
        _bgImage.color = color;
    }

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
        for (int i = nb - 1; i >= 0; i--)
        {
            var power = _powerList[i];
            _powerList.RemoveAt(i);
            Destroy(power);
        }
    }
}