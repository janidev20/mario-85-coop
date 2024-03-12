using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerMovement plyMovement;
    [SerializeField] Animator playerAnimator;


    private void Update()
    {
        if (plyMovement.isRunning)
        {
            playerAnimator.SetBool("isRunning", true);
        } else
        {
            playerAnimator.SetBool("isRunning", false);
        }

        if (plyMovement.isJumping)
        {
            playerAnimator.SetBool("isJumping", true);
        } else
        {
            playerAnimator.SetBool("isJumping", false);
        }

        if (plyMovement.isSliding)
        {
            playerAnimator.SetBool("isSliding", true);
        } else
        {
            playerAnimator.SetBool("isSliding", false);
        }
    }
}
