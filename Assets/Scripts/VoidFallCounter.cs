using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidFallCounter : MonoBehaviour
{
    public static int fellInVoidAmount;
    public static bool fellInVoid;
    public static bool LucasFellInVoid;
     bool firstTime = true;


    // Start is called before the first frame update
    private void Start()
    { 
        if (SceneManager.GetActiveScene().name == "Story Mode")
        firstTime = true;
    }

    // Update is called once per frame
    void Update()
    {
       if (fellInVoid && firstTime)
        {
            fellInVoidAmount += 1;
           firstTime = false;
        }

       DontDestroyThisObject();
    }

    void DontDestroyThisObject()
    {
        if (SceneManager.GetActiveScene().name == "Story Mode" && LucasDeathManager.LucasLife == 0 || SceneManager.GetActiveScene().name == "Story Mode" && LucasEscape.Escaped)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            fellInVoidAmount = 0;
            fellInVoid = false;
            LucasFellInVoid = false;
            Destroy(this.gameObject);
        }
    }
}
