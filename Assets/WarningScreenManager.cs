using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningScreenManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject Warning1, Warning2, Warning3, Warning4, Warning5;
    [SerializeField] GameObject PressZtoProceed;

    [Header("Audio")]
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip bumpSound;

    bool canHitNext = false;

    private void Awake()
    {
        canHitNext = false;
        StartCoroutine(Warning1Text());
    }

    private void Update()
    {
        if (canHitNext)
        {
            if (UserInput.instance.JumpJustPressed)
            {
                if (Warning1.activeSelf)
                {
                    StartCoroutine(Warning2Text());
                    canHitNext = false;
                } 

                else if (Warning2.activeSelf)
                {
                    StartCoroutine(Warning3Text());
                    canHitNext = false;
                }

                else if (Warning3.activeSelf)
                {
                    StartCoroutine(Warning4Text());
                    canHitNext = false;
                }

                else if (Warning4.activeSelf)
                {
                    StartCoroutine(Warning5Text());
                    canHitNext = false;
                }

                else if (Warning5.activeSelf)
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }

        if (UserInput.instance.Run)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator Warning1Text()
    {
        PressZtoProceed.SetActive(false);
        src.PlayOneShot(bumpSound);
        
        Warning1.SetActive(true);

        yield return new WaitForSeconds(3f);

        PressZtoProceed.SetActive(true);
        canHitNext = true;
    }

    IEnumerator Warning2Text()
    {
        Warning1.SetActive(false);
        PressZtoProceed.SetActive(false);
        src.PlayOneShot(bumpSound);

        Warning2.SetActive(true);

        yield return new WaitForSeconds(3f);

        PressZtoProceed.SetActive(true);
        canHitNext = true;
    }
    IEnumerator Warning3Text()
    {
        Warning2.SetActive(false);
        PressZtoProceed.SetActive(false);
        src.PlayOneShot(bumpSound);

        Warning3.SetActive(true);

        yield return new WaitForSeconds(3f);

        PressZtoProceed.SetActive(true);
        canHitNext = true;
    }
    IEnumerator Warning4Text()
    {
        Warning3.SetActive(false);
        PressZtoProceed.SetActive(false);
        src.PlayOneShot(bumpSound);

        Warning4.SetActive(true);

        yield return new WaitForSeconds(3f);

        PressZtoProceed.SetActive(true);
        canHitNext = true;
    }

    IEnumerator Warning5Text()
    {
        Warning4.SetActive(false);
        PressZtoProceed.SetActive(false);
        src.PlayOneShot(bumpSound);

        Warning5.SetActive(true);

        yield return new WaitForSeconds(3f);

        PressZtoProceed.SetActive(true);
        canHitNext = true;
    }
}
