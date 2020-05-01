using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerA : Command
{
    GameObject playerAObject = Resources.Load("O") as GameObject;
    Vector3 selectedPosition;

    public override void Execute(Vector3 selectedPosition, Command command, int angle = 0, PlayerNames playerNames = PlayerNames.NONE)
    {
        Plot(selectedPosition);
    }

    public override void Plot(Vector3 selectedPosition)
    {
        Debug.Log("entered Player A");
        var objectSpawned = Object.Instantiate(playerAObject, selectedPosition, Quaternion.identity);
        GameManager.placedObjects.Add(objectSpawned);
    }

    public override void Undo()
    {
        GameManager.isPlayerB = false;
        GameManager.isPlayerA = true;
        GameManager.playerNameText.text = "Player A";
        Object.Destroy(GameManager.placedObjects[GameManager.placedObjects.Count - 1]);
        GameManager.placedObjects.RemoveAt(GameManager.placedObjects.Count - 1);
        GameManager.placedObjects.TrimExcess();
        GameManager.replayCommands.RemoveAt(GameManager.replayCommands.Count - 1);
        GameManager.replayCommands.TrimExcess();
        GameManager.replayPositions.RemoveAt(GameManager.replayPositions.Count - 1);
        GameManager.replayPositions.TrimExcess();
    }
}
