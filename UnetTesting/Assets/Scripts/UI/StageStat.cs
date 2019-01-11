using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageStat : MonoBehaviour {
    public Text statusTxt, timeTxt, triesTxt, firstTryTxt;
    
    /// <summary>
    /// Update the stage status
    /// </summary>
    public void UpdateText(Level level)
    {
        statusTxt.text = level.status;
        timeTxt.text = level.time.ToString("f1");
        triesTxt.text = level.tries.ToString("f0");
        firstTryTxt.text = level.firstTryScore.ToString("f0");
    }
}
