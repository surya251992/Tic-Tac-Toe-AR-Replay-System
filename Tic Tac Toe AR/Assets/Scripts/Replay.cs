using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replay : Command
{
    public override void Execute(Vector3 selectedPosition, Command command, int angle = 0, PlayerNames playerNames = PlayerNames.NONE)
    {
        foreach (GameObject ga in GameManager.placedObjects)
            Object.Destroy(ga);
        GameManager.replayCommands.Clear();
        GameManager.replayCommands.TrimExcess();
        GameManager.replayPositions.Clear();
        GameManager.replayPositions.TrimExcess();
        GameManager.placedObjects.Clear();
        GameManager.placedObjects.TrimExcess();
        GameManager.isPlayerA = true;
        GameManager.isPlayerB = false;
        GameManager.gameOver = false;
    }

    public override void Plot(Vector3 selectedPosition)
    {
        throw new System.NotImplementedException();
    }
}
