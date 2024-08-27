using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject mobileUI;

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            if (GameManager.isPaused)
            {
                mobileUI.SetActive(false);
            } else
            {
                mobileUI.SetActive(true);
            }
        }
    }
}
