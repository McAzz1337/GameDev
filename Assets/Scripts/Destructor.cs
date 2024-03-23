using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Destructor : NetworkBehaviour
{

    [SerializeField] private float duration = 1.0f;

    [SerializeField] private bool destructionByClientAllowed = false;
    void Start()
    {

        if (destructionByClientAllowed && !IsHost) return;

        StartCoroutine("destruct");
    }

    void Update()
    {

    }

    private IEnumerator destruct()
    {

        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }



    public void setDuration(float duration)
    {

        this.duration = duration;
    }

    public void setDestructableByClient(bool b)
    {

        destructionByClientAllowed = b;
    }
}
