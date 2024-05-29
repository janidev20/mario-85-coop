using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneTips : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tipTxt;
    int randomNumber; // For choosing a random tip

    private void Start()
    {
        randomNumber = Random.Range(0, 6);
    }

    // The collection of the pro tips showed in the Loading Screen.
    private void Update()
    {
        if (randomNumber == 0)
        {
            tipTxt.text = "pro tip : press 'v' in the void";
        }
        else if (randomNumber == 1)
        {
            tipTxt.text = "pro tip : don't fall in the void";
        }
        else if (randomNumber == 2)
        {
            tipTxt.text = "pro tip : destroy obstacles";
        }
        else if (randomNumber == 3)
        {
            tipTxt.text = "pro tip : smile!";
        }
        else if (randomNumber == 4)
        {
            tipTxt.text = "pro tip : don't let lucas escape";
        }
        else if (randomNumber == 5)
        {
            tipTxt.text = "pro tip : go take a break";
        }
        else if (randomNumber == 6)
        {
            tipTxt.text = "pro tip : mario is in your house";
        }
    }

}
