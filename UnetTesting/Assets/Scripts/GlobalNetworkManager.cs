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

    public Dictionary<string, PlayerData> AllPlayerData = new Dictionary<string, PlayerData>();
    public void GetPlayerData(PlayerData newplayerdata)
    {
        Debug.Log("does the player have persistence yet? " + newplayerdata.m_ipaddress);

        if (AllPlayerData.ContainsKey(newplayerdata.m_ipaddress))
        {
            //Only if its the server, then refresh the scoreboard
            if (RoomSystem.instance)
            {
                //AllPlayerData[newplayerdata.m_ipaddress] = new PlayerData(newplayerdata);
                newplayerdata = AllPlayerData[newplayerdata.m_ipaddress];
                RoomSystem.instance.RefreshScoreboard();
            }
        }
        else
        {
            //if doesnt exist, set a new one
            SetPlayerData(newplayerdata);
        }
    }

    public void SetPlayerData(PlayerData newplayerdata)
    {
        StartCoroutine(SetPlayerDataCoroutine(newplayerdata));

        if (newplayerdata.isServer)
        {
            Debug.Log(newplayerdata.isServer);
            RoomSystem.instance.RefreshScoreboard();
        }
    }
    private IEnumerator SetPlayerDataCoroutine(PlayerData newplayerdata)
    {
        yield return new WaitForSeconds(1);
        if (AllPlayerData.ContainsKey(newplayerdata.m_ipaddress))
        {
            AllPlayerData[newplayerdata.m_ipaddress] = newplayerdata;
            if (RoomSystem.instance)
            {
                RoomSystem.instance.RefreshScoreboard();
            }
        }
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

    private void Update()
    {
        if(AllPlayerData.Count > 0)
        {
            //Debug.Log(AllPlayerData["192.168.1.134"].m_color + " : " + AllPlayerData["192.168.1.134"].m_name);
        }
    }

    public void ResetHostData()
    {
        AllPlayerData = new Dictionary<string, PlayerData>();
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

    public void HostRoom()
    {
        StartHost();
    }

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
