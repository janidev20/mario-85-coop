using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [SerializeField] GameObject SkipIntroParent;
    [SerializeField] TextMeshProUGUI SkipIntroText;
    [SerializeField] Camera main;
    public static bool canShow = false;
    public static bool skipped = false;

    private void Awake()
    {
        skipped = false;
    }

    private void Update()
    {
        DontDestroyThisObject();

        if (SceneManager.GetActiveScene().name == "Scores")
        {
            EnableSkippingIntro();
        }

        if (SceneManager.GetActiveScene().name == "Intro")
        {

            if (canShow)
            {
                SkipIntroParent.SetActive(true);

                if (UserInput.instance.Interact)
                {
                    skipped = true;
                }
            }


            if (Camera.main != null)
            {
                main = Camera.main;
                //This enables Main Camera
                main.enabled = true;
            }
            GetComponentInChildren<Canvas>().worldCamera = main;


        }
        else
        {   
            skipped = false;
            SkipIntroParent.SetActive(false);
        }

    }

    public void EnableSkippingIntro ()
    {

            canShow = true;
            Destroy(this.gameObject);
 
    }
    void DontDestroyThisObject()
    {
        if (SceneManager.GetActiveScene().name != "Scores")
        {
            DontDestroyOnLoad(this.gameObject);

        } 
    
    }
}
