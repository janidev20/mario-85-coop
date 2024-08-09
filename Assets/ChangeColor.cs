using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeColor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI text;


    public void ChangRed()
    {
        text.color = Color.red;
    }

    public void ChangeWhite()
    {
        text.color = Color.white;
    }
}
