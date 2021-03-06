﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RoomSystem will work on local client in the serverhost
/// </summary>
public class RoomSystem : MonoBehaviour {
    //singleton
    public static RoomSystem instance;

    //public 
    public PlayerStat defaultPlayerStat;
    
    //Private
    [SerializeField]
    private Transform StatScoreboard;

    private List<PlayerStat> playerStatList = new List<PlayerStat>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        RefreshScoreboard();
    }

    /// <summary>
    /// Clear the board of players
    /// </summary>
    private void ClearPlayerList()
    {        
        //Clear the current player list
        foreach (PlayerStat playerstat in playerStatList)
        {
            Destroy(playerstat.gameObject);
        }

        playerStatList = new List<PlayerStat>();
    }

    /// <summary>
    /// Refresh the scoreboard
    /// </summary>
    private IEnumerator RefreshScoreboardCoroutine()
    {
        yield return new WaitForSeconds(1);

        //Clear the current player list
        foreach(PlayerStat playerstat in playerStatList)
        {
            Destroy(playerstat.gameObject);
        }
        
        playerStatList = new List<PlayerStat>();

        //Temp player list, check if existing players have disconnected 
        List<Player> FreshPlayerList = new List<Player>();
        //Clear out the players that are not there anymore
        foreach(Player player in GlobalNetworkManager.instance.AllPlayers)
        {
            if(player != null)
            {
                FreshPlayerList.Add(player);
            }
        }

        GlobalNetworkManager.instance.AllPlayers = FreshPlayerList;

        //For each player in room, add a new stat for them
        for (int i = 0; i < GlobalNetworkManager.instance.allPlayerIPAddress.Count; i++)
        {
            //Spawn a new stat
            GameObject newPlayerStat = Instantiate(defaultPlayerStat.gameObject, StatScoreboard);

            playerStatList.Add(newPlayerStat.GetComponent<PlayerStat>());
            newPlayerStat.GetComponent<PlayerStat>().m_PlayerData = GlobalNetworkManager.instance.AllPlayerData[GlobalNetworkManager.instance.allPlayerIPAddress[i]];

            newPlayerStat.GetComponent<PlayerStat>().UpdateStat();
        }
    }

    /// <summary>
    /// Run the refresh user corountine
    /// </summary>
    public void RefreshScoreboard()
    {
        StartCoroutine(RefreshScoreboardCoroutine());
    }
    
    public void AddPlayer(Player player)
    {
        //add the player && refresh
        GlobalNetworkManager.instance.AllPlayers.Add(player);
        
        RefreshScoreboard();
    }
}
