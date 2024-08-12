using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LucasLivesManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI lucasLifeTitle;

    private void Update()
    {
        LucasLivesChange();
    }

    void LucasLivesChange()
    {
        lucasLifeTitle.text = "x " + LucasDeathManager.LucasLife;
    }
}
