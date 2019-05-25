using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenuScript : MonoBehaviour {

    [SerializeField]
    public GameObject PauseMenuPanel;
    private NetworkManager _netManager;
    public static bool IsOn;

	// Use this for initialization
	void Start () {
        IsOn = false;
        _netManager = NetworkManager.singleton;
	}
	
	// Update is called once per frame
	void Update () {
		if(PauseMenuPanel != null && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
	}

    void TogglePauseMenu()
    {
        PauseMenuPanel.SetActive(!PauseMenuPanel.activeSelf);
        IsOn = PauseMenuPanel.activeSelf;
    }

    public void LeaveRoom()
    {
        MatchInfo matchInfo = _netManager.matchInfo;
        _netManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, _netManager.OnDropConnection);
        _netManager.StopHost();
    }
}
