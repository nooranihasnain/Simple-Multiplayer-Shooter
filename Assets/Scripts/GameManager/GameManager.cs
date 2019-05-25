using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager singleton;
    [SerializeField]
    private GameObject Scenecam;
    public MatchSettings matchSettings;

    void Awake()
    {
        if(singleton != null)
        {
            Debug.LogError("More than 1 gamemanger in the scene");
        }
        else
        {
            singleton = this; 
        }
    }

    public void SetSceneCameraActive(bool IsActive)
    {
        if(Scenecam == null)
        {
            return;
        }
        Scenecam.SetActive(IsActive);
    }

    #region Player Tracking


    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private const string PLAYER_ID_PREFIX = "Player";

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnregisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    /* void OnGUI()
     {
         GUILayout.BeginArea(new Rect(200, 200, 200, 500));
         GUILayout.BeginVertical();

         foreach(string _playerID in players.Keys)
         {
             GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
         }

         GUILayout.EndVertical();
         GUILayout.EndArea();
     }*/
    #endregion

}
