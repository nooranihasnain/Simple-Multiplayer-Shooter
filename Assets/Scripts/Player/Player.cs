using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead = false;
    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]
    private int CurrHealth;

    //Disable scripts and components
    [SerializeField]
    private Behaviour[] DisableOnDeath;

    [SerializeField]
    private GameObject[] DisableAllGameObjectsOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject DeathEffect;

    private bool FirstSetup = true;

	public void Setup () {
        if(isLocalPlayer)
        {
            //Disable Scene Camera and UI
            GameManager.singleton.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().PlayerUIInstance.SetActive(true);
        }
        //Sending a call to server to setup new player
        CmdBroadcastNewPlayerSetup();
	}
	
    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if(FirstSetup)
        {
            wasEnabled = new bool[DisableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = DisableOnDeath[i].enabled;
            }
            FirstSetup = false;
        }
        
        SetDefaults();
    }

    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    public void SetDefaults()
    {
        isDead = false;
        CurrHealth = maxHealth;
        //Enable scripts and components
        for (int i = 0; i < DisableOnDeath.Length; i++)
        {
            DisableOnDeath[i].enabled = wasEnabled[i];
        }
        //Enable gameobjects
        for (int i = 0; i < DisableAllGameObjectsOnDeath.Length; i++)
        {
            DisableAllGameObjectsOnDeath[i].SetActive(true);
        }
        //Enable collider
        Collider _col = GetComponent<Collider>();
        if(_col != null)
        {
            _col.enabled = true;
        }
        
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {
       if(isDead)
        {
            return;
        }
        CurrHealth -= _amount;

        if(CurrHealth <=0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        //Disable components
        for (int i = 0; i < DisableOnDeath.Length; i++)
        {
            DisableOnDeath[i].enabled = false;
        }
        //Disable All Gameobjects
        for (int i = 0; i < DisableAllGameObjectsOnDeath.Length; i++)
        {
            DisableAllGameObjectsOnDeath[i].SetActive(false);
        }
        //Disable collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }
        //Spawn DeathParticle
        Instantiate(DeathEffect, transform.position, Quaternion.identity);
        //Switch cameras on death
        if (isLocalPlayer)
        {
            GameManager.singleton.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().PlayerUIInstance.SetActive(false);
        }
        Debug.Log(transform.name + "is dead");
        //Respawn method
        StartCoroutine(Respawn());
        
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.singleton.matchSettings.RespawnTime);

        Transform _startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _startPoint.position;
        transform.rotation = _startPoint.rotation;

        yield return new WaitForSeconds(0.1f);


        Setup();
        Debug.Log(transform.name + "Respawned");
    }

	// Update is called once per frame
	//void Update () {
	//	if(!isLocalPlayer)
 //       {
 //           return;
 //       }
 //       if(Input.GetKeyDown(KeyCode.K))
 //       {
 //           RpcTakeDamage(200);
 //       }
	//}
}
