using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [field: SerializeField] public Colors CellColor { get; set; }
    [field: SerializeField] public bool IsSea { get; set; }
    [field: SerializeField] public List<Cell> Neighbor { get; set; } = new List<Cell>();

    [Header("---- Prefab")]
    [SerializeField] private GameObject _img;


    private void Start()
    {
        if (CellColor == Colors.Yellow)
        {
            UpdateViewCell(true);
            foreach (var cell in Neighbor)
            {
                cell.UpdateViewCell(true);
            }
        }
    }

    public void UpdateViewCell(bool state)
    {
        _img.SetActive(state);
    }
}

public enum Colors
{
    Neutral = -1,
    Blue = 0,
    Yellow = 1,
    Red = 2,
    Green = 3,
}
