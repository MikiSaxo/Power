using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [field: SerializeField] public Colors Color { get; set; }
    [field: SerializeField] public bool IsSea { get; set; }
    [field: SerializeField] public List<Cell> Neighbor { get; set; } = new List<Cell>();
}

public enum Colors
{
    Neutral = -1,
    Blue = 0,
    Yellow = 1,
    Red = 2,
    Green = 3,
}
