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
        if (SceneManager.GetActiveScene().name == "Story Mode" && LucasDeathManager.LucasLife == 1 || SceneManager.GetActiveScene().name == "Story Mode" && LucasEscape.Escaped)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            gameTime = 0;
            Destroy(this.gameObject);
        }
    }
}
