using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = System.Random;
public class WeaponFactory : MonoBehaviour
{

    public static WeaponFactory instance;

    public static Random random = new Random();

    [SerializeField] private GameObject[] weaponPrefabs;

    void Awake()
    {

        instance = this;


    }


    public static WeaponNetwork spawnWeaponOnNetwork(EWeapon identifier)
    {

        GameObject g = Instantiate(instance.weaponPrefabs[(int)identifier]);
        g.GetComponent<NetworkObject>().Spawn(true);

        return g.GetComponent<WeaponNetwork>();
    }



    public static GameObject spawnRandomWeaponOnNetwork()
    {

        int index = random.Next() % instance.weaponPrefabs.Length;

        GameObject g = Instantiate(instance.weaponPrefabs[index]);
        Debug.Log("weapon : " + g == null);
        g.GetComponent<NetworkObject>().Spawn(true);

        return g;
    }

}
