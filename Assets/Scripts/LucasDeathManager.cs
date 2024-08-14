using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LucasDeathManager : MonoBehaviour
{

   [Header("Components")]
   [SerializeField] Animator animator;
   [SerializeField] SpriteRenderer sr;
   [SerializeField] Collider2D cd;

   [Header("Booleans")]
   [SerializeField] private bool didYouDieYet = false;

   [Header("Audio")]
   [SerializeField] private AudioSource SRC;
   [SerializeField] private AudioClip DeathSound;

    [Header("Death Event")]
    public static bool needToRestart;
    public static bool diedOnce = false;
    public static bool GameOver;
    public static int LucasLife = 5;
    public static int maxLife;

    private void Start()
    {
       

        if (!diedOnce)
        {
            maxLife = LucasLife; 
        }

        needToRestart = false;

        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Scores")
        {
            // Reset the values if on the "Scores" Screen.
            needToRestart = false;
            diedOnce = false;
            GameOver = false;
            LucasLife = 5;
            maxLife = 5;
        }

        if (!didYouDieYet)
        {
            if (LucasController.LucasIsDead)
            {
                if (!diedOnce)
                {
                    diedOnce = true;
                }

                animator.SetTrigger("dead");
                sr.sortingLayerID = 0;
                cd.enabled = false;
                SRC.PlayOneShot(DeathSound);
                LucasLife -= 1;
                StartCoroutine(RestartGame());
                didYouDieYet = true;
            }
        }

        if ((maxLife - LucasLife) == maxLife)
        {
            GameOver = true;
        } else
        {
            GameOver = false;
        }

        DontDestroyThisObject();
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForEndOfFrame();

        needToRestart = true;
    }

    void DontDestroyThisObject()
    {
        if (SceneManager.GetActiveScene().name == "Story Mode")
        {
            DontDestroyOnLoad(this);
        }
    }
}
