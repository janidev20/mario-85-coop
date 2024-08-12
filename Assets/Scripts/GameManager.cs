using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Fall Void Stuff")]
    [SerializeField] GameObject voidFallText;
    [SerializeField] GameObject voidLucasFallText;
    bool lucasDeathPlayed = false;

    [Header("Lucas")]
    [SerializeField] LucasAI LucasAI;
    [SerializeField] AudioSource LucasAudio;

    [Header("Scene Stuff, Events")]
    public static bool isStoryMode;
    public static bool cutScenePlaying;
    [SerializeField] GameObject PressTText;
    [SerializeField] GameObject pressYtoExitCastle;


    [Header("Dialogue")]
    [SerializeField] GameObject DialogueBox;
    [SerializeField] TextMeshProUGUI DialogueText;
    [SerializeField] AudioClip fhd1, fhd2, fhd3, fhd4, fhd5, Laughter;
    [SerializeField] GameObject MarioDisplay, MarioName;
    [SerializeField] string[] DialogueContent;

    [Header("Player/Void Stuff")]
    [Space(1)]
    [SerializeField] private PlayerMovement plyMoveScript;
    [SerializeField] private MXController MXController;
    [SerializeField] private PlayerAnimation plyAnimScript;
    [SerializeField] private CameraMovement cameraScript;
    [SerializeField] private Rigidbody2D plyRB;
    [SerializeField] private GameObject VoidWarningTXT;
    [SerializeField] private Vector2 previousVelocity;
    [SerializeField] private GameObject blackFadeCastle;
    private bool isDead;
    public bool PreIntroPlayed = false;
    public bool exitCastle = false;
    public bool canBeginDialogue = false;
    public bool dialogueDidntPlay = false;
    public bool startLucas = false;


    [Header("Pause Menu")]
    [Space(1)]
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject FadeOut;
    [SerializeField] private GameObject FadeIn;
    [SerializeField] private GameObject BlackScreen;
    [SerializeField] private GameObject MXBody;
    [SerializeField] private AudioSource[] InGameSounds;


    [Header("Global Audio Source")]
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource AmbientSource;
    [SerializeField] private AudioSource PreIntroBGMusicSRC;
    [SerializeField] private AudioClip UnPauseSFX, DeathSound;
    [SerializeField] private AudioClip DialogueNext, DialogueSpeak;
    [SerializeField] private AudioClip PreIntroBGMusic, PreIntroBGMusicFade;
    [SerializeField] private AudioClip Voice_Lucas, Voice_OhLucas, Voice_LetTheGamesBegin;
    [SerializeField] private AudioClip breakSound;

    [Header("Story Mode")]
    [SerializeField] private AudioClip LucasShout;
    [SerializeField] private AudioSource ScaryAmbientIntro;
    [SerializeField] private AudioSource ChaseMusicSource;
    [SerializeField] private GameObject pressTtoBreakDisguise;
    [SerializeField] private GameObject CastleBreaking;
    [SerializeField] private Animator camAnim;
    [SerializeField] int breakCount = 5; 
    [SerializeField] bool Break = false;


    public static bool gameStarted = false;

    public static bool isPaused = false;

    private void Awake()
    {

        FadeIn.SetActive(true);
        FadeOut.SetActive(false);
        isPaused = false;

     

            if (SceneManager.GetActiveScene().name == "Intro")
        {
            PreIntroPlayed = false;
            gameStarted = false;
            
            isStoryMode = true;
        }
        else
        {
            isStoryMode = false;
        }

        if (isStoryMode) { 
            cutScenePlaying = true;
        }

        if (SceneManager.GetActiveScene().name == "Story Mode")
        {
            ChaseMusicSource.enabled = false;
            ScaryAmbientIntro.enabled = false;
            LucasAI.enabled = false;
            AmbientSource.enabled = false;
            gameStarted = false;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Intro();
    }

    private void Update()
    {
        SkipIntroFunction();
    

            if (SceneManager.GetActiveScene().name == "Intro")
        {
           if (PreIntroPlayed && !canBeginDialogue)
            {
                StartCoroutine(IntroBeginIE());
                IntroBegin();
            }
            else if (canBeginDialogue && !dialogueDidntPlay)
            {
                StartCoroutine(DialogueBegin());
                dialogueDidntPlay = true;
            }

            if (exitCastle)
            {
                pressYtoExitCastle.SetActive(false);
            }

            if (startLucas)
            {
                StartCoroutine(WaitForLucas());
            }
        }


        if (SceneManager.GetActiveScene().name == "Story Mode")
        {


            ChangeBGState();


            if (!gameStarted)
            {
                if (!Break)
                {
                    if (UserInput.instance.Transform)
                    {
                        Break = true;
                        BreakDisguise();
                    }
                }

            }

               

            if (LucasEscape.Escaped)
            {
                ChaseMusicSource.enabled = false;

                FadeIn.SetActive(false);
                FadeOut.SetActive(false);
                FadeOut.SetActive(true);
                StartCoroutine(GoToScores());
            }

            if (VoidFallCounter.fellInVoid)
            {
                plyMoveScript.enabled = false;
                LucasAI.enabled = false;
                ChaseMusicSource.enabled = false;

                FadeIn.SetActive(false);
                FadeOut.SetActive(false);
                FadeOut.SetActive(true);
                StartCoroutine(FellInVoidRestart());
            }

            if (VoidFallCounter.LucasFellInVoid)
            {

                plyMoveScript.enabled = false;
                LucasAI.enabled = false;
                ChaseMusicSource.enabled = false;
                if (!lucasDeathPlayed)
                {
                    SFXSource.PlayOneShot(DeathSound);
                    lucasDeathPlayed = true;
                }

                FadeIn.SetActive(false);
                FadeOut.SetActive(false);
                FadeOut.SetActive(true);
                StartCoroutine(LucasFellInVoidRestart());

            }

            if (LucasDeathManager.needToRestart)
            {

                if (LucasDeathManager.GameOver)
                {
                    ChaseMusicSource.enabled = false;


                    FadeIn.SetActive(false);
                    FadeOut.SetActive(false);
                    FadeOut.SetActive(true);
                    StartCoroutine(GoToScores());

                } else
                {
                    ChaseMusicSource.enabled = false;

                    FadeIn.SetActive(false);
                    FadeOut.SetActive(false);
                    FadeOut.SetActive(true);
                    StartCoroutine(LucasDeathRestart());
                }



            }

        }


        if (!isPaused)
        {
           // ReloadScene();
            DeathHandler();
        }

        if (UserInput.instance.Pause && !isPaused)
        {
            if (SceneManager.GetActiveScene().name != "Intro")
            Pause();
        }

        

        if (isPaused) // If paused freeze time,
        {
            Time.timeScale = 0;
        }
        else // if unpaused, unfreeze time.
            Time.timeScale = 1;


        // Method for the Text : "Press 'V' to jump"
        if (plyMoveScript.onVoid)
        {
            if (plyAnimScript.isMX)
            {
                StartCoroutine(VoidWarning());
            }
           
        } else
        {
            VoidWarningTXT.SetActive(false);
        }
    }

    void Intro()
    {
        if (cutScenePlaying) { 
        AmbientSource.enabled = false;

            if (!PreIntroPlayed)
            {
                StartCoroutine(PreIntroDialogue());
            } 
        }
    }

    void ChangeBGState()
    {
        if (plyMoveScript.isFalling && !plyMoveScript.isJumping || plyMoveScript.onVoid)
        {
            camAnim.SetBool("chase", false);
        } 
        
        if (gameStarted && !plyMoveScript.isFalling && !plyMoveScript.onVoid || plyMoveScript.isJumping || plyMoveScript.isWahooJumping)
        {
                camAnim.SetBool("chase", true);
        }
    }

    void BreakDisguise()
    {
        if (breakCount > 0)
        {
            SFXSource.PlayOneShot(breakSound);
            if (breakCount != 1)
            {
                StartCoroutine(resetCamShakeBool(1));
                camAnim.SetBool("shake", true);
            }
            breakCount -= 1;
            Break = false;

            if (breakCount == 5)
            {
                ScaryAmbientIntro.enabled = true;
            }

            else if (breakCount <= 1)
            {
                StartCoroutine(plyAnimScript.ForceTransform());
                camAnim.SetBool("shake", true);
                camAnim.SetBool("chase", true);
                StartCoroutine(resetCamShakeBool(3));
                pressTtoBreakDisguise.SetActive(false);
                AmbientSource.enabled = true;
                SFXSource.PlayOneShot(LucasShout);
                //    CastleBreaking.SetActive(true);
                LucasAI.enabled = true;
                ChaseMusicSource.enabled = true;
                gameStarted = true;

            }
        }
    }

    IEnumerator resetCamShakeBool(float timeToReset)
    {
        yield return new WaitForSeconds(timeToReset);

        camAnim.SetBool("shake", false);
    }

    IEnumerator IntroBeginIE()
    {
        FadeIn.SetActive(false);
        FadeIn.SetActive(true);
        BlackScreen.SetActive(false);
        blackFadeCastle.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (!exitCastle)
        {
            pressYtoExitCastle.SetActive(true);
        }
        
    }

    void SkipIntroFunction()
    {
        if (IntroManager.skipped)
        {
            StartCoroutine(SkipIntro());
        }
    }

    IEnumerator SkipIntro()
    {

        cutScenePlaying = false;
        FadeIn.SetActive(false);
        FadeIn.SetActive(true);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Story Mode");
    }

    void IntroBegin()
    {

        if (UserInput.instance.Talk)
        {
            blackFadeCastle.SetActive(false);
            exitCastle = true;
        }

        if (exitCastle)
        {
            blackFadeCastle.SetActive(false);
            if (!plyMoveScript.cantMove)
            {
                plyMoveScript.direction.x = -1;
            } else
            {
                plyMoveScript.direction.x = 0;
                startLucas = true;
                
            }
        }

    }

    IEnumerator FellInVoidRestart()
    {
        if (VoidFallCounter.fellInVoid)
        {

            voidFallText.SetActive(true);

            yield return new WaitForSeconds(4.5f);

            VoidFallCounter.fellInVoid = false;
            VoidFallCounter.LucasFellInVoid = false;

            SceneManager.LoadScene("Story Mode");
        }
    }

    IEnumerator LucasFellInVoidRestart()
    {
        if (VoidFallCounter.LucasFellInVoid)
        {

            voidLucasFallText.SetActive(true);

            yield return new WaitForSeconds(4.5f);


            VoidFallCounter.fellInVoid = false;
            VoidFallCounter.LucasFellInVoid = false;

            SceneManager.LoadScene("Story Mode");
        }
    }

    IEnumerator WaitForLucas()
    {   

        yield return new WaitForSeconds(5f);


        if (!LucasAI.stop)
       {
           LucasAI.moveRight = true;
       } else
        {
          LucasAI.moveRight = false;
           canBeginDialogue = true;
        }
       
        
    }

    IEnumerator LucasDeathRestart()
    {

        yield return new WaitForSeconds(4.5f);


            VoidFallCounter.fellInVoid = false;
            VoidFallCounter.LucasFellInVoid = false;


            SceneManager.LoadScene("Story Mode");
        
    }

    IEnumerator GoToScores()
    {
        yield return new WaitForSeconds(5.5f);

        SceneManager.LoadScene("Scores");
    }

    IEnumerator PreIntroDialogue()
    {
        if (!PreIntroPlayed)
        {
            PreIntroBGMusicSRC.clip = PreIntroBGMusic;

            yield return new WaitForSeconds(12f);

            MXBody.SetActive(true);
            SFXSource.PlayOneShot(Voice_Lucas);

            yield return new WaitForSeconds(13.5f);


            MXBody.GetComponent<Animator>().SetTrigger("change");

            yield return new WaitForSeconds(3f);

            SFXSource.PlayOneShot(Voice_LetTheGamesBegin);

            yield return new WaitForSeconds(5.5f);
            MXBody.SetActive(false);

            PreIntroBGMusicSRC.enabled = false;

            yield return new WaitForSeconds(5f);
           
            PreIntroPlayed = true;
        }


    }

    IEnumerator DialogueBegin()
    {
        BlackScreen.SetActive(false);
        FadeIn.SetActive(false);
        FadeIn.SetActive(true);

        yield return new WaitForSeconds(3f);

        DialogueBox.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        MarioName.SetActive(true);
        MarioDisplay.SetActive(true);

        yield return new WaitForSeconds(1.2f);

        DialogueText.text = "Hey there, Champ!";
        SFXSource.PlayOneShot(fhd1);
        SFXSource.PlayOneShot(DialogueSpeak);

        yield return new WaitForSeconds(2.5f);

        DialogueText.text = "Thanks for coming!";
        SFXSource.PlayOneShot(fhd2);
        SFXSource.PlayOneShot(DialogueSpeak);

        yield return new WaitForSeconds(2);

        DialogueText.text = "I left ya a little something at the beginning";
        SFXSource.PlayOneShot(fhd3);
        SFXSource.PlayOneShot(DialogueSpeak);

        yield return new WaitForSeconds(3);

        DialogueText.text = "I left ya a little something at the beginning, you wanna go take a look?";
        SFXSource.PlayOneShot(fhd4);
        SFXSource.PlayOneShot(DialogueSpeak);

        yield return new WaitForSeconds(3);

        DialogueText.text = "It's a big surprise!";
        SFXSource.PlayOneShot(fhd5);
        SFXSource.PlayOneShot(DialogueSpeak);

        yield return new WaitForSeconds(2.5f);


        yield return new WaitForSeconds(2.5f);

        //StartCoroutine(plyAnimScript.ForceTransform());
        SFXSource.PlayOneShot(DialogueSpeak);
        //SFXSource.PlayOneShot(Laughter);
        DialogueBox.SetActive(false);
        cutScenePlaying = false;

        yield return new WaitForSeconds(1.5f);

        FadeOut.SetActive(false);
        FadeOut.SetActive(true);

        yield return new WaitForSeconds(2.25f);


        SceneManager.LoadScene("Story Mode");
        
        FadeIn.SetActive(false);
        FadeIn.SetActive(true);
        
    }
    
    IEnumerator VoidWarning()
    {
        yield return new WaitForSeconds(1.75f);

        VoidWarningTXT.SetActive(true);
    }

    public void Pause()
    {
        isPaused = true;
        PauseMenu.SetActive(true);

      
       foreach (AudioSource AudioObject in InGameSounds)
        {
            AudioObject.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DeathHandler()
    {
        if (plyMoveScript.onVoid && plyAnimScript.isFH && !isDead)
        {
            StartCoroutine(DeathSequence());
            isDead = true;
        }
    }

    IEnumerator DeathSequence()
    {

        // Disable Player and Camera Movement
         plyRB.constraints = RigidbodyConstraints2D.FreezePosition; 
         plyMoveScript.enabled = false;
         plyAnimScript.enabled = false;

        cameraScript.enabled = false;

        foreach (AudioSource AudioObject in InGameSounds)
        {
            AudioObject.enabled = false;
        }

        SFXSource.PlayOneShot(DeathSound);

        yield return new WaitForSeconds(4.25f);

        FadeOut.SetActive(true);
        FadeIn.SetActive(false);

        yield return new WaitForSeconds(1.55f);

        // Basically reload the Scene
        SceneManager.LoadScene("Story Mode");
    }

    void ReloadScene()
    {
        if (SceneManager.GetActiveScene().name == "Story Mode")
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Story Mode");
            }
        }
    }

    public void UnPause()
    {

        SFXSource.PlayOneShot(UnPauseSFX);

        isPaused = false;
        PauseMenu.SetActive(false);

        foreach (AudioSource AudioObject in InGameSounds)
        {
            AudioObject.enabled = true;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitToMenu()
    {
        StartCoroutine(_quitToMenu());
    }

    IEnumerator _quitToMenu()
    {
        FadeOut.SetActive(true);
        isPaused = false; // This is to unfreeze the time, otherwise it won't change to 'MainMenu' Scene
    
        yield return new WaitForSeconds(2.1f);

        SceneManager.LoadScene("MainMenu");

    }


}
