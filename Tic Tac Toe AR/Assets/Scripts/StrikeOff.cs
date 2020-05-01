using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeOff : Command
{
    GameObject strikeOffMark = Resources.Load("Cross Off") as GameObject;

    public override void Execute(Vector3 selectedPosition, Command command, int angle, PlayerNames playerNames = PlayerNames.NONE)
    {
        StrikeOffDots(selectedPosition, angle);
    }

    public override void Plot(Vector3 selectedPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void StrikeOffDots(Vector3 strikeOffPosition, int angle)
    {
        var objectSpawned = Object.Instantiate(strikeOffMark,strikeOffPosition, Quaternion.identity);
        objectSpawned.transform.eulerAngles =new Vector3(0, 0, angle);
        GameManager.placedObjects.Add(objectSpawned);
        GameManager.angle = angle;
    }
}
