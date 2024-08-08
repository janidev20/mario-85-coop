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
    [SerializeField] LayerMask JumpSmaller;
    [SerializeField] LayerMask JumpDanger; // When MX is too close

    [Header("Control")]
    [SerializeField] private bool testInput = false;
    public bool moveLeft;
    public bool moveRight;
    public bool run;
    public bool RUNLEFT;
    public bool dodgedMX = false;

    private void Start()
    {
        RUNLEFT = !RUNLEFT;

        movementScript = GetComponent<LucasController>();
    }

    private void Update()
    {
        if (LucasController.LucasIsDead)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RUNLEFT = !RUNLEFT;
        }

        if (RUNLEFT)
        {
            moveLeft = true;
            run = true;
        }
        else
        {
            moveLeft = false;
            run = false;
        }


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
        }
        else
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
        }
        else
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("GoRight"))
        {
            moveRight = true;
            RUNLEFT = false;
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("JumpDangerRight"))
        {
            if (movementScript.onGround)
            {


                if (collision.gameObject.GetComponentInParent<PlayerAnimation>().isPCrawler)
                {
                    if (collision.gameObject.GetComponentInParent<PlayerMovement>().isMoving)
                    {
                        if (!collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                        {
                            RUNLEFT = false;
                            moveRight = false;
                            movementScript.JumpDanger(movementScript.jumpSmall * 1.35f);
                        }
                        else if (collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                        {
                            RUNLEFT = true;
                            moveRight = false;
                        }

                    }
                    else
                    {
                        RUNLEFT = false;
                        moveRight = false;
                    }
                }
                else if (collision.gameObject.GetComponentInParent<PlayerAnimation>().isMX)
                {
                    if (collision.gameObject.GetComponentInParent<PlayerMovement>().isMoving)
                    {
                        if (!collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                        {
                            RUNLEFT = false;
                            moveRight = true;
                            movementScript.JumpDanger(movementScript.jumpBig);
                        }
                        else if (collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                        {
                            RUNLEFT = false;
                            moveRight = false;
                        }
                    }
                    else
                    {
                        RUNLEFT = true;
                        moveRight = false;
                    }
                }
                else
                {
                    RUNLEFT = true;
                    moveRight = false;
                }


            } else
            {
           
            if (collision.gameObject.GetComponentInParent<PlayerMovement>().isMoving)
            {
                if (!collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                {
                    RUNLEFT = true;
                    moveRight = false;
                }
                else if (collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                {
                    RUNLEFT = false;
                    moveRight = true;
                }
            }
            else
            {
                RUNLEFT = false;
                moveRight = false;
            
    }
            }
        }


        else if (collision.gameObject.layer == LayerMask.NameToLayer("JumpDangerLeft"))
        {
            if (collision.gameObject.GetComponentInParent<PlayerAnimation>().isPCrawler)
            {
                if (movementScript.onGround)
                {

                        
                        if (collision.gameObject.GetComponentInParent<PlayerMovement>().isMoving)
                        {
                            if (!collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                            {
                                RUNLEFT = false;
                                moveRight = false;
                                movementScript.JumpDanger(movementScript.jumpMiddle * 1.35f);
                            }
                            
                            else if (collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                            {
                                RUNLEFT = false;
                                moveRight = false;
                            }

                        }

                        else
                        {
                            RUNLEFT = true;
                            moveRight = false;
                        }

                } else
                {
                    if (collision.gameObject.GetComponentInParent<PlayerMovement>().isMoving)
                    {
                        if (!collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                        {
                            RUNLEFT = false;
                            moveRight = false;
                        }

                        else if (collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                        {
                            RUNLEFT = false;
                            moveRight = false;
                        }

                    }
                    else if (collision.gameObject.GetComponentInParent<PlayerAnimation>().isMX)
                    {
                        if (collision.gameObject.GetComponentInParent<PlayerMovement>().isMoving)
                        {
                            if (!collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                            {
                                RUNLEFT = true;
                            }
                            else if (collision.gameObject.GetComponentInParent<PlayerMovement>().isJumping)
                            {
                                RUNLEFT = true;
                                moveRight = false;
                            }
                        }
                        else
                        {
                            RUNLEFT = true;
                            moveRight = false;
                        }

                    }

                    
                } 

            }
            
        } else
        {
            RUNLEFT = true;
            moveRight = false;
        }


        if 
            
            (collision.gameObject.layer == LayerMask.NameToLayer("JumpBig"))
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

            if (collision.gameObject.layer == LayerMask.NameToLayer("JumpSmaller"))
            {
                movementScript.JumpSmaller();
            }
        
       
    }
}
