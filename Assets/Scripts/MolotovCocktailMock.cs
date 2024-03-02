using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovCocktailMock : MonoBehaviour
{
    [SerializeField] private GameObject molotovPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void shoot()
    {
        GameObject g = Instantiate(molotovPrefab, transform.position, transform.rotation);
        Rigidbody rb = g.GetComponent<Rigidbody>();
        Vector3 throwDirection = transform.forward + transform.up;
        float throwForce = 10f;
        rb.AddForce(throwDirection.normalized * throwForce, ForceMode.VelocityChange);
    }
}
