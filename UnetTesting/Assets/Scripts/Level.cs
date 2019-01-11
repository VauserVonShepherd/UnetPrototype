using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Level {
    [SyncVar]
    public int stageVal;
    public string StageName
    {
        get
        {
            //Stage1, etc
            return "Stage" + stageVal;
        }
    }

    [SyncVar]
    public string status;
    [SyncVar]
    public float time;
    [SyncVar]
    public int tries;
    [SyncVar]
    public int firstTryScore;

    /// <summary>
    /// Assign the level it's values
    /// </summary>
    /// <param name="levelVal">The Level number</param>
    public Level(int levelVal)
    {
        stageVal = levelVal;
        status = "Incomplete";
        time = 0;
        tries = 0;
        firstTryScore = 0;
    }

    public Level (Level level)
    {
        stageVal = level.stageVal;
        status = level.status;
        time = level.time;
        tries = level.tries;
        firstTryScore = level.firstTryScore;
    }
}
