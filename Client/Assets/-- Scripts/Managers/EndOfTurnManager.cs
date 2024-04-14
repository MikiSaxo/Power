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
        PlayerIOScript.Instance.Pioconnection.Send("Want_EndOfTurn");
    }

    public void AddNewPlayer(int nbPlayer)
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

    private void GoEndOfTurn()
    {
        _hasVote = false;
        OrdersManager.Instance.ResetOrders();
        Manager.Instance.MoveAllTroops();
        // Manager.Instance.ResetMovTroops();

        int count = 0;
        foreach (var troop in Manager.Instance.AllTroop)
        {
            troop.ResetLineConnector();

            if (troop.IsMyCellEnemyColor())
                count++;
        }
        
        ReserveManager.Instance.SpawnPowerToReserve(count);
    }
}
