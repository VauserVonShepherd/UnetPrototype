using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GlobalNetworkManager : UnityEngine.Networking.NetworkManager {
    public static GlobalNetworkManager instance;

    //Current players in the room
    public List<Player> AllPlayers = new List<Player>();
    //Store a dictionary of all players that joined, prevent loss of data (Persistence), use ipaddress (string) to find the player data again.

    /// <summary>
    /// Contains All PlayerData, the key is the ip address of the player data, used by the server to find the data of player
    /// </summary>
    public Dictionary<string, PlayerData> AllPlayerData = new Dictionary<string, PlayerData>();
    /// <summary>
    /// IP address of all players that connected before, used by the room system.
    /// </summary>
    public List<string> allPlayerIPAddress = new List<string>();

    /// <summary>
    /// Returns whether or not player is connected by checking their ip address, used by the playerstat, player tab on scoreboard
    /// </summary>
    /// <param name="ipaddress"></param>
    /// <returns></returns>
    public bool IsPlayerConnected(string ipaddress)
    {
        //CHECK ALL CONNECTED PLAYER
        foreach(Player player in AllPlayers)
        {
            //IF THE CONNECTED PLAYER CONTAINS THE ADDRESS, RETURN TRUE
            if(player.playerData.m_ipaddress == ipaddress)
            {
                return true;
            }
        }
        return false;
    }
    
    public void SetPlayerData(PlayerData newplayerdata)
    {
        StartCoroutine(SetPlayerDataCoroutine(newplayerdata));        
    }
    private IEnumerator SetPlayerDataCoroutine(PlayerData newplayerdata)
    {
        yield return new WaitForSeconds(0.5f);
        if (AllPlayerData.ContainsKey(newplayerdata.m_ipaddress))
        {
            AllPlayerData[newplayerdata.m_ipaddress] = newplayerdata;
        }
        RoomSystem.instance.RefreshScoreboard();
    }

    public string clientExternalIP {
        get
        {
            return NetworkManager.singleton.networkAddress;
        }
    }
    
    private void Start()
    {
        instance = this;
    }
    
    public void ResetHostData()
    {
        AllPlayerData = new Dictionary<string, PlayerData>();
        allPlayerIPAddress = new List<string>();
        Debug.Log(AllPlayerData.Count);
    }

    /// <summary>
    /// Connect to room with ip address
    /// </summary>
    /// <param name="ipaddress"></param>
    public void ConnectToServer(string ipaddress)
    {
        networkAddress = ipaddress;
        StartClient();
    }

    /// <summary>
    /// What to do when hosting a room
    /// </summary>
    public void HostRoom()
    {
        StartHost();
    }

    /// <summary>
    /// What happens when disconnect room
    /// </summary>
    public void DisconnectRoom()
    {
        StopClient();
        StopHost();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        
        if (MainMenuManager.instance)
        {
            MainMenuManager.instance.SetMenu(1);
            
        }
    }
    
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        if (MainMenuManager.instance)
        {
            MainMenuManager.instance.SetMenu(0);
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        if (RoomSystem.instance)
        {
            RoomSystem.instance.RefreshScoreboard();
        }
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        MyNetworkDiscovery.instance.StartBroadcast();
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        MyNetworkDiscovery.instance.StartListen();

        if (MainMenuManager.instance)
        {
            MainMenuManager.instance.SetMenu(0);
            RoomSystem.instance.RefreshScoreboard();
        }

        ResetHostData();
    }
}

//public void GetPlayerData(PlayerData newplayerdata)
//{
//    Debug.Log("does the player have persistence yet? " + newplayerdata.m_ipaddress);

//    if (AllPlayerData.ContainsKey(newplayerdata.m_ipaddress))
//    {
//        //Only if its the server, then refresh the scoreboard
//        if (RoomSystem.instance)
//        {
//            //AllPlayerData[newplayerdata.m_ipaddress] = new PlayerData(newplayerdata);
//            newplayerdata = AllPlayerData[newplayerdata.m_ipaddress];
//            RoomSystem.instance.RefreshScoreboard();
//        }
//    }
//    else
//    {
//        //if doesnt exist, set a new one
//        SetPlayerData(newplayerdata);
//    }
//}