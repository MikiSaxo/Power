using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;
using UnityEngine;

public class MoveTroopS2C : IFunction
{
    public void Execute(Message m)
    {
        int troopID = m.GetInt(1);
        string newCell = m.GetString(2);
        int color = m.GetInt(3);
        
        // Debug.Log($"troop ID : {troopID}, newCell :{newCell}, color : {color}");
        
        TroopsManager.Instance.StockMoveTroopS2C(troopID, newCell, (Colors)color);
    }
}
