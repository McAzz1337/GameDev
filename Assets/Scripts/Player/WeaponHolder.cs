using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponHolder : NetworkBehaviour
{

    [SerializeField] private Transform weaponTransform;
    [SerializeField] private WeaponNetwork weapon;
    [SerializeField]
    private NetworkVariable<EWeapon> weaponType =
                            new NetworkVariable<EWeapon>(
                                EWeapon.NONE,
                                NetworkVariableReadPermission.Everyone,
                                NetworkVariableWritePermission.Server);

    public void pickupWeapon(WeaponNetwork weapon)
    {

        this.weapon = weapon;
        weaponType.Value = weapon.getIdentifier();
        weapon.transform.position = weaponTransform.position;
        weapon.transform.rotation = weaponTransform.rotation;
        weapon.transform.SetParent(transform);

        ulong clientID = GetComponent<IDHolder>().getClientID();
        weapon.GetComponent<IDHolder>().setClientID(clientID);
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


    public void dropWeapon()
    {

        weapon?.drop();
        weapon = null;
    }


    public bool canPickupWeapon()
    {

        return weapon == null;
    }
}
