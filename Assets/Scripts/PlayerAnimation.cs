using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerMovement plyMovement;
    [SerializeField] Animator playerAnimator;

    [SerializeField] Animator MXAnimator, PCAnimator, FHAnimator, FScrAnimator;
    [SerializeField] private GameObject MX_Body, PCrawler_Body, isFH_Body;

    [SerializeField] public bool isMX, isPCrawler, isFH;

    [SerializeField] private float coolDownTimer; // This is for switching characters, so that the Y position doesn't bug out.
    [SerializeField] private float coolDownAmount = 2.5f;

    [SerializeField] private float transformTime = 0.85f;
    [SerializeField] public bool isTransforming; // If the transform animation is playing (based on time calculation)

    private void Awake()
    {
        if (playerAnimator == null)
        {
            AnimationMethod();
        }
    }

    private void Update()
    {
        if (coolDownTimer < coolDownAmount)
        {
            coolDownTimer += Time.deltaTime;
        } 
        else if (coolDownTimer >= coolDownAmount)
        {
            CharacterMethod();
        }

        if (coolDownTimer < transformTime && !isFH && plyMovement.onGround)
        {
            isTransforming = true;
        }
        else if (coolDownTimer >= transformTime)
        {
            isTransforming = false;
        }

        AnimatorBooleans();
        AnimationMethod();
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

        if (plyMovement.isMoving)
        {
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
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

        if (plyMovement.isCrouching)
        {
            playerAnimator.SetBool("isCrouching", true); 
        } else
        {
            playerAnimator.SetBool("isCrouching", false);
        }

        if (plyMovement.isWahooJumping)
        {
            playerAnimator.SetBool("isWahooJumping", true);
        } else
        {
            playerAnimator.SetBool("isWahooJumping", false);
        }
    }

    void CharacterMethod()
    {
        if (Input.GetKeyDown(KeyCode.F3) && isPCrawler)
        {
            isMX = true;
            isPCrawler = false;
            isFH = false;
            coolDownTimer = 0; // Resets the coolDownTimer
        }
        else if (Input.GetKeyDown(KeyCode.F2) && isFH)
        {
            isPCrawler = true;
            isMX = false;
            isFH = false;
            coolDownTimer = 0; // Resets the coolDownTimer
        }
        else if (Input.GetKeyDown(KeyCode.F1) && !isFH)
        {
            isFH = true;
            isMX = false;
            isPCrawler = false;
            FScrAnimator.gameObject.SetActive(false);
            FScrAnimator.gameObject.SetActive(true);
            coolDownTimer = 0; // Resets the coolDownTimer
        }


    }

    void AnimationMethod()
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
