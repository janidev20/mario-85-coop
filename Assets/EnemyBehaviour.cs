using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private Collider2D collider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SelfDestruct sd;

    [Header("Booleans")]
    [SerializeField] private bool isDead=false;
    [SerializeField] private bool isGoomba, isKoopa;

    [Header("Audio")]
    [SerializeField] private AudioSource audsrc;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private bool didntPlay = false;

    [Header("Movement")]
    [SerializeField] private int direction = 1;
    
    [SerializeField] private bool isTouchingWall;
    [SerializeField] private float touchDetectCoolDown = 2;

    [SerializeField] private float circleRadius;
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private List<LayerMask> wallLayer;

    private void Awake()
    {
        sd = GetComponent<SelfDestruct>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        enemyAnim = GetComponent<Animator>();
        audsrc = GetComponent<AudioSource>();

        direction = 1;
        sd.enabled = false;
    }

    private void Update()
    {
        AnimationsHandler();
        MovementSystem(35);
    }

    void MovementSystem(float goombaSpeed)
    {
        if (isGoomba)
        {
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
                Destroy(collider);
                Destroy(rb);
                enemyAnim.SetBool("dead", true);
                if (!didntPlay)
                {
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

    // Collision Stuff
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            isDead = true;
            sd.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + colliderOffset, circleRadius);
        Gizmos.DrawSphere(transform.position - colliderOffset, circleRadius);
    }
}
