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
        }
    }

    public Dictionary<string, Level> allLevel = new Dictionary<string, Level>();

    //When first spawn
    private void Start()
    {
        gameState.Initiate(this);

        DontDestroyOnLoad(gameObject);

        if(isLocalPlayer)
        CmdChangeIPAddress(IPManager.GetLocalIPAddress());

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
        GlobalNetworkManager.instance.UpdatePlayerData(playerData);
        
        if (RoomSystem.instance)
        {
            RoomSystem.instance.RefreshScoreboard();
        }
    }

    [Command]
    void CmdChangeIPAddress(string ipaddress)
    {
        playerData.m_ipaddress = ipaddress;
        GlobalNetworkManager.instance.UpdatePlayerData(playerData);

        if (RoomSystem.instance)
        {
            RoomSystem.instance.RefreshScoreboard();
        }
    }

    [Command]
    void CmdChangeColor(Color newColor)
    {
        playerData.m_color = newColor;
        GlobalNetworkManager.instance.UpdatePlayerData(playerData);

        if (RoomSystem.instance)
        {
            RoomSystem.instance.RefreshScoreboard();
        }
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