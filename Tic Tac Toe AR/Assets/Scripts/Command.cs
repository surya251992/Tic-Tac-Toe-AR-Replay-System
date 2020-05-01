using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public enum PlayerNames { PLAYER_A = 0, PLAYER_B, NONE };
    public abstract void Execute(Vector3 selectedPosition, Command command, int angle = 0, PlayerNames playerNames = PlayerNames.NONE);
    public abstract void Plot(Vector3 selectedPosition);
    public virtual void Undo() { }
    public virtual void StrikeOffDots(Vector3 strikeOffPosition, int angle) { }
    public virtual void UpdateScores(PlayerNames playerNames) { }
}
