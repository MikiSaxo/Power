using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopsMovements
{
    public int TroopID;
    public string CellName;
    public ColorsID TroopColorID;

    public TroopsMovements(int id, string cellName, ColorsID colorsID)
    {
        TroopID = id;
        CellName = cellName;
        TroopColorID = colorsID;
    }
}
