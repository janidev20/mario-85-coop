using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LucasEscape : MonoBehaviour
{
    bool didntPlay = true;
    public static bool Escaped;


    [SerializeField] AudioSource SRC;
    [SerializeField] AudioClip escapeSound;

    // Start is called before the first frame update
    void Start()
    {
       if (SceneManager.GetActiveScene().name == "Story Mode")
        {
                Escaped = false;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyThisObject();

        if (Escaped && didntPlay)
        {
            SRC.PlayOneShot(escapeSound);
            didntPlay = false;
        }

    }

    void DontDestroyThisObject()
    {
        if (SceneManager.GetActiveScene().name == "Story Mode" && LucasDeathManager.LucasLife == 0)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(this.gameObject);
        }
    }
}
