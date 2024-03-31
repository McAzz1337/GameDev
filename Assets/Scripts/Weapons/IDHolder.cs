using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class IDHolder : NetworkBehaviour
{

    [SerializeField]
    private NetworkVariable<ulong> clientID =
                            new NetworkVariable<ulong>(
                                ulong.MaxValue,
                                NetworkVariableReadPermission.Everyone,
                                NetworkVariableWritePermission.Owner
                            );

    public ulong getClientID()
    {

        return clientID.Value;
    }

    public void setClientID(ulong clientID)
    {

        this.clientID.Value = clientID;
    }
}
