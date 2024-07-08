using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VoiceManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] FHVoices, PipeCrawlerVoices, MXVoices, WahooJumpSound, FallSound;
    [SerializeField] private AudioSource SRC;
    [SerializeField] private bool canPlay = true;
    [SerializeField] private bool canPlayEvent = true;


    public void RandomFH()
    {
        if (canPlay)
        {
            SRC.clip = FHVoices[Random.Range(0, FHVoices.Length)];

            SRC.Play();
            canPlay = false;
            StartCoroutine(VoiceCoolDown(3));
        }
    }

    public void RandomPC()
    {
        if (canPlay)
        {
            SRC.clip = PipeCrawlerVoices[Random.Range(0, PipeCrawlerVoices.Length)];

            SRC.Play();
            canPlay = false;
            StartCoroutine(VoiceCoolDown(3));
        }
    }

    public void RandomMX()
    {
        if (canPlay)
        {
            SRC.clip = MXVoices[Random.Range(0, MXVoices.Length)];

            SRC.Play();
            canPlay = false;
            StartCoroutine(VoiceCoolDown(3));
        }
    }

    public void Fall()
    {
        if (canPlayEvent)
        {
            SRC.clip = FallSound[Random.Range(0, FallSound.Length)];

            SRC.Play();
            canPlayEvent = false;
            StartCoroutine(EventCoolDown(1.5f));
        }
    }

    public void WahooJump()
    {
        if (canPlayEvent)
        {
            SRC.clip = WahooJumpSound[Random.Range(0, WahooJumpSound.Length)];

            SRC.Play();
            canPlayEvent = false;
            StartCoroutine(EventCoolDown(1));
        }
    }

    // Used for WahooJump and Fall
    IEnumerator EventCoolDown(float time)
    {
        canPlayEvent = false;

        yield return new WaitForSeconds(time);

        canPlayEvent = true;
    }

    IEnumerator VoiceCoolDown(float time)
    {
        canPlay = false;
        

        yield return new WaitForSeconds(time);

        canPlay = true;
   
    }
}
