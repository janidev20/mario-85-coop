using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneTips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tipTxt;
    [SerializeField] private List<string> tipList;
    int randomNumber; // For choosing a random tip

    private void Start()
    {
        randomNumber = Random.Range(0, tipList.Count);
    }
 
    private void Update()
    {
        TipsHandling();
    }

    // The collection of the pro tips showed in the Loading Screen.
    void TipsHandling()
    {
        tipTxt.text = tipList[randomNumber];
    }

}
