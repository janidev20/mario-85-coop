using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucasAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private LucasController moveScript;

    void Start()
    {
        moveScript = GetComponent<LucasController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (moveScript.isMoving)
        {
            animator.SetBool("isMoving", true);
        } else
        {
            animator.SetBool("isMoving", false);
        }

        if (moveScript.isSliding)
        {
            animator.SetBool("isSliding", true);
        }
        else
        {
            animator.SetBool("isSliding", false);
        }

        if (moveScript.isJumping)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }
}
