using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint RoomSize = 6;
    private string RoomName;
    private NetworkManager netManager;

    void Start()
    {
        netManager = NetworkManager.singleton;
        if(netManager.matchMaker == null)
        {
            netManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string _name)
    {
        RoomName = _name; 
    }

    public void CreateRoom()
    {
        if(RoomName != "" && RoomName != null)
        {
            Debug.Log("Creating Room: " + RoomName + " with size " + RoomSize);
            //CreateRoom
            netManager.matchMaker.CreateMatch(RoomName, RoomSize, true, "", "", "", 0, 0, netManager.OnMatchCreate);
        }
    }
}
