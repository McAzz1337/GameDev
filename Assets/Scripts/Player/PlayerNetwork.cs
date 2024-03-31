using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerNetwork : NetworkBehaviour
{

    public static PlayerNetwork localPlayer;

    [Header("References")]
    [SerializeField] private Transform weaponTransform;

    [Header("Debug Info")]
    [SerializeField] private bool isHost;
    [SerializeField] private bool isOwner;
    [SerializeField] private bool isClient;
    [SerializeField] private WeaponNetwork weapon;
    [SerializeField]
    private NetworkVariable<EWeapon> weaponType =
                            new NetworkVariable<EWeapon>(
                                EWeapon.NONE,
                                NetworkVariableReadPermission.Everyone,
                                NetworkVariableWritePermission.Server);


    private NetworkVariable<Vector2> moveInput =
        new NetworkVariable<Vector2>(Vector2.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);


    Rigidbody rb;

    PlayerControls controls;

    public delegate void UseCallback();
    private UseCallback onUse;

    void Awake()
    {

        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
    }



    public void enableRenderer()
    {

        enableRendererClientRpc();
    }

    [ClientRpc]
    public void enableRendererClientRpc()
    {

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = true;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    public void disableRenderer()
    {

        disableRendererClientRpc();
    }

    [ClientRpc]
    public void disableRendererClientRpc()
    {

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = false;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

    }

    void Start()
    {

        if (IsOwner)
        {

            localPlayer = this;
            GetComponent<IDHolder>().setClientID(NetworkManager.Singleton.LocalClientId);
        }

        isHost = IsHost;
        isOwner = IsOwner;
        isClient = IsClient;

    }




    public void shoot()
    {


        if (weapon == null) return;

        weapon.shoot();

        if (weapon.isEmpty())
        {

            dropWeapon();
        }
        else if (weapon as MolotovCocktailMock != null)
        {

            weapon = null;
        }
    }



    public void pickupWeapon(WeaponNetwork weapon)
    {

        this.weapon = weapon;
        weaponType.Value = weapon.getIdentifier();
        weapon.transform.position = weaponTransform.position;
        weapon.transform.rotation = weaponTransform.rotation;
        weapon.transform.SetParent(transform);

        ulong clientID = GetComponent<IDHolder>().getClientID();
        Debug.Log("ID ON PICKUP: " + clientID);
        weapon.GetComponent<IDHolder>().setClientID(clientID);
    }




    public void dropWeapon()
    {

        weapon.drop();
        weapon = null;
    }


    public bool canPickupWeapon()
    {

        return weapon == null;
    }

}
