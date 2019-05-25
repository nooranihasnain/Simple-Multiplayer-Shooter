using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour {

    public delegate void JoinRoomDelegate(MatchInfoSnapshot match);
    private JoinRoomDelegate _jRoomCallback;
    [SerializeField]
    private Text RoomNameText;
    MatchInfoSnapshot match;

	public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback)
    {
        match = _match;
        RoomNameText.text = match.name + " ( " + match.currentSize + " / " + match.maxSize + " ) ";
        _jRoomCallback = _joinRoomCallback;
    }

    public void JoinRoom()
    {
        _jRoomCallback.Invoke(match);
    }
}
