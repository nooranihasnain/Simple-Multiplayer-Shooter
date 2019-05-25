using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    private NetworkManager netManager;
    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private GameObject RoomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

	// Use this for initialization
	void Start () {
        netManager = NetworkManager.singleton;
        if(netManager.matchMaker == null)
        {
            netManager.StartMatchMaker();
        }
        RefreshRoomList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RefreshRoomList()
    {
        netManager.matchMaker.ListMatches(0, 20, "",false,0,0, OnMatchList);
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot>matches)
    {
        if(matches == null)
        {
            return;
        }
        ClearRoomList();
        foreach(MatchInfoSnapshot match in matches)
        {
            GameObject RoomListItemGO = Instantiate(RoomListItemPrefab);
            RoomListItemGO.transform.SetParent(roomListParent);
            RoomListItem _roomListItem = RoomListItemGO.GetComponent<RoomListItem>();
            if(_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }
            roomList.Add(RoomListItemGO);
        }
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {
        netManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, netManager.OnMatchJoined);
    }
}
