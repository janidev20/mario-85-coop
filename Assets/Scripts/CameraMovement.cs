using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CameraMovement : NetworkBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform camera;
    [SerializeField] private float moveAmount;

    private void Update()
    {
        if (!IsOwner)
        {
            Destroy(this);
        }

        camera.position = new Vector3(player.position.x, camera.position.y, camera.position.z);
    }
}
