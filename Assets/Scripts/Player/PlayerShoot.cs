using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    [SerializeField]
    private Camera cam; //Playercamera

    public PlayerWeapon weapon;
    private const string PLAYERTAG = "Player";

    [SerializeField]
    private ParticleSystem MuzzleFlash;
    [SerializeField]
    private GameObject HitParticlePrefab;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if(cam == null)
        {
            Debug.Log(" Player Shoot: No Camera Reference");
            this.enabled = false;
        }
    }

    void Update()
    {
        if(PauseMenuScript.IsOn)
        {
            return;
        }
        if(Input.GetButtonDown("Fire1"))
        {
            InvokeRepeating("Shoot", 0.1f, 0.1f);
        }
        else if(Input.GetButtonUp("Fire1"))
        {
            CancelInvoke("Shoot");
        }
    }

    //Is called on the server when we hit something
    [Command]
    void CmdOnHit(Vector3 _hitPos, Vector3 _normal)
    {
        RpcShowHit(_hitPos, _normal);
    }

    //Is called on the server when player shoots
    [Command]
    void CmdOnShoot()
    {
        RpcShowMuzzle();
    }

    [ClientRpc]
    void RpcShowHit(Vector3 hitPos, Vector3 normal)
    {
        Instantiate(HitParticlePrefab, hitPos, Quaternion.LookRotation(normal));
    }

    //Is called on all clients when we need to show flash particle
    [ClientRpc]
    void RpcShowMuzzle()
    {
        MuzzleFlash.Play();
    }

    [Client]
    private void Shoot()
    {
        if(!isLocalPlayer)
        {
            return;
        }
        //Calls this method on server
        CmdOnShoot();
        Debug.Log("Shoot");
        RaycastHit _hit;
        if(Physics.Raycast(cam.transform.position,cam.transform.forward, out _hit,weapon.range,mask))
        {
            if(_hit.collider.tag == PLAYERTAG)
            {
                CmdPlayerShot(_hit.collider.name, weapon.damage);
            }
            CmdOnHit(_hit.point, _hit.normal);
        }
    }
    //player hitting and damage taken
    [Command]
    void CmdPlayerShot(string PlayerID, int _damage)
    {
        Debug.Log(PlayerID + "has been shot");
        Player player = GameManager.GetPlayer(PlayerID);
        player.RpcTakeDamage(_damage);
    }
}
