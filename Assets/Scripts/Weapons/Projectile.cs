using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{

    [SerializeField] private ulong clientID;
    void Start()
    {

    }

    void Update()
    {

    }


    public ulong getClientID()
    {

        return clientID;
    }

    public void setClientID(ulong clientID)
    {

        this.clientID = clientID;
    }
}
