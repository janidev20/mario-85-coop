using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LucasSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject Lucas;
    public static bool newGame = true;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Story Mode")
        {
            if (newGame)
            {
                Destroy(Lucas);
                newGame = false;
                SceneManager.LoadScene("Story Mode");
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Story Mode")
        {
            if (LucasDeathManager.LucasLife == 0)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        if (SceneManager.GetActiveScene().name == "Scores")
        {
            newGame = true;
            Destroy(this.gameObject);
        }
    }
}
 