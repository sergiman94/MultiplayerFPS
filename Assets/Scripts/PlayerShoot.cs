﻿
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player"; 

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Awake()
    {
        GameObject gameObject = new GameObject("PlayerWeapon");
        weapon = gameObject.AddComponent<PlayerWeapon>();
    }

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced");
            this.enabled = false;
        }
       
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }    
    }

    [Client]
    void Shoot()
    {
        RaycastHit _hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            // revisamos si le ha disparado a un jugador y no a otra cosa
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, weapon.damage);

                Debug.Log(_hit.collider.name);
            }
            

            
        }
    }

    [Command] // command para que unity sepa que es en el servidor que esto ocurre
    void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + "has been shot");
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }

    
}
