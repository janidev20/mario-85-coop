using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{

    [SerializeField] private float volume;
    [SerializeField] AudioSource bgMusic;
    private bool musicEnabled = true;

    private void Start()
    {
        AudioListener.volume = 0.25f;
    }

    private void Update()
    {
        volume = AudioListener.volume;

        if (Input.GetKeyDown(KeyCode.Minus) && volume >= 0.0f || Input.GetKeyDown(KeyCode.KeypadMinus) && volume >= 0.0f)
        {
            AudioListener.volume -= 0.25f;
        } 

        if (Input.GetKeyDown(KeyCode.Plus) && volume <= 1.0f || Input.GetKeyDown(KeyCode.KeypadPlus) && volume <= 1.0f)
        {
            AudioListener.volume += 0.25f;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            musicEnabled = !musicEnabled;
        }

        if (musicEnabled)
        {
            bgMusic.enabled = true;
        } else
        {
            bgMusic.enabled = false;
        }
    }
}
