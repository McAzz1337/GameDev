using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponFactory : MonoBehaviour
{

    public static WeaponFactory instance;

    private static string[] weaponPrefabNames =  {

        "WeaponsNetwork/Bazooka",
        "WeaponsNetwork/MolotovCocktail"
    };

    [SerializeField] private GameObject[] weaponPrefabs;

    void Awake()
    {

        instance = this;

        weaponPrefabs = new GameObject[weaponPrefabNames.Length];

        for (int i = 0; i < weaponPrefabNames.Length; i++)
        {

            weaponPrefabs[i] = Resources.Load(weaponPrefabNames[i]) as GameObject;
        }
    }


    public static GameObject createWeapon(EWeapon identifier)
    {

        return Instantiate(instance.weaponPrefabs[(int)identifier]);
    }

    public static GameObject spawnWeaponOnNetwork(EWeapon identifier)
    {

        GameObject g = Instantiate(instance.weaponPrefabs[(int)identifier]);
        g.GetComponent<NetworkObject>().Spawn();

        return g;
    }

}
