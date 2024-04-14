
using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;
using UnityEngine;

public class AddNewVoteEndOfTurnS2C : IFunction
{
    public void Execute(Message m)
    {
        Debug.Log("add vote");
        EndOfTurnManager.Instance.UpdateNbVote(1);
    }
}
