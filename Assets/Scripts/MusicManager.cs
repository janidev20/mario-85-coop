using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    [Header("Scripts")]
    [SerializeField] private PlayerMovement moveScript;
    [SerializeField] private PlayerAnimation animScript;

    [Header("Audio")]
    [SerializeField] private AudioSource SRC;
    [SerializeField] private AudioClip HappyAmbient, ScaryAmbient;

    [Header("Booleans")]
    bool isHappy = false;

    private void Start()
    {
        SRC.clip = ScaryAmbient;
        SRC.mute = false;
    }

    private void Update()
    {
        if (LucasDeathManager.needToRestart)
        {
            SRC.mute = true;
        } else

        CheckForInput(); // on pressing "M" > change music
        CheckState(); // Check if player is falling OR on the void (mute the music)
    }

    void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isHappy = !isHappy;
        }
    }

    void CheckState()
    {

        if ((moveScript.isFalling && !moveScript.isJumping && !moveScript.isWahooJumping || moveScript.onVoid) && !animScript.isFH)
        {
            SRC.mute = true;
        }
        else
        {
            SRC.mute = false;
        }
    }
}
