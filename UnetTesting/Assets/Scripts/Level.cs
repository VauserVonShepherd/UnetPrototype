using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {
    public int stageVal;
    public string StageName
    {
        get
        {
            //Stage1, etc
            return "Stage" + stageVal;
        }
    }

    public string status;
    public float time;
    public int tries;
    public int firstTryScore;

    /// <summary>
    /// Assign the level it's values
    /// </summary>
    /// <param name="levelVal">The Level number</param>
    public Level(int levelVal)
    {
        stageVal = levelVal;
        status = "-";
        time = 0;
        tries = 0;
        firstTryScore = 0;
    }
}
