using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private Collider2D collider;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SelfDestruct sd;

    [Header("Booleans")]
    [SerializeField] public bool isDead = false, squash = false;
    [SerializeField] private bool isGoomba, isKoopa;

    [Header("Audio")]
    [SerializeField] private AudioSource audsrc;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private bool didntPlay = false;

    [Header("Movement")]
    [SerializeField] private int direction = 1;
    [SerializeField] private float _goombaSpeed;
    
    [Header("Collision")]
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private List<LayerMask> wallLayer;
    [SerializeField] private bool isTouchingWall;
    [SerializeField] public bool headIsTouched;
    [SerializeField] private float touchDetectCoolDown = 2;
    [SerializeField] private float circleRadius;

    [Header("Prefabs")]
    [SerializeField] GameObject pointEffect; // after killed 

    private void Awake()
    {
        sd = GetComponent<SelfDestruct>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        enemyAnim = GetComponentInChildren<Animator>();
        audsrc = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();

        rb.isKinematic = true;


        direction = 1;
        sd.enabled = false;
    }

    private void Update()
    {
        if (!GameManager.isPaused)
        {
            AnimationsHandler();
            MovementSystem(35);

        }

    }

    void MovementSystem(float goombaSpeed)
    {
        if (isGoomba)
        {
            goombaSpeed = _goombaSpeed;

            // touch detect
            isTouchingWall = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, wallLayer[0]) || Physics2D.OverlapCircle(transform.position - colliderOffset, circleRadius, wallLayer[0]);
        
            
            if (touchDetectCoolDown == 2)
            {
                DirectionSwitch();
            }

            if (touchDetectCoolDown < 2)
            {
                touchDetectCoolDown -= Time.deltaTime;
            }
            
            if (touchDetectCoolDown <= 0)
            {
                touchDetectCoolDown = 2;
            } 
        
            if (direction == 1)
            {
                rb.AddForce(Vector2.right * direction * goombaSpeed);
            }

            if (direction == -1)
            {
                rb.AddForce(Vector2.right * direction * goombaSpeed);
            }
        }
    }

    void AnimationsHandler()
    {
        if (isGoomba)
        {
            if (isDead)
            {
                rb.isKinematic = true;
                rb.velocity = new Vector2(0, 0);

                if (!squash)
                {
                    enemyAnim.SetBool("dead", true);
                } else
                {
                    sr.sortingLayerID = 1; // Change the Sprite renderer's sorting layer to 1.
                    enemyAnim.SetBool("squashDead", true);
                    sd.timeToDestroy = 1.5f;
                    sd.enabled = true;

                }

               
                if (!didntPlay)
                {
                    Instantiate(pointEffect, transform.position, Quaternion.identity);
                    audsrc.PlayOneShot(deathSound);
                    didntPlay = true;
                }
            }
        }
    }

    void DirectionSwitch()
    {
        if (isTouchingWall)
        {

            
            if (direction == 1)
            {
                direction = -1;
                touchDetectCoolDown -= 0.1f;
            }

            else if (direction == -1)
            {
                direction = 1;
                touchDetectCoolDown -= 0.1f;
            }

        }
    }

    private void OnWillRenderObject()
    {
        rb.isKinematic = false;
    }

    // Collision Stuff
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player") && !collision.collider.gameObject.GetComponent<PlayerAnimation>().isFH)
        {
            isDead = true;
            sd.timeToDestroy = 5f;
            sd.enabled = true;
        }


        // ignore Player collision if dead 
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player") && isDead)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + colliderOffset, circleRadius);
        Gizmos.DrawSphere(transform.position - colliderOffset, circleRadius);
    }
}
