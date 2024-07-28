using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{

    [SerializeField] GameObject LoadFade;

    private void Start()
    {
        LoadFade.SetActive(false);
    }

    private void Update()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(9f);

        LoadFade.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        
        if (MenuManager.StoryChoose)
        {
            SceneManager.LoadScene("Intro");
        }
        else
        {
            SceneManager.LoadScene("Test Scene");
        }
    }
}
