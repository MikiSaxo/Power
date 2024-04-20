using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;
using UnityEngine;

public class MoveAllTroopS2C : IFunction
{
    public void Execute(Message message)
    {
        Debug.Log("MoveAllTroops go go go !");
        TroopsManager.Instance.MoveAllTroops();
    }
}
