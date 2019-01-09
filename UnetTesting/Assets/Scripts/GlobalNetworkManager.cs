using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GlobalNetworkManager : UnityEngine.Networking.NetworkManager {
    public static GlobalNetworkManager instance;
    NetworkClient myClient;
    
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
            RoomSystem.instance.RefreshUser();
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
            RoomSystem.instance.RefreshUser();
        }
    }
}
