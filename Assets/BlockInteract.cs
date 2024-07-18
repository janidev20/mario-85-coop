using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerMovement))]
public class BlockInteract : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] [HideInInspector] private float circleRadius = 0.15f; // circleRadius will be changed according to what we are (FH, Pcrawelr , MX)
    [SerializeField] private float circleRadiusSmall = 0.15f, circleRadiusMX = 0.8f;
    [SerializeField] private List<LayerMask> blockLayer; // THE 0TH ELEMENT SHOULD ALWAYS BE "block"!!!!!
    public bool headCollided;
    bool cooldown; // for collision cooldown

    [Header("Offset and stuff")]
    [SerializeField] private Vector3 headColliderOffset;
    [SerializeField] private Vector3 headColliderBoxOffset;
    [SerializeField] private Vector3 headColliderBoxSize;
    [SerializeField] private float headColliderCircleRadius = 0.15f;

    [Header("References")]
    [SerializeField] private PlayerAnimation AnimationScript;
    [SerializeField] private PlayerMovement MovementScript;

    [Header("BlockBreak")]
    [SerializeField] GameObject breakEffect;

    private void Update()
    {
        DetectCollision();
        CircleRadiusManage();
        StartCoroutine(BlockBreak());
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

    IEnumerator BlockBreak()
    {
        // Get the object's collider that the overlapbox is colliding with.
        Collider2D hit = Physics2D.OverlapBox(transform.position - headColliderBoxOffset, headColliderBoxSize, 0, blockLayer[0]);

        // If the collided object's layer name is "BrickBlock"...
        if (hit.gameObject.layer == LayerMask.NameToLayer("BrickBlock"))
        {
            yield return new WaitForSeconds(0.035f);

            //... Destroy it and
            Destroy(hit.gameObject);
            // Spawn down epic break effect.
            Instantiate(breakEffect, hit.transform.position, Quaternion.identity);
        }



    }

    void DetectCollision()
        {
            // Head Bump Detection(When mario hits something with his head)
            headCollided = Physics2D.OverlapBox(transform.position - headColliderBoxOffset, headColliderBoxSize, 0, blockLayer[0]); // This is to indicate if mario's head bumped into something


            if (headCollided) // If it did, 
            {
                StartCoroutine(CollideCooldown());
                Debug.Log("Head Collided.");
            }
        }

    // This is a timer based cooldown system so that the 'headCollided' boolean doesn't count more than it has to.
    IEnumerator CollideCooldown()
        {
            if (headCollided)
            {
                cooldown = true;
            }

            yield return new WaitForSeconds(0.15f);

            cooldown = false;
        }
}
