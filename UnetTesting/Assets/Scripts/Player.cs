using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// Holds player information through network
/// </summary>
public class Player : NetworkBehaviour
{    
    public GameStateScriptableObject gameState;

    public PlayerData playerData = new PlayerData();

    [SyncVar]
    public string m_name;
    [SyncVar]
    public string m_ipaddress;
    [SyncVar]
    public Color m_color;

    //Player Info
    public string playerIPAddress
    {
        get
        {
            return playerData.m_ipaddress;
        }
        set
        {
            playerData.m_ipaddress = value;
            m_ipaddress = value;
        }
    }
    
    public string playerName
    {
        get
        {
            return playerData.m_name;
        }
        set
        {
            playerData.m_name = value;
            m_name = value;
        }
    }
    
    public Color playerColor
    {
        get
        {
            return playerData.m_color;
        }
        set
        {
            playerData.m_color = value;
            m_color = value;
        }
    }

    public Dictionary<string, Level> allLevel = new Dictionary<string, Level>();

    //When first spawn
    private void Start()
    {
        gameState.Initiate(this);

        DontDestroyOnLoad(gameObject);


        //If the player has not connected before
        if (!GlobalNetworkManager.instance.AllPlayerData.ContainsKey(playerIPAddress))
        {
            Initialise();

            PlayerData newplayerdata = new PlayerData(playerData);

            //add it to the history of connected player to save persistence
            GlobalNetworkManager.instance.AllPlayerData.Add(newplayerdata.m_ipaddress, newplayerdata);
        }
        else
        {
            //Otherwise load the player with their saved data
            playerData = new PlayerData(GlobalNetworkManager.instance.AllPlayerData[playerIPAddress]);
        }


        GlobalNetworkManager.instance.GetPlayerData(playerData);
        
        if (isLocalPlayer)
        {
            CmdChangeIPAddress(IPManager.GetLocalIPAddress());
        }

        Debug.Log("After : " + playerColor);

        if (isServer) {
            RoomSystem.instance.AddPlayer(this);
            return;
        }
        
        SceneManager.LoadScene("Game");
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                CmdChangeName("NICE");
            }
        }
    }

    public void Initialise()
    {
        CmdChangeColor(new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
    }

    [Command]
    void CmdChangeName(string newName)
    {
        playerName = newName;
        GlobalNetworkManager.instance.SetPlayerData(playerData);
    }

    [Command]
    void CmdChangeIPAddress(string ipaddress)
    {
        playerIPAddress = ipaddress;
        GlobalNetworkManager.instance.SetPlayerData(playerData);
    }

    [Command]
    void CmdChangeColor(Color newColor)
    {
        playerColor = newColor;
        GlobalNetworkManager.instance.SetPlayerData(playerData);
    }
}

public class PlayerData
{
    [SyncVar]
    public string m_name;
    [SyncVar]
    public string m_ipaddress;
    [SyncVar]
    public Color m_color;

    public PlayerData()
    {
        m_name = "";
        m_ipaddress = IPManager.GetLocalIPAddress();
        m_color = new Color(0, 0, 0);
    }

    public PlayerData(PlayerData playerdata)
    {
        m_name = playerdata.m_name;
        m_ipaddress = playerdata.m_ipaddress;
        m_color = playerdata.m_color;
    }
}