using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BlocksCounter : MonoBehaviour
{
    [Header("Values")]
    public static int BrickBlock;
    public static int QMBlock;
    public static int StoneBlock;
    public static int EmptyBlock;
    public static int Pipe;

    private void Start()
    {
        QMBlock = 54;

        // if (!LucasDeathManager.diedOnce)
        //{
        //     BrickBlock = 0;
        //     QMBlock = 0;
        //    StoneBlock = 0;
        //    EmptyBlock = 0;
        //     Pipe = 0;
        //   }
    }

    private void Update()
    {
        DontDestroyThisObject();

    }

    void DontDestroyThisObject()
    {
        if (SceneManager.GetActiveScene().name == "Story Mode")
        {
            DontDestroyOnLoad(this.gameObject);
        }

        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(this.gameObject);
        }
    }
}
