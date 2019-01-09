using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class MyNetworkDiscovery : NetworkDiscovery {
    public static MyNetworkDiscovery instance;

    private float timeout = 1;
    private Dictionary<LanConnectionInfo, float> lanAddresses = new Dictionary<LanConnectionInfo, float>();

    // Get the Address as a list instead of dictionary
    public List<LanConnectionInfo> LanAddresses
    {
        get
        {
            //List<LanConnectionInfo> connectionList = new List<LanConnectionInfo>();
            Dictionary<string, LanConnectionInfo> connectionList = new Dictionary<string, LanConnectionInfo>();

            foreach (LanConnectionInfo connInfo in lanAddresses.Keys.ToList())
            {
                if (connectionList.ContainsKey(connInfo.ipAddress))
                {
                    continue;
                } 
                connectionList.Add(connInfo.ipAddress, connInfo);
            }

            return connectionList.Values.ToList();
        }
    }

    private void Awake()
    {   
        base.Initialize();
        StartListen();
        StartCoroutine(CleanupExpiredEntries());

        instance = this;
    }

    /// <summary>
    /// Broadcast the server
    /// </summary>
    public void StartBroadcast()
    {
        StopBroadcast();
        base.Initialize();
        base.StartAsServer();
    }
    
    /// <summary>
    /// Start listening instead of broadcasting
    /// </summary>
    public void StartListen()
    {
        StopBroadcast();
        base.Initialize();
        base.StartAsClient();
    }

    /// <summary>
    /// Refresh the entry for receiving broadcast    
    /// </summary>
    IEnumerator CleanupExpiredEntries()
    {
        while (true)
        {
            var keys = lanAddresses.Keys.ToList();

            foreach(var key in keys)
            { 
                if (lanAddresses[key] <= Time.time)
                {
                    lanAddresses.Remove(key);
                    
                    UpdateMatchInfos();
                }
            } //End check all IP

            yield return new WaitForSeconds(timeout);
        }
    }

    /// <summary>
    /// Update the match info, the lobbymanager will use this
    /// </summary>
    public void UpdateMatchInfos()
    {
        //foreach (LanConnectionInfo lanInfo in LanAddresses)
        //{
        //    Debug.Log(lanInfo.ipAddress);
        //}

        //Debug.Log(LanAddresses.Count);

        if (LobbyRoomManager.instance)
        {
            LobbyRoomManager.instance.AllRooms = LanAddresses;
        }
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);

        string ip = fromAddress.Substring(fromAddress.LastIndexOf(":") + 1, fromAddress.Length - (fromAddress.LastIndexOf(":") + 1));

        LanConnectionInfo info = new LanConnectionInfo(ip, data);

        if (lanAddresses.ContainsKey(info) == false)
        {
            lanAddresses.Add(info, Time.time + timeout);
            UpdateMatchInfos();
        }
        else
        {
            lanAddresses[info] = Time.time + timeout;
        }
    }
}

public class LanConnectionInfo
{
    public string ipAddress;
    public int port;
    public string name;

    public LanConnectionInfo(string fromAddress, string data)
    {
        ipAddress = fromAddress.Substring(fromAddress.LastIndexOf(":") + 1, fromAddress.Length - (fromAddress.LastIndexOf(":") + 1));
        string portText = data.Substring(fromAddress.LastIndexOf(":") + 1, data.Length - (data.LastIndexOf(":") + 1));
        port = 7777;
        int.TryParse(portText, out port);
        name = "local";
    }
}

public static class IPManager
{
    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
}
