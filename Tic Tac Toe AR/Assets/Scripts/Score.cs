using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : Command
{

    public static int playerAScore = 0, playerBScore = 0;
    public override void Execute(Vector3 selectedPosition, Command command, int angle = 0, PlayerNames playerNames = PlayerNames.NONE)
    {
        UpdateScores(playerNames);
    }

    public override void Plot(Vector3 selectedPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateScores(PlayerNames playerNames)
    { 
        if (playerNames == PlayerNames.PLAYER_A)
            playerAScore++;
        else if(playerNames == PlayerNames.PLAYER_B)
            playerBScore++;
        GameManager.scoreCard.SetActive(true);
        GameManager.playerAScoreText.text = "Player A:" + playerAScore;
        GameManager.playerBScoreText.text = "Player B:" + playerBScore;
    }
}
