using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform camera;
    [SerializeField] private float moveAmount;

    private void Update()
    {
        // Camera follows the X position of the player. 
        camera.position = new Vector3(player.position.x, camera.position.y, camera.position.z);
    }
}
