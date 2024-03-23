using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IDHolder
{

    public ulong getClientID();
    public void setClientID(ulong clientID);
}
