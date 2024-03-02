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

        int layer = 1 << collider.gameObject.layer;

        if (layer == LayerMask.GetMask("Player"))
        {

            Player player = collider.gameObject.GetComponent<Player>();
            player.pickupWeapon(spawnedWeapon);

            spawnedWeapon = null;
            StartCoroutine("spawnWeaponTimed", spawnDelay);
        }
    }

    public IEnumerator spawnWeaponTimed(float delay)
    {

        yield return new WaitForSeconds(delay);

        spawnWeapon();

    }

    public void spawnWeapon()
    {

        int index = random.Next() % weaponPrefabs.Length;

        GameObject g = Instantiate(weaponPrefabs[index], transform);
        spawnedWeapon = g.GetComponent<Weapon>();
    }
}
