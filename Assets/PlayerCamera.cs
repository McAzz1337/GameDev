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
        if (!IsOwner) return;
        
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        if (!IsGameScene()) return;
        cameraHolder.SetActive(IsOwner);
        cameraHolder.transform.position = transform.position + offset;
        cameraHolder.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private static bool IsGameScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return sceneName is "Map_001" or "Map_002" or "Map_003" or "Map_004" or "Map_005";
    }
}
