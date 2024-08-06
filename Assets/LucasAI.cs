using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LucasController))]

public class LucasAI : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] LucasController movementScript;

    [Header("Layers")]
    [SerializeField] LayerMask JumpBig;
    [SerializeField] LayerMask JumpMiddle;
    [SerializeField] LayerMask JumpSmall;
    [SerializeField] LayerMask JumpDanger; // When MX is too close

    [Header("Control")]
    [SerializeField] private bool testInput = true;
    public bool moveLeft;
    public bool moveRight;
    public bool run;


    private void Start()
    {
        movementScript = GetComponent<LucasController>();
    }

    private void Update()
    {
        ManageMovement();
        CheckForInput(testInput);
    }
    
    void ManageMovement()
    {
        if (moveLeft)
        {
            movementScript.direction.x = -1;
        }

        else if (moveRight)
        {
            movementScript.direction.x = 1;
        }

        else
        {
            movementScript.direction.x = 0;
        }
    
        if (run)
        {
            movementScript.isRunning = true;
        } else
        {
            movementScript.isRunning = false;
        }
    }

    void CheckForInput(bool enabled)
    {
        if (!enabled)
            return;

        if (Input.GetKey(KeyCode.C))
        {
            run = true;
        } else
        {
            run = false;
        }

        if (Input.GetKey(KeyCode.H))
        {
            moveLeft = true;
            moveRight = false;
        }

        else if (Input.GetKey(KeyCode.K))
        {
            moveLeft = false;
            moveRight = true;
        }
        else
        {
            moveLeft = false;
            moveRight = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("JumpBig"))
        {
            movementScript.JumpBig();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("JumpMiddle"))
        {
            movementScript.JumpMiddle();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("JumpSmall"))
        {
            movementScript.JumpSmall();
        }
    }
}
