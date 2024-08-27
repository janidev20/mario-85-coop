using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ScoresManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject lucasHead;
    [SerializeField] GameObject yourScoresTitle;
    [SerializeField] GameObject brokenObstacles;
    [SerializeField] GameObject QMBlock;
    [SerializeField] GameObject brickBlock;
    [SerializeField] GameObject stoneBlock;
    [SerializeField] GameObject pipe;
    [SerializeField] GameObject timeSpent;
    [SerializeField] GameObject lucasStatusTitle;
    [SerializeField] GameObject fellInVoidTitle;

    [Header("Values")]
    [SerializeField] TextMeshProUGUI qmBlockAmount;
    [SerializeField] TextMeshProUGUI brickBlockAmount;
    [SerializeField] TextMeshProUGUI stoneBlockAmount;
    [SerializeField] TextMeshProUGUI pipeAmount;
    [SerializeField] TextMeshProUGUI timeAmount;
    [SerializeField] TextMeshProUGUI lucasDead;
    [SerializeField] TextMeshProUGUI lucasEscaped;
    [SerializeField] TextMeshProUGUI fellInVoidAmount;
    [SerializeField] TextMeshProUGUI pressSpaceToReturn;

    [Header("Audio")]
    [SerializeField] AudioSource SRC;
    [SerializeField] AudioClip woosh, beep, count, success, fail, returnSound, titleWoosh;

    bool showedScored = false;

    private void Start()
    {
        float time = 0;
        int wholeMin = 0;
        int seconds = 0;
        time = TimeManager.gameTime / 60;
        wholeMin = Mathf.FloorToInt(time);
        seconds = Mathf.FloorToInt((time - wholeMin) * 60);
        timeAmount.text = wholeMin + " m " + seconds + " s";

        if (LucasEscape.Escaped)
        {
            lucasHead.SetActive(false);
        } else
        {
            lucasHead.SetActive(true);
        }
    }

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            pressSpaceToReturn.text = "tap screen to return to menu";
        } else
        {
            pressSpaceToReturn.text = "press 'space' to return to menu";
        }

        if (!showedScored)
        {
            StartCoroutine(Scores());
            showedScored = true;
        }

        if (UserInput.instance.Interact || Application.isMobilePlatform && UserInput.instance.Talk)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator Scores()
    {
        SRC.PlayOneShot(titleWoosh);
        yourScoresTitle.SetActive(true);

        yield return new WaitForSeconds(3f);

        SRC.PlayOneShot(woosh);
        brokenObstacles.SetActive(true);

        yield return new WaitForSeconds(2f);


        SRC.PlayOneShot(woosh);
        QMBlock.SetActive(true);

        yield return new WaitForSeconds(0.4f);


        SRC.PlayOneShot(woosh);
        brickBlock.SetActive(true);

        yield return new WaitForSeconds(0.4f);


        SRC.PlayOneShot(woosh);
        stoneBlock.SetActive(true);

        yield return new WaitForSeconds(0.4f);


        SRC.PlayOneShot(woosh);
        pipe.SetActive(true);

        yield return new WaitForSeconds(1f);


        SRC.PlayOneShot(beep);
        qmBlockAmount.gameObject.SetActive(true);
        brickBlockAmount.gameObject.SetActive(true);
        stoneBlockAmount.gameObject.SetActive(true);
        pipeAmount.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        StartCoroutine(countDestroyedObstacles());

        yield return new WaitForSeconds(0.5f);


        SRC.PlayOneShot(woosh);
        timeSpent.SetActive(true);

        yield return new WaitForSeconds(1f);


        SRC.PlayOneShot(beep);
        timeAmount.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);


        SRC.PlayOneShot(woosh);
        lucasStatusTitle.SetActive(true);

        yield return new WaitForSeconds(1f);

        if (LucasEscape.Escaped)
        {

            SRC.PlayOneShot(fail);
            lucasDead.gameObject.SetActive(false);
            lucasEscaped.gameObject.SetActive(true);
        } else
        {

            SRC.PlayOneShot(success);
            lucasDead.gameObject.SetActive(true);
            lucasEscaped.gameObject.SetActive(false);

        }

        yield return new WaitForSeconds(1f);


        SRC.PlayOneShot(woosh);
        fellInVoidTitle.SetActive(true);

        yield return new WaitForSeconds(1f);

        SRC.PlayOneShot(beep);
        fellInVoidAmount.gameObject.SetActive(true);
        
        fellInVoidAmount.text = "" + VoidFallCounter.fellInVoidAmount;

        yield return new WaitForSeconds(1f);

        SRC.PlayOneShot(returnSound); 
        pressSpaceToReturn.gameObject.SetActive(true);

    }

    // FINISH THIS
    IEnumerator countDestroyedObstacles()
    {
        StartCoroutine(countQMBlock());
        StartCoroutine(countBrickBlock());
        StartCoroutine(countStoneBlock());
        StartCoroutine(countPipe());
        yield return new WaitForEndOfFrame();
    }

    IEnumerator countQMBlock()
    {
        for (int i = 0; i < BlocksCounter.QMBlock; i++)
        {
            qmBlockAmount.text = "x " + i;
            yield return new WaitForSeconds(0.005f);
        }
    }
    IEnumerator countBrickBlock()
    {
        for (int i = 0; i < BlocksCounter.BrickBlock; i++)
        {
            brickBlockAmount.text = "x " + i;
            yield return new WaitForSeconds(0.005f);
        }


    }
    IEnumerator countStoneBlock()
    {
        for (int i = 0; i < BlocksCounter.StoneBlock; i++)
        {
            stoneBlockAmount.text = "x " + i;
            yield return new WaitForSeconds(0.005f);
        }

    }
    IEnumerator countPipe()
    {
        for (int i = 0; i < BlocksCounter.Pipe; i++)
        {
            pipeAmount.text = "x " + i;
            yield return new WaitForSeconds(0.005f);
        }

    }

}
