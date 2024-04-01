using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reserve : MonoBehaviour
{
    [SerializeField] private GameObject _powerPrefab;
    [SerializeField] private Transform _grid;

    private List<GameObject> _powerList = new List<GameObject>();

    public void AddNewPower(int colorIndex)
    {
        GameObject go = Instantiate(_powerPrefab, _grid.transform);
        go.GetComponent<Power>().InitReserve(colorIndex);
        
        _powerList.Add(go);
    }
}
