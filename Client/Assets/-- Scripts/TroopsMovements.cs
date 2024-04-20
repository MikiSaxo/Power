using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopsMovements
{
    public int TroopID;
    public string CellName;
    public Colors TroopColor;

    public TroopsMovements(int id, string cellName, Colors colors)
    {
        TroopID = id;
        CellName = cellName;
        TroopColor = colors;
    }
}
