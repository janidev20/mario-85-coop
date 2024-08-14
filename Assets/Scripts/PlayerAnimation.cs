using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Animator))]
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
    [SerializeField] public bool forceTransform = false; // used at story mode intro
    [SerializeField] public bool forceTransformMX = false; // used at story mode intro
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

        if (plyMovement.isFalling)
        {
            playerAnimator.SetBool("isFalling", true);
        }
        else
        {
            playerAnimator.SetBool("isFalling", false);
        }

        if (plyMovement.isLanding)
        {
            playerAnimator.SetBool("isLanding", true);
        } else
        {
            playerAnimator.SetBool("isLanding", false);
        }

        if (StunManager.isStunned && isPCrawler)
        {
            playerAnimator.SetBool("isStunned", true);
        } else
        {
            playerAnimator.SetBool("isStunned", false);
        }
    }

    void CharacterMethod()
    {
       

        if (!GameManager.isPaused)
        {

                if (isPCrawler && UserInput.instance.Transform && plyMovement.onVoid || plyMovement.onVoid && isPCrawler && GameManager.gameStarted || forceTransformMX) // Will also initiate if we're in the void as PCrawler
                {
                    isMX = true;
                    isPCrawler = false;
                    isFH = false;
                    forceTransformMX = false;
                    coolDownTimer = 0; // Resets the coolDownTimer
                }
                else if (isFH && UserInput.instance.Transform && GameManager.gameStarted || forceTransform)
                {
                    forceTransform = false;
                    isPCrawler = true;
                    isMX = false;
                    isFH = false;
                    coolDownTimer = 0; // Resets the coolDownTimer
                }
             //   else if (isMX && UserInput.instance.Transform && GameManager.gameStarted)
              //  {
              //      isFH = true;
              //      isMX = false;
              //      isPCrawler = false;
             //       FScrAnimator.gameObject.SetActive(false);
             //       FScrAnimator.gameObject.SetActive(true);
              //      coolDownTimer = 0; // Resets the coolDownTimer
              //  }
            
        
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

    public IEnumerator ForceTransform()
    {
       
            forceTransform = true;
        
        yield return new WaitForEndOfFrame();
    }


}
