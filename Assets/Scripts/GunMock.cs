using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMock : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform muzzle;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void shoot()
    {

        GameObject g = Instantiate(bulletPrefab);

        g.transform.position = muzzle.position;
        g.transform.rotation = muzzle.rotation;
    }
}
