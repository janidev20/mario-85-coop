using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PPManager : MonoBehaviour
{
    [SerializeField] GameObject PP;
    private bool PPEnabled = true;

    private void Update()
    {
        HandlePP();
    }

    void HandlePP()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PPEnabled = !PPEnabled;
        }

        if (PPEnabled)
        {
            PP.SetActive(true);
        }
        else
            PP.SetActive(false);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
