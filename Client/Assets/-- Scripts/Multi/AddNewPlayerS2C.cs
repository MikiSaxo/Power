using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;
using UnityEngine;

public class AddNewPlayerS2C : IFunction
{
    public void Execute(Message m)
    {
        int nbPlayer = m.GetInt(1);
        Debug.Log("add new player");
        EndOfTurnManager.Instance.UpdateNbPlayer(nbPlayer);
    }
}
