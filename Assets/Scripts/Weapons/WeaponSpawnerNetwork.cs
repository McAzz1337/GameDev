using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

using Random = System.Random;

public class WeaponSpawnerNetwork : NetworkBehaviour
{

    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private float delay;

    Random random;

    [SerializeField] private WeaponNetwork spawnedWeapon;

    void Awake()
    {

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsHost)
        {

            spawnWeapon();
        }
    }

    void Start()
    {

        random = new Random();

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

        if (!IsHost)
        {
            Debug.Log("IsNotHost");
            return;
        }



        if (spawnedWeapon == null) return;

        int layer = 1 << collider.gameObject.layer;

        if (layer == LayerMask.GetMask("Player"))
        {

            PlayerNetwork player = collider.gameObject.GetComponent<PlayerNetwork>();

            if (player.canPickupWeapon())
            {

                player.pickupWeapon(spawnedWeapon);
                removeWeapon();
            }

            StartCoroutine("spawnWeaponTimed");
        }
    }

    public void removeWeapon()
    {

        removeWeaponClientRpc();

        spawnedWeapon = null;
    }

    [ClientRpc]
    public void removeWeaponClientRpc()
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

        GameObject g = Instantiate(weaponPrefabs[index], transform);
        spawnedWeapon = g.GetComponent<WeaponNetwork>();
        g.GetComponent<NetworkObject>().Spawn();

        spawnedWeaponClientRpc(index);
    }

    [ClientRpc]
    private void spawnedWeaponClientRpc(int index)
    {

        //GameObject g = (GameObject)Instantiate((weaponPrefabs[index]), transform);
        //spawnedWeapon = g.GetComponent<WeaponNetwork>();
    }

}
