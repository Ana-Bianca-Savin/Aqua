using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public static Vector2 checkpointPosSave;

    public PlayerData()
    {
        checkpointPosSave = MOVEMENT.checkpointPos;
    }
}
