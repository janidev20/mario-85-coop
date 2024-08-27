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
    public static int LucasLife = 3;
    public static int maxLife;

    private void Start()
    {
       

        needToRestart = false;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            // Reset the values if on the "Scores" Screen.
            needToRestart = false;
            diedOnce = false;
            GameOver = false;
            LucasLife = 3;
            maxLife = 3;
            Destroy(this.gameObject);
        }

        if (!didYouDieYet)
        {
            if (LucasController.LucasIsDead)
            {
                if (LucasLife == 1)
                {
                    GameOver = true;
                }
                else
                {
                    GameOver = false;
                }

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

       

        DontDestroyThisObject();
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForEndOfFrame();

        needToRestart = true;
        Destroy(this.gameObject);
    }

    void DontDestroyThisObject()
    {
        if (SceneManager.GetActiveScene().name == "Story Mode" && LucasLife == 0 || SceneManager.GetActiveScene().name == "Story Mode" && LucasEscape.Escaped)
        {
            DontDestroyOnLoad(this);
        }
    }
}
