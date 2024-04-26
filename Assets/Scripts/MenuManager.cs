using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // PLAY MENU
    [Header("Play Menu")]
    [Space(1)]
    [SerializeField] private RectTransform MenuHolder;

    Vector3 testingPos = new Vector2(1931, 0);
    Vector3 storyPos = new Vector2(0, 0);
    Vector3 funPos = new Vector2(-1931, 0);
    Vector3 sandboxPos = new Vector2(-3862, 0);

    [SerializeField] private bool isTestingMode, isStoryMode, isFunMode, isSandboxMode;

    [SerializeField] private GameObject UnavailableTXT;
    private bool canInteract = true;

    [SerializeField] private GameObject LoadFade;

    [Header("Main Menu")]
    [Space(1)]

    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject PlayMenu;

    private void Start()
    {
       // MainMenu.SetActive(true);
       // PlayMenu.SetActive(false);


        // PLAY MENU
        isTestingMode = false;
        isStoryMode = true;
        isFunMode = false;
        isSandboxMode = false;
    }

    private void FixedUpdate()
    {

        // PLAY MENU
        if (isFunMode)
        {
            MenuHolder.localPosition = Vector3.Lerp(MenuHolder.localPosition, funPos, .1f);
        }

        else if (isStoryMode)
        {
            MenuHolder.localPosition = Vector3.Lerp(MenuHolder.localPosition, storyPos, .1f);
        }

       else if (isSandboxMode)
        {
            MenuHolder.localPosition = Vector3.Lerp(MenuHolder.localPosition, sandboxPos, .1f);
        }

       else if (isTestingMode)
        {
            MenuHolder.localPosition = Vector3.Lerp(MenuHolder.localPosition, testingPos, .1f);
        }
    }

    // -1931 swipe amount
    // PLAY MENU
    public void SwapRight()
    {
        if (isTestingMode)
        {
            isTestingMode = false;
            isStoryMode = true;
            isFunMode = false;
            isSandboxMode = false;
        }
        
        else if (isStoryMode)
        {
            isTestingMode = false;
            isStoryMode = false;
            isFunMode = true;
            isSandboxMode = false;
        } 
        
       else if (isFunMode)
        {
            isTestingMode = false;
            isStoryMode = false;
            isFunMode = false;
            isSandboxMode = true;
        }
    }

    public void SwapLeft()
    {
        if (isSandboxMode)
        {
            isTestingMode = false;
            isStoryMode = false;
            isFunMode = true;
            isSandboxMode = false;
        }
        
       else if (isFunMode)
        {
            isTestingMode = false;
            isStoryMode = true;
            isFunMode = false;
            isSandboxMode = false;
        }
        
       else if (isStoryMode)
        {
            isTestingMode = true;
            isStoryMode = false;
            isFunMode = false;
            isSandboxMode = false;
        }
    }

    public void StartUNAV()
    {
        if (canInteract)
        StartCoroutine(Unavailable());
    }

    IEnumerator Unavailable()
    {
        canInteract = false;
        UnavailableTXT.SetActive(true);

        yield return new WaitForSeconds(2.2f);

        UnavailableTXT.SetActive(false);
        canInteract = true;
    }

    public void LoadTestingMode()
    {
        StartCoroutine(STRTLoadFade());
    }

    public IEnumerator STRTLoadFade()
    {
        LoadFade.SetActive(true);

        yield return new WaitForSeconds(3.25f);

        SceneManager.LoadScene("LoadingScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
