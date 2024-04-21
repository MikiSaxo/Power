using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EndOfTurnManager : MonoBehaviour
{
    public static EndOfTurnManager Instance;

    public int CurrentPlayerNumberGame { get; set; }
    public int CurrentPlayerNumberVote { get; set; }
    
    [SerializeField] private TMP_Text _playerNbText;
    private bool _hasVote;

    private void Awake()
    {
        Instance = this;
    }
    
    public void OnEndOfTurnButton()
    {
        if (_hasVote == true) return;
        
        _hasVote = true;
        // Verify if PlayerIO is not null
        if (PlayerIOScript.Instance.Pioconnection == null)
            return;
        PlayerIOScript.Instance.Pioconnection.Send("Want_EndOfTurn");
    }

    public void UpdateNbPlayer(int nbPlayer)
    {
        CurrentPlayerNumberGame = nbPlayer;
        UpdatePlayerNbText();
    }

    public void UpdateNbVote(int value)
    {
        CurrentPlayerNumberVote += value;
        UpdatePlayerNbText();
        
        CheckGoEndOfTurn();
    }

    private void UpdatePlayerNbText()
    {
        _playerNbText.text = $"{CurrentPlayerNumberVote}/{CurrentPlayerNumberGame}";
    }

    private void CheckGoEndOfTurn()
    {
        if(CurrentPlayerNumberVote == CurrentPlayerNumberGame)
            GoEndOfTurn();
    }

    private void ResetEndOfTurnNb()
    {
        CurrentPlayerNumberVote = 0;
        UpdatePlayerNbText();
    }

    private void GoEndOfTurn()
    {
        _hasVote = false;
        ResetEndOfTurnNb();
        OrdersManager.Instance.ResetOrders();
        TroopsManager.Instance.MoveMyTroopC2S();
        // Manager.Instance.ResetMovTroops();

        // int count = 0;
        // foreach (var troop in TroopsManager.Instance.AllTroop)
        // {
        //     troop.ResetLineConnector();
        //
        //     if (troop.IsMyCellEnemyColor())
        //         count++;
        // }
        
        // ReserveManager.Instance.SpawnPowerToReserve(count);
    }
}
