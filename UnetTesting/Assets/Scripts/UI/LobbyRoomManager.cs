using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRoomManager : MonoBehaviour {
    public static LobbyRoomManager instance;

    private List<RoomStatBtn> allRoomButtons = new List<RoomStatBtn>();

    [SerializeField]
    private Transform LobbyRoomContentTransform;

    public RoomStatBtn defaultRoomButton;

    //For other classes to use, will return all rooms and seperate duplicates
    public List<LanConnectionInfo> AllRooms
    {
        set
        {
            int i = 0;
            for (i = 0; i < allRoomButtons.Count; i++)
            {
                Destroy(allRoomButtons[i].gameObject);
            }

            allRoomButtons = new List<RoomStatBtn>();
            
            foreach(LanConnectionInfo connectionInfo in value)
            {
                GameObject newRoomButton = Instantiate(defaultRoomButton.gameObject, LobbyRoomContentTransform);
                newRoomButton.GetComponent<RoomStatBtn>().UpdateText(connectionInfo);

                allRoomButtons.Add(newRoomButton.GetComponent<RoomStatBtn>());
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void RefreshRoom(LanConnectionInfo roomInfo)
    {

    }
}
