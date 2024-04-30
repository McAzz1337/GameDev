using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Vector3 offset;

    public override void OnNetworkSpawn()
    {
        cameraHolder.SetActive(IsOwner);
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Map_001" ||
            SceneManager.GetActiveScene().name == "Map_002" ||
            SceneManager.GetActiveScene().name == "Map_003" ||
            SceneManager.GetActiveScene().name == "Map_004" ||
            SceneManager.GetActiveScene().name == "Map_005")
        {
            cameraHolder.transform.position = transform.position + offset;
            cameraHolder.transform.rotation = Quaternion.Euler(42, 0, 0);
        }
    }
}
