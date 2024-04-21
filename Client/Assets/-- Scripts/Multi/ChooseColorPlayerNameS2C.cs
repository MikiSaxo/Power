using PlayerIOClient;
using UnityEngine;

public class ChooseColorPlayerNameS2C : IFunction
{
    public void Execute(Message message)
    {
        var playerName = message.GetString(1);
        var index = message.GetInt(2);

        ColorMenuAnim.Instance.AddOtherPlayerName(playerName, index);
    }
}