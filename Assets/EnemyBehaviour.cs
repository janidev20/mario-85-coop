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
    [Tooltip("Change it to a value (0 = No movement, 1 = Go right, -1 = Go left)")]
    [SerializeField] private int direction = 1;
    [Tooltip("Default : 50")]
    [SerializeField] private float _goombaSpeed = 50;
    
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

        // disable movement (enable after seen on camera renderer)
        rb.isKinematic = true;

        sd.enabled = false;
    }

    private void Update()
    {
        // put "if paused" here, otherwise the enemy will still move even if it's paused)
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

            // Wall detection
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

    // Handle the death animations, 
    // Disable the Rigidbody,
    // Play death sound 
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

    // Switch the direction if colliding with a wall
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

    // Don't start moving until the enemy is visible on camera.
    private void OnWillRenderObject()
    {
        rb.isKinematic = false;
    }

    // Collision Stuff
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Death by player (MX or Pipe Crawler)
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

    // If the player headbumps a block which the enemy is standing on, the break effect with a "Kill" layer has a -> Trigger Collider.
    // If the enemy collides with this, initiate the death.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Kill"))
        {
            isDead = true;
            sd.timeToDestroy = 5f;
            sd.enabled = true;
        }
    }

    // Draw the Sphere detections on Unity Preview
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + colliderOffset, circleRadius);
        Gizmos.DrawSphere(transform.position - colliderOffset, circleRadius);
    }
}
