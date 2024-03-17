using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerMovement plyMovement;
    [SerializeField] Animator playerAnimator;

    [SerializeField] Animator MXAnimator, PCAnimator, FHAnimator;
    [SerializeField] private GameObject MX_Body, PCrawler_Body, isFH_Body;

    [SerializeField] private bool isMX, isPCrawler, isFH;


    private void Update()
    {
        AnimatorBooleans();
        CharacterMethod();
     
        if (Input.GetKeyDown(KeyCode.F1) && !isMX)
        {
            isMX = true;
            isPCrawler = false;
        } else if (Input.GetKeyDown(KeyCode.F2) && !isPCrawler)
        {
            isPCrawler = true;
            isMX = false;
        }
    }

    void AnimatorBooleans()
    {
        if (plyMovement.isRunning)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }

        if (plyMovement.isJumping)
        {
            playerAnimator.SetBool("isJumping", true);
        }
        else
        {
            playerAnimator.SetBool("isJumping", false);
        }

        if (plyMovement.isSliding)
        {
            playerAnimator.SetBool("isSliding", true);
        }
        else
        {
            playerAnimator.SetBool("isSliding", false);
        }
    }

    void CharacterMethod()
    {
        if (isMX)
        {
            playerAnimator = MXAnimator;
            MX_Body.SetActive(true);
            PCrawler_Body.SetActive(false);
            isFH_Body.SetActive(false);
        }
        else if (isPCrawler)
        {
            playerAnimator = PCAnimator;
            PCrawler_Body.SetActive(true);
            MX_Body.SetActive(false);
            isFH_Body.SetActive(false);
        }
        else if (isFH)
        {
            playerAnimator = FHAnimator;
            MX_Body.SetActive(false);
            PCrawler_Body.SetActive(false);
            isFH_Body.SetActive(true);
        }
    }


}
