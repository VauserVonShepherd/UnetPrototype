using System.Collections;
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

    private List<Player> AllPlayers = new List<Player>();
    //Store a dictionary of all players that joined, prevent loss of data (Persistence), use ipaddress (string) to find the player data again.
    private Dictionary<string, PlayerData> AllPlayerData = new Dictionary<string, PlayerData>();
    private List<PlayerStat> playerStatList = new List<PlayerStat>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        RefreshUser();
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
    private IEnumerator RefreshUserCoroutine()
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
        foreach(Player player in AllPlayers)
        {
            if(player != null)
            {
                FreshPlayerList.Add(player);
            }
        }

        AllPlayers = FreshPlayerList;

        //For each player in room, add a new stat for them
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            //Spawn a new stat
            GameObject newPlayerStat = Instantiate(defaultPlayerStat.gameObject, StatScoreboard);

            playerStatList.Add(newPlayerStat.GetComponent<PlayerStat>());
            newPlayerStat.GetComponent<PlayerStat>().m_Player = AllPlayers[i];
            newPlayerStat.GetComponent<PlayerStat>().UpdateStat();
        }
    }

    /// <summary>
    /// Run the refresh user corountine
    /// </summary>
    public void RefreshUser()
    {
        StartCoroutine(RefreshUserCoroutine());
    }

    public List<string> allip = new List<string>();

    public void AddPlayer(Player player)
    {
        //If the player has not connected before
        if (!AllPlayerData.ContainsKey(player.playerIPAddress))
        {
            PlayerData newplayerdata = new PlayerData(player.playerData);

            //add it to the history of connected player to save persistence
            AllPlayerData.Add(newplayerdata.m_ipaddress, newplayerdata);
            allip.Add(newplayerdata.m_ipaddress);
        }
        else
        {
            player.playerData = new PlayerData(AllPlayerData[player.playerIPAddress]);
        }

        //add the player && refresh
        AllPlayers.Add(player);
        RefreshUser();
    }
}
