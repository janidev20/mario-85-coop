using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Play Menu")]
    [Space(1)]
    [SerializeField] private RectTransform PlayHolder;

    Vector3 testingPos = new Vector2(1931, 0);
    Vector3 storyPos = new Vector2(0, 0);
    Vector3 funPos = new Vector2(-1931, 0);
    Vector3 sandboxPos = new Vector2(-3862, 0);

    [SerializeField] private bool isTestingMode, isStoryMode, isFunMode, isSandboxMode;
    [SerializeField] public static bool StoryChoose = false, TestChoose = false;

    [SerializeField] private GameObject UnavailableTXT;
    private bool canInteract = true;

    [SerializeField] private GameObject LoadFade;


    private void Start()
    {
      

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isTestingMode = false;
        isStoryMode = true;
        isFunMode = false;
        isSandboxMode = false;

     
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {

        // PLAY MENU
        PlayMenu();
    }

    // -1931 swipe amount
    // The functions that build up the Play Menu
    public void PlayMenu()
    {
        if (isFunMode)
        {
            PlayHolder.localPosition = Vector3.Lerp(PlayHolder.localPosition, funPos, .1f);
        }

        else if (isStoryMode)
        {
            PlayHolder.localPosition = Vector3.Lerp(PlayHolder.localPosition, storyPos, .1f);
        }

        else if (isSandboxMode)
        {
            PlayHolder.localPosition = Vector3.Lerp(PlayHolder.localPosition, sandboxPos, .1f);
        }

        else if (isTestingMode)
        {
            PlayHolder.localPosition = Vector3.Lerp(PlayHolder.localPosition, testingPos, .1f);
        }
    }
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

    // 'Coming soon' text on the gamemodes that are not yet available (UNAVAILABLE)
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

    // Load the Testing Gamemode
    public void LoadTestingMode()
    {
        StartCoroutine(STRTLoadFade());
        TestChoose = true;
    }

    public void LoadStoryMode()
    {
        StartCoroutine(STRTLoadFade());
        StoryChoose = true;
    }

    public IEnumerator STRTLoadFade()
    {
        LoadFade.SetActive(true);

        yield return new WaitForSeconds(3.25f);

        DontDestroyOnLoad(this);
        SceneManager.LoadScene("LoadingScene");
    }

    //QUIT Button Function
    public void QuitGame()
    {
        Application.Quit();
    }
}