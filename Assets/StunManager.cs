using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] List<GameObject> KillTriggers;

    [Header("Booleans")]
    public bool inStunProximity;
    public static bool isStunned = false;

    [Header("Audio")]
    [SerializeField] AudioSource SRC;
    [SerializeField] AudioClip StunSound;

    private void Update()
    {
        IsNearStun(isStunned);
        if (Input.GetKeyDown(KeyCode.K))
        {

            Stun();
        }
    }


    void IsNearStun(bool stunned)
    {
        if (stunned)
        {
            for (int i = 0; i < KillTriggers.Count; i++)
            {
                KillTriggers[i].SetActive(false);
            }
        }

        else
        {
            for (int i = 0; i < KillTriggers.Count; i++)
            {
                KillTriggers[i].SetActive(true);
            }
        }
    }

    public void Stun()
    {
        StartCoroutine(StunCoolDown());
    }

    IEnumerator StunCoolDown()
    {
        isStunned = true;

        SRC.PlayOneShot(StunSound);

        yield return new WaitForSeconds(1.75f);

        isStunned = false;
    }

}
