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

    [Header("Sprite/Animation")]
    [SerializeField] private bool facingRight;

    [Header("Booleans")]
    [SerializeField] public bool isDead = false, squash = false;
    public bool isGoomba, isKoopa;

    [Header("Audio")]
    [SerializeField] private AudioSource audsrc;
    [SerializeField] private AudioClip deathSound, bumpSound;
    [SerializeField] private bool didntPlay = false;

    [Header("Movement")]
    [Tooltip("Change it to a value (0 = No movement, 1 = Go right, -1 = Go left)")]
    [SerializeField] private int direction = 1;
    [Tooltip("Default : 50")]
    [SerializeField] private float goombaSpeed = 50, koopaSpeed = 50, koopaShellSpeed = 150;

    [Header("Koopa")]
    [SerializeField] private Vector3 hitColliderSize, hitColliderOffset, hitColliderSizeMiddle, hitColliderOffsetMiddle;
    public bool isShell = false;
    public bool isRolling = false, rollingLeft, rollingRight;
    public bool hasExited;
    bool canExit;

    [SerializeField] private float timeToShellExit = 15f;

    [Header("Collision")]
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private List<LayerMask> wallLayer;
    [SerializeField] private LayerMask plyLayer;
    [SerializeField] private bool isTouchingWall;
    [SerializeField] public bool headIsTouched;
    [SerializeField] private float touchDetectCoolDown = 0.1f;
    [SerializeField] private float circleRadius;

    [Header("Prefabs")]
    [SerializeField] GameObject pointEffect; // after killed

    private void Awake()
    {
        sd = GetComponent<SelfDestruct>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        enemyAnim = GetComponentInChildren<Animator>();
        audsrc = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();

        // disable movement (enable after seen on camera renderer)
        rb.isKinematic = true;

        sd.enabled = false;
    }

    private void Start()
    {
        if (direction == 1)
        {
            facingRight = false;
            transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        }

        if (direction == -1)
        {
            facingRight = true;
        }
    }

    private void Update()
    {
        // put "if paused" here, otherwise the enemy will still move even if it's paused)
        if (!GameManager.isPaused)
        {
            AnimationsHandler();
            MovementSystem();
        }
    }

    void MovementSystem()
    {
        GoombaMovement(goombaSpeed);
        KoopaMovement(koopaSpeed, koopaShellSpeed);
    }

    void GoombaMovement(float movementSpeed)
    {
        if (!isGoomba)
            return;


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
            rb.AddForce(Vector2.right * direction * movementSpeed * Time.deltaTime);
        }

        if (direction == -1)
        {
            rb.AddForce(Vector2.right * direction * movementSpeed * Time.deltaTime);
        }
    }

    void KoopaMovement(float movementSpeed, float rollSpeed)
    {
        if (!isKoopa)
            return;

        // Wall detection
        isTouchingWall = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, wallLayer[0]) || Physics2D.OverlapCircle(transform.position - colliderOffset, circleRadius, wallLayer[0]);


        if (touchDetectCoolDown == 0.5f)
        {
            DirectionSwitch();
        }

        if (touchDetectCoolDown < 0.5f)
        {
            touchDetectCoolDown -= Time.deltaTime;
        }

        if (touchDetectCoolDown <= 0)
        {
            touchDetectCoolDown = 0.5f;
        }

        if (!isShell)
        {
             if (direction == 1)
             {
                 rb.AddForce(Vector2.right * direction * movementSpeed * Time.deltaTime);
             }

             if (direction == -1)
             {
                 rb.AddForce(Vector2.right * direction * movementSpeed * Time.deltaTime);
             }
        }
    
        if (isShell)
        {

           
            rollingLeft = Physics2D.OverlapBox(transform.position + hitColliderOffset, hitColliderSize, 0, plyLayer) || Physics2D.OverlapBox(transform.position + hitColliderOffsetMiddle, hitColliderSizeMiddle, 0, plyLayer);
            rollingRight = Physics2D.OverlapBox(new Vector3(transform.position.x - hitColliderOffset.x, transform.position.y + hitColliderOffset.y, transform.position.z - hitColliderOffset.z), hitColliderSize, 0, plyLayer);


            if (rollingRight && rollingLeft && !isRolling)
            {


                if (direction == -1)
                {
                    direction = 1;
                    Flip();
                }
                else
                {
                    direction = 1;
                }

                audsrc.PlayOneShot(deathSound);
                isRolling = true;
            }

            else if (rollingRight && !isRolling)
            {

                
                if (direction == -1)
                {
                    direction = 1;
                    Flip();
                }
                else
                {
                    direction = 1;
                }

                audsrc.PlayOneShot(deathSound);
                isRolling = true;
            }

            else if (rollingLeft && !isRolling)
            {


                if (direction == 1)
                {
                    direction = -1;
                    Flip();
                }
                else
                {
                    direction = -1;
                }

                audsrc.PlayOneShot(deathSound);
                isRolling = true;
            }

            if (isRolling)
            {
                if (direction == -1)
                {
                    rb.AddForce(Vector2.right * direction * rollSpeed * Time.deltaTime);
                }

                if (direction == 1)
                {
                    rb.AddForce(Vector2.right * direction * rollSpeed * Time.deltaTime);
                }
            }
        }
    }

 

    // Handle the death animations, 
    // Disable the Rigidbody,
    // Play death sound 
    void AnimationsHandler()
    {
        GoombaAnimations();
        KoopaAnimations();
    }

    void GoombaAnimations()
     {
        if (!isGoomba)
            return;

            if (isDead)
            {
                rb.isKinematic = true;
                rb.velocity = new Vector2(0, 0);

                if (!squash)
                {
                    enemyAnim.SetBool("dead", true);
                }
                else
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

    void KoopaAnimations()
    {
        if (!isKoopa)
            return;

        if (isDead)
        {
            rb.isKinematic = true;
            rb.velocity = new Vector2(0, 0);

            if (!squash)
            {
                enemyAnim.SetBool("dead", true);
            }
            else
            {
                sr.sortingLayerID = 1; // Change the Sprite renderer's sorting layer to 1.
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

        if (isShell)
        {
            if (!didntPlay)
            {
                enemyAnim.SetBool("shell", true);
                audsrc.PlayOneShot(deathSound);
                didntPlay = true;
            }

            if (!isRolling)
            {
                ExitShell();
            } else
            {
                enemyAnim.SetBool("exitshell", false);
                timeToShellExit = 15;
            }

        } else 
        {
            hasExited = false;
            enemyAnim.SetBool("shell", false);
            if (!isDead)
            {
                didntPlay = false;
            }
        }
    }

    void ExitShell()
    {
        if (!hasExited)
        {
            timeToShellExit -= Time.deltaTime;

            if (timeToShellExit >= 0 && timeToShellExit <= 5)
            {
                enemyAnim.SetBool("exitshell", true);
            }

            else if (timeToShellExit < 0)
            {
                enemyAnim.SetBool("exitshell", false);
                isShell = false;
                timeToShellExit = 15;
                hasExited = true;
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
                Flip();
                if (isKoopa && isShell)
                {
                    audsrc.PlayOneShot(bumpSound);
                }
            }

            else if (direction == -1)
            {
                direction = 1;
                touchDetectCoolDown -= 0.1f;
                Flip();
                if (isKoopa && isShell)
                {
                    audsrc.PlayOneShot(bumpSound);
                }
            }

        }
    }

    // Flip the enemy sprite on the Y Axis
    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    // Don't start moving until the enemy is visible on camera.
    // IMPORTANT NOTE : Viewing the enemies in Unity Scene Editor may already trigger this void. (so they start moving immediately on start while viewing on scene editor)
    private void OnWillRenderObject()
    {
        rb.isKinematic = false;
    }

    // Collision Stuff
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Death by player (MX or Pipe Crawler)
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Kill") && !collision.collider.gameObject.GetComponent<PlayerAnimation>().isFH || collision.collider.gameObject.layer == LayerMask.NameToLayer("Player") && !collision.collider.gameObject.GetComponent<PlayerAnimation>().isFH)
        {
            isDead = true;
            sd.timeToDestroy = 5f;
            sd.enabled = true;
        }

        // ignore Player collision if dead 
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player") && isDead || collision.collider.gameObject.layer == LayerMask.NameToLayer("Kill") && isDead || collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && !isRolling)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && isRolling)
        {
            collision.collider.gameObject.GetComponent<EnemyBehaviour>().isDead = true;
        }

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("whatIsVoid"))
        {
            Destroy(this.gameObject);
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
        Gizmos.DrawWireCube(transform.position + hitColliderOffset, hitColliderSize);
        Gizmos.DrawWireCube(new Vector3(transform.position.x - hitColliderOffset.x, transform.position.y + hitColliderOffset.y, transform.position.z - hitColliderOffset.z), hitColliderSize);
        Gizmos.DrawWireCube(transform.position + hitColliderOffsetMiddle, hitColliderSizeMiddle);
    }
}
