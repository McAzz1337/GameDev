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

        gameObject.SetActive(false);
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


        Debug.Log("check on  client");

        if (spawnedWeapon == null) return;

        Debug.Log("not null");

        if (1 << collider.gameObject.layer == LayerMask.GetMask("Player"))
        {

            Debug.Log("player collided");

            PlayerNetwork player = collider.gameObject.GetComponent<PlayerNetwork>();

            if (player.canPickupWeapon())
            {

                Debug.Log("can pick up");
                player.pickupWeapon(spawnedWeapon);
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

    private void spawnWeapon()
    {

        int index = random.Next() % weaponPrefabs.Length;


        GameObject g = Instantiate(NetworkManager.GetNetworkPrefabOverride(weaponPrefabs[index]));
        g.GetComponent<NetworkObject>().Spawn();
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
