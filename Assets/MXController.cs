using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MXController : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] PlayerMovement moveScript;

    [Header("IntroThingies")]
    public bool moveLeft = false;
    public bool cantMove = false;


    private void Update()
    {
        if (moveLeft)
        {
            moveScript.direction.x = -1;
        } else
        {
            moveScript.direction.x = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stop"))
        {
            cantMove = true;
        } else
        {
            cantMove = false;
        }
    }
}
