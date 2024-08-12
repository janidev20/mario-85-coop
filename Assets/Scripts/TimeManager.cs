using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
   public static float gameTime = 0;

    private void Update()
    {
        DontDestroyThisObject();

        if (SceneManager.GetActiveScene().name == "Story Mode" && !LucasController.LucasIsDead && !LucasEscape.Escaped && !VoidFallCounter.fellInVoid && GameManager.gameStarted)
        {
            TimeCount();
        }
    }

    void TimeCount()
    {
        gameTime += Time.deltaTime;
    }

    void DontDestroyThisObject()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            gameTime = 0;

        }
    }
}
