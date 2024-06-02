using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInteract : MonoBehaviour
{
    [SerializeField] PlayerMovement movementScript;
    [SerializeField] bool interacted;

    private void Update()
    {
        
    }

    void BlockDetect()
    {
       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (!interacted && movementScript.headCollided)
        {
            {
                if (collision.gameObject.CompareTag("Breakable"))
                {

                    interacted = true;
                    collision.gameObject.SetActive(false);
                    StartCoroutine(InteractCoolDown());
                }
            }
        }
    }


    IEnumerator InteractCoolDown()
    {
        yield return new WaitForSeconds(0.1f);
        interacted = false;
    }
}
