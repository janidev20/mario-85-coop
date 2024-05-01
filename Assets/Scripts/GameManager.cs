using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player/Void Stuff")]
    [Space(1)]
    [SerializeField] private PlayerMovement plyMoveScript;
    [SerializeField] private PlayerAnimation plyAnimScript;
    [SerializeField] private Rigidbody2D plyRB;
    [SerializeField] private GameObject VoidWarningTXT;
    [SerializeField] private Vector2 previousVelocity;


    [Header("Pause Menu")]
    [Space(1)]
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject FadeOut;
    [SerializeField] private GameObject FadeIn;
    [SerializeField] private AudioSource[] InGameSounds;

    [Header("Global Audio Source")]
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource AmbientSource;
    [SerializeField] private AudioClip UnPauseSFX, DeathSound, ScaryAmbient, HappyAmbient;

    public bool isHappy = true;
    public static bool isPaused = false;

    private void Start()
    {
        FadeIn.SetActive(true);
        FadeOut.SetActive(false);
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeMusic();
        }


        if (!isPaused)
        {
            DeathHandler();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Pause();
        }

        if (isPaused)
        {
            plyRB.isKinematic = true; // disables physics on ply rigidbody
            plyMoveScript.enabled = false;
            plyAnimScript.enabled = false;
            return;
        }
        else
            plyRB.isKinematic = false; // enables physics on ply rigidbody 
        plyMoveScript.enabled = true;
        plyAnimScript.enabled = true;



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

    void ChangeMusic()
    {
        isHappy = !isHappy;

        if (isHappy)
        {
            AmbientSource.enabled = false;
            AmbientSource.clip = HappyAmbient;
            AmbientSource.enabled = true;
        }
        else
        {
            AmbientSource.enabled = false;
            AmbientSource.clip = ScaryAmbient;
            AmbientSource.enabled = true;

        }
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
        if (plyMoveScript.onVoid && !plyAnimScript.isMX)
        {
            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator DeathSequence()
    {
        isPaused = true;

        foreach (AudioSource AudioObject in InGameSounds)
        {
            AudioObject.enabled = false;
        }

        SFXSource.PlayOneShot(DeathSound);

        yield return new WaitForSeconds(4.25f);

        FadeOut.SetActive(true);
        FadeIn.SetActive(false);

        yield return new WaitForSeconds(1.55f);

        SceneManager.LoadScene("Test Scene");
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

        yield return new WaitForSeconds(2.1f);

        SceneManager.LoadScene("MainMenu");

    }


}
