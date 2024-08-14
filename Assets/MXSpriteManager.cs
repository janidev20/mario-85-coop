using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MXSpriteManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (MXCutsceneManager.pause)
        {
            sr.enabled = false;
        } else
        {
            sr.enabled = true;
        }
    }
}
