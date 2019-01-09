using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour {
    [HideInInspector]
    public Player m_Player;

    public Text nameTxt;    //Name
    public Image colorSprite; //Color

    //Stat for all stage, each have 4 text
    public List<StageStat> allStageStat = new List<StageStat>();
    
    public void UpdateStat()
    {
        nameTxt.text = m_Player.playerName;

        colorSprite.color = m_Player.playerColor;

        for (int i = 0; i < m_Player.allLevel.Count; i++)
        {
            allStageStat[i].UpdateText(m_Player.allLevel["Stage"+ (i+1)]);
        }
    }
}
