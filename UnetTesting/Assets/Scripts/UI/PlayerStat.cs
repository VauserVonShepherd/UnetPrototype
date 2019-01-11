using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour {
    [HideInInspector]
    public PlayerData m_PlayerData;

    public Text nameTxt;    //Name
    public Image colorSprite; //Color

    /// <summary>
    /// Check if the player exist using the player data this object is assigned to on the globalnetwork manager
    /// </summary>
    public bool isConnected = false;
    /// <summary>
    /// Feedack of disconnection of player.
    /// </summary>
    public GameObject DisconnectedIndicatorObject;
    
    //Stat for all stage, each have 4 text
    public List<StageStat> allStageStat = new List<StageStat>();
    
    public void UpdateStat()
    {
        nameTxt.text = m_PlayerData.m_name;

        colorSprite.color = m_PlayerData.m_color;
        
        for (int i = 0; i < m_PlayerData.allLevel.Count; i++)
        {
            allStageStat[i].UpdateText(m_PlayerData.allLevel["Stage"+ (i+1)]);
        }

        //Disconnect Indicator will be active if the player is not active in the server, vice versa if true
        DisconnectedIndicatorObject.SetActive(!GlobalNetworkManager.instance.IsPlayerConnected(m_PlayerData.m_ipaddress));
    }
}
