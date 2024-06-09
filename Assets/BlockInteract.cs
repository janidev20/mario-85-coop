using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInteract : MonoBehaviour
{
    [SerializeField] PlayerMovement movementScript;
    [SerializeField] GameObject BreakEffect;
    [SerializeField] GameObject EmptyBlock;
    [SerializeField] bool interacted;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip bumpEffect;

    [SerializeField] private LayerMask QMLayer;

    private void Update()
    {
    }


    private void OnCollisionExit2D(Collision2D collision)
    {

        if (!interacted && movementScript.headCollided)
        {
            StartCoroutine(PlayBumpEffect());
            {
                if (collision.gameObject.CompareTag("QM"))
                {

                    interacted = true;
                    //// PUT METHOD IN THE MIDDLE
                    Instantiate(EmptyBlock, collision.transform.position, Quaternion.identity);
                    Destroy(collision.gameObject);
                    ////
                    StartCoroutine(InteractCoolDown());
                }

                if (collision.gameObject.CompareTag("Breakable"))
                {

                    interacted = true;
                    //// PUT METHOD IN THE MIDDLE
                    Instantiate(BreakEffect, collision.transform.position, Quaternion.identity);
                    Destroy(collision.gameObject);
                    ////
                    StartCoroutine(InteractCoolDown());
                }

               
            }
        }
    }


    IEnumerator InteractCoolDown()
    {
        yield return new WaitForSeconds(0.3f);
        interacted = false;
    }

    IEnumerator PlayBumpEffect()
    {
            audio.PlayOneShot(bumpEffect);
            yield return new WaitForSeconds(0.3f);
            interacted = false;
    }
    
}
