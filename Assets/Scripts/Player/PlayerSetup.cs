using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] ComponentsToDisable;

    [SerializeField]
    GameObject PlayerUIPrefab;

    [HideInInspector]
    public GameObject PlayerUIInstance;

    [SerializeField]
    string RemoteLayerName = "RemotePlayer";

   
	// Use this for initialization
	void Start () {
		if(!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            PlayerUIInstance = Instantiate(PlayerUIPrefab);
            PlayerUIInstance.name = PlayerUIPrefab.name;
            GetComponent<Player>().Setup();
        }
	}

    //Calls when client joins the server
    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Disables components of other player except our own
    void DisableComponents()
    {
        for (int i = 0; i < ComponentsToDisable.Length; i++)
        {
            ComponentsToDisable[i].enabled = false;
        }
    }

    //Assigns a remote layer name to every player instance except our own
    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(RemoteLayerName);
    }

    /// <summary>
    /// Calls when the player is leaving the server, it disables the components and unregisters the player
    /// </summary>
    void OnDisable()
    {
        Destroy(PlayerUIInstance);
        if(isLocalPlayer)
        {
            GameManager.singleton.SetSceneCameraActive(true);
        }
        GameManager.UnregisterPlayer(transform.name);
    }
}
