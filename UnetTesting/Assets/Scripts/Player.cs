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
    
    //When first spawn
    private void Start()
    {
        gameState.Initiate(this);

        DontDestroyOnLoad(gameObject);

        //Setup the player and check whether or not it's new or been in the room before
        //CmdInitialize();
        
        //If object is local, tell the server to give it its value
        if (isLocalPlayer)
        {
            CmdSetupNetworkPresence(IPManager.GetLocalIPAddress());
        }
    
        //If you are server, then run
        if (isServer) {
            RoomSystem.instance.AddPlayer(this);
            return;
        }
        
        //If you are not the server, then load a new scene
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

            //SHIFT CURRENT LEVEL -1
            if (Input.GetKeyDown(KeyCode.Q))
            {

            }
            //SHIFT CURRENT LEVEL -1
            if (Input.GetKeyDown(KeyCode.E))
            {

            }
            //ADD TIME
            if (Input.GetKeyDown(KeyCode.I))
            {

            }
            //ADD TRIES
            if (Input.GetKeyDown(KeyCode.O))
            {

            }
            //ADD FIRST TRY SCORE
            if (Input.GetKeyDown(KeyCode.P))
            {

            }
        }
    }

    //Use command cuz only the server will check
    [Command]
    public void CmdInitialize()
    {
        Debug.Log(playerIPAddress + " NAME DURING INITIALIZING " + playerName);

        //If the player has not connected before
        if (!GlobalNetworkManager.instance.AllPlayerData.ContainsKey(playerIPAddress))
        {
            //CmdChangeColor(new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
            playerColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

            playerName = "DefaultName";
            
            //Create a new temp to copy the data
            PlayerData newplayerdata = new PlayerData(playerData);
            
            //add temp to the history of connected player to save persistence
            GlobalNetworkManager.instance.AllPlayerData.Add(newplayerdata.m_ipaddress, newplayerdata);

            //If the there is no duplicate of ip on the GlobalNetworkManager
            if (!GlobalNetworkManager.instance.allPlayerIPAddress.Contains(playerIPAddress))
            {
                //Add the ip into it
                GlobalNetworkManager.instance.allPlayerIPAddress.Add(playerIPAddress);
            }

            Debug.Log(m_ipaddress + " JUST CONNECTED " + playerName);
        }
        else
        {
            //Connected before
            Debug.Log(m_ipaddress + " RECONNECTED " + playerName);
            
            //Otherwise load the player with their saved data
            playerData = new PlayerData(GlobalNetworkManager.instance.AllPlayerData[playerIPAddress]);
        }

        Debug.Log(playerIPAddress + " NAME AFTER INITIALIZING " + playerName);
    }

    [Command]
    void CmdChangeName(string newName)
    {
        playerName = newName;
        Debug.Log(playerIPAddress + " CHANGE NAME TO " + playerName);
        GlobalNetworkManager.instance.SetPlayerData(playerData);
    }

    [Command]
    void CmdSetupNetworkPresence(string ipaddress)
    {
        playerIPAddress = ipaddress;

        Debug.Log(playerIPAddress + " NAME BEFORE INITIALIZING " + playerName);

        CmdInitialize();
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

    public Dictionary<string, Level> allLevel = new Dictionary<string, Level>();

    [SyncVar]
    public int CurrentLevel = 0;

    public PlayerData()
    {
        m_name = "Default_Name";
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