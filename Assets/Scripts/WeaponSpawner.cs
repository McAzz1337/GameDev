using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

public class WeaponSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private float spawnDelay;

    private Weapon spawnedWeapon;

    Random random;


    void Awake()
    {
        random = new Random();
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

        if (spawnedWeapon == null) return;

        int layer = 1 << collider.gameObject.layer;

        if (layer == LayerMask.GetMask("Player"))
        {

            Player player = collider.gameObject.GetComponent<Player>();

            if (spawnedWeapon as MolotovCocktailMock != null && player.canPickupMolotov())
            {

                player.pickupWeaponClientRpc(spawnedWeapon);
                spawnedWeapon = null;
            }
            else if (player.canPickupWeapon())
            {

                player.pickupWeaponClientRpc(spawnedWeapon);
                spawnedWeapon = null;
            }

            StartCoroutine("spawnWeaponTimed");
        }
    }

    public IEnumerator spawnWeaponTimed()
    {

        yield return new WaitForSeconds(spawnDelay);

        spawnWeapon();

    }

    public void spawnWeapon()
    {

        int index = random.Next() % weaponPrefabs.Length;

        GameObject g = Instantiate(weaponPrefabs[index], transform);
        spawnedWeapon = g.GetComponent<Weapon>();
    }
}
