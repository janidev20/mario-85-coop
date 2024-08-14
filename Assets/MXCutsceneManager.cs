using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MXCutsceneManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerMovement playerMove;

    [Header("Components")]
    [SerializeField] private Animator camAnim;
    [SerializeField] private GameObject MXSprite;
    [SerializeField] private GameObject SpamVText;
    [SerializeField] private GameObject BlackScreen;
    [SerializeField] private GameObject ScaryAmbient;
    [SerializeField] private GameObject IntenseScaryAmbient;
    [SerializeField] private GameObject FadeOut;
    [SerializeField] private List<GameObject> ScaryTexts;

    [Header("Audio")]
    [SerializeField] private AudioSource ChaseMusic;
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip spamSound;
    [SerializeField] AudioClip Lucas;
    [SerializeField] AudioClip Lucas2;
    [SerializeField] AudioClip InnocenceDoesntGetYouFar;
    [SerializeField] AudioClip bumpSound;
    [SerializeField] AudioClip wahooSound;

    [Header("Events")]
    public static bool firstTime = true;
    public static bool canPlayCutscene = true;
    public static bool pause = false;
    public static bool startCutscene = false;
    public static bool cutsceneIsPlaying = false;
    public static bool getToWahoo = false;
    [SerializeField] private int SpamLeft = 150;
    bool canStartSpamming = false;
    bool didntStartCutsceneBegin = true;
    bool didntStartMXShow = true;
    bool didntStartLucas2 = true;
    bool didntStartEnd = true;
    bool canShowScaryTexts = false;
    bool showed;
    bool reset;

    [SerializeField] private float timeToWait = 12f;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Story Mode")
        {
            if (!firstTime)
            {
                Destroy(this.gameObject);
            }

            if (startCutscene)
            {
                if (canPlayCutscene && didntStartCutsceneBegin && GameManager.gameStarted)
                {
                    pause = true;
                    StartCoroutine(CutsceneBegin());
                    didntStartCutsceneBegin = false;
                } 

            }
            else if (!startCutscene)
            {
                pause = false;
            }

            if (cutsceneIsPlaying)
            {
                if (UserInput.instance.SuperJump)
                {
                    if (canStartSpamming)
                    {
                        src.PlayOneShot(spamSound);
                        SpamLeft -= 1;
                    }
                    ScaryAmbient.SetActive(true);
                    if (didntStartMXShow && SpamLeft <= 120)
                    {
                        camAnim.SetBool("shake", true);
                        StartCoroutine(ShowMX());
                        didntStartMXShow = false;
                    }

                    if (didntStartLucas2 && SpamLeft <= 40)
                    {
                        ScaryAmbient.SetActive(false);
                        IntenseScaryAmbient.SetActive(true);
                        StartCoroutine(SayLucas2());
                        didntStartLucas2 = false;
                    }

                    if (didntStartEnd && SpamLeft <= 0)
                    {
                        ScaryAmbient.SetActive(false);
                        IntenseScaryAmbient.SetActive(false);
                        StartCoroutine(EndCutscene());
                        didntStartEnd = false;
                    }
                }

                if (canShowScaryTexts)
                {
                    ShowScaryText();
                }
            }
        }

    }

    IEnumerator CutsceneBegin()
    {
        src.PlayOneShot(bumpSound);
        camAnim.SetBool("chase", false);

        yield return new WaitForSeconds(5f);

        cutsceneIsPlaying = true;
        camAnim.SetBool("shiftdown", true);

        yield return new WaitForSeconds(2f);

        FadeOut.SetActive(true);

        yield return new WaitForSeconds(timeToWait - 2);

        SpamVText.SetActive(true);
        BlackScreen.SetActive(true);
        FadeOut.SetActive(false);
        camAnim.SetBool("shiftdown", false);
        canStartSpamming = true;
    }

    IEnumerator ShowMX()
    {
        yield return new WaitForSeconds(3f);

        MXSprite.SetActive(true);
        src.PlayOneShot(Lucas);
    }

    IEnumerator SayLucas2()
    {
        src.PlayOneShot(Lucas2);
        canShowScaryTexts = true;

        yield return new WaitForEndOfFrame();
    }

    void ShowScaryText()
    {
        if (!showed)
        {
            StartCoroutine(ScaryText());
            showed = true;
        }

        if (reset)
        {
            StartCoroutine(ResetText());
            reset = false;
        }
    }

    IEnumerator ScaryText()
    {
        GameObject text;
        text = ScaryTexts[Random.Range(0, ScaryTexts.Count)];
        text.SetActive(true);
        text.GetComponent<RectTransform>().anchoredPosition = new Vector3(Random.Range(-193, 271), Random.Range(-253, 460), 0);
        
        yield return new WaitForSeconds(Random.Range(0.15f, 0.3f));

        text.SetActive(false);
        reset = true;
    }

    IEnumerator EndCutscene()
    {
        canShowScaryTexts = false;
        foreach (GameObject e in ScaryTexts)
        {
            e.SetActive(false);
        }
        SpamVText.SetActive(false);
        MXSprite.SetActive(false);
        canStartSpamming = false;

        yield return new WaitForSeconds(2f);

        src.PlayOneShot(InnocenceDoesntGetYouFar);
        getToWahoo = true;

        yield return new WaitForSeconds(5f);

        ChaseMusic.enabled = true;
        src.PlayOneShot(wahooSound);
        camAnim.SetBool("shake", false);
        BlackScreen.SetActive(false);
        startCutscene = false;
        cutsceneIsPlaying = false;
        firstTime = false;
    }

    IEnumerator ResetText()
    {
        yield return new WaitForSeconds(Random.Range(0.45f, 1f));

        showed = false;
    }
}
