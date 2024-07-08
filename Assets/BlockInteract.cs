using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
public class BlockInteract : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] [HideInInspector] private float circleRadius = 0.15f; // circleRadius will be changed according to what we are (FH, Pcrawelr , MX)
    [SerializeField] private float circleRadiusSmall = 0.15f, circleRadiusMX = 0.8f;
    [SerializeField] private Vector3 headColliderOffset;
    [SerializeField] private List<LayerMask> blockLayer; // THE 0TH ELEMENT SHOULD ALWAYS BE "block"!!!!!
    public bool headCollided;
    bool cooldown; // for collision cooldown

    [Header("References")]
    [SerializeField] private PlayerAnimation AnimationScript;

    [Header("BlockBreak")]
    [SerializeField] GameObject breakEffect;

    private void Update()
    {
        DetectCollision();
        CircleRadiusManage();
    }

    void CircleRadiusManage()
    {
        if (AnimationScript.isFH || AnimationScript.isPCrawler)
        {
            circleRadius = circleRadiusSmall;
        }
        else if (AnimationScript.isMX)
        {
            circleRadius = circleRadiusMX;
        }
    }

    void DetectCollision()
    {
        // Head Bump Detection(When mario hits something with his head)
        headCollided = Physics2D.OverlapCircle(transform.position - headColliderOffset, circleRadius - 0, blockLayer[0]) && !cooldown; // This is to indicate if mario's head bumped into something

        if (headCollided) // If it did, 
        {
            StartCoroutine(CollideCooldown());
            Debug.Log("Head Collided.");
        }
    }

    IEnumerator CollideCooldown()
    {
        if (headCollided)
        {
            cooldown = true;
        }

        yield return new WaitForSeconds(0.15f);

        cooldown = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Breakable" && collision.collider.gameObject.layer == LayerMask.NameToLayer("brickBlock"))
        {
            if (headCollided)
            {
                Destroy(collision.gameObject);
                Instantiate(breakEffect, collision.transform.position, Quaternion.identity);
            }
        }
    }
}
