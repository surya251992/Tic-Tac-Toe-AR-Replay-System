using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerB : Command
{
    GameObject playerBObject = Resources.Load("X") as GameObject;
    Vector3 selectedPosition;

    public override void Execute(Vector3 selectedPosition, Command command, int angle = 0, PlayerNames playerNames = PlayerNames.NONE)
    {
        Plot(selectedPosition);
    }

    public override void Plot(Vector3 selectedPosition)
    {
        Debug.Log("entered Player B");
        var objectSpawned = Object.Instantiate(playerBObject, selectedPosition, Quaternion.identity);
        GameManager.placedObjects.Add(objectSpawned);
    }

    public override void Undo()
    {
        GameManager.isPlayerB = true;
        GameManager.isPlayerA = false;
        GameManager.playerNameText.text = "Player B";
        Object.Destroy(GameManager.placedObjects[GameManager.placedObjects.Count - 1]);
        GameManager.placedObjects.RemoveAt(GameManager.placedObjects.Count - 1);
        GameManager.placedObjects.TrimExcess();
        GameManager.replayCommands.RemoveAt(GameManager.replayCommands.Count - 1);
        GameManager.replayCommands.TrimExcess();
        GameManager.replayPositions.RemoveAt(GameManager.replayPositions.Count - 1);
        GameManager.replayPositions.TrimExcess();
    }
}
