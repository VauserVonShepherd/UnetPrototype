using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomStatBtn : MonoBehaviour {
    public string room_ipAddress;

    public Text roomNameTxt, roomSceneTxt, studentAmountTxt, typeTxt;
    
    /// <summary>
    /// Refreshes the text to the stat of the room, mostly used when spawned on room update
    /// </summary>
    public void UpdateText(LanConnectionInfo connectionInfo)
    {
        room_ipAddress = connectionInfo.ipAddress;
        roomNameTxt.text = connectionInfo.ipAddress;
        roomSceneTxt.text = "Default";
    }

    /// <summary>
    /// Client Joins the room using room_ipaddress
    /// </summary>
    public void Btn_JoinRoom()
    {
        GlobalNetworkManager.instance.ConnectToServer(room_ipAddress);
    }
}
