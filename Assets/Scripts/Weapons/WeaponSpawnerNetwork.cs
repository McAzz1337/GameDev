using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

using Random = System.Random;

public class WeaponSpawnerNetwork : NetworkBehaviour
{

    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private float delay;
    [SerializeField] private WeaponNetwork spawnedWeapon;

    Random random = new Random();

    void Awake()
    {




    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }


    public void acitvate()
    {


        gameObject.SetActive(true);
        spawnWeapon();
    }


    void Start()
    {

        if (!IsHost) return;

        spawnWeapon();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {

        checkPlayerPickup(collider);
    }


    void OnTriggerStay(Collider collider)
    {

        checkPlayerPickup(collider);
    }

    private void checkPlayerPickup(Collider collider)
    {



        if (spawnedWeapon == null) return;


        if (1 << collider.gameObject.layer == LayerMask.GetMask("Player"))
        {


            PlayerNetwork player = collider.gameObject.GetComponent<PlayerNetwork>();

            if (player.canPickupWeapon())
            {

                player.pickupWeapon(spawnedWeapon);
                spawnedWeapon.GetComponent<CapsuleCollider>().enabled = true;
                removeWeapon();
                StartCoroutine("spawnWeaponTimed");
            }

        }


    }



    public void removeWeapon()
    {

        spawnedWeapon = null;
    }

    public IEnumerator spawnWeaponTimed()
    {

        yield return new WaitForSeconds(delay);

        spawnWeapon();
    }

    [ServerRpc]
    public void spawnWeaponServerRpc()
    {

        spawnWeapon();
    }

    private void spawnWeapon()
    {


        int index = random.Next() % weaponPrefabs.Length;


        GameObject g = Instantiate(weaponPrefabs[index]);
        g.GetComponent<NetworkObject>().Spawn(true);
        spawnedWeapon = g.GetComponent<WeaponNetwork>();
        g.transform.position = transform.position;

        g.GetComponent<CapsuleCollider>().enabled = false;

        MeshCollider mc = g.GetComponentInChildren<MeshCollider>();
        if (mc != null)
        {
            mc.enabled = false;
        }
    }
}
