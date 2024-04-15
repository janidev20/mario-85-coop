using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public float sprintSpeed = 16f;
    public float defaultSpeed;
    public Vector2 direction;
    private bool facingRight = true;
    public bool isSprinting => Input.GetKey(KeyCode.X);

    [Header("Vertical Movement")]
    public float jumpSpeedMX = 15f, jumpSpeedPC = 12.5f, jumpSpeedFH = 10f; //the jump force based on what character the player is
    public float jumpSpeed;
   [SerializeField] private float jumpStartTime;
   [SerializeField] private float jumpTime;
   [SerializeField] [HideInInspector] private bool _isJumping = false;
    private bool _isWahooJumping;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;
    public BoxCollider2D collider;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float maxSprintSpeed = 12f;
    public float maxDefaultSpeed;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float circleRadius = 0.15f;
    public Vector3 colliderOffset;

    [Header("Character Animator")]
    [SerializeField] private PlayerAnimation AnimationScript;
    public bool isRunning;
    public bool isMoving;
    public bool isJumping;
    public bool isWahooJumping;
    public bool isSliding;
    public bool isCrouching => Input.GetKey(KeyCode.DownArrow) && onGround && Input.GetAxis("Horizontal") <= 0.9f && Input.GetAxis("Horizontal") >= -0.9f;


    [Header("Audio")]
    [SerializeField] private AudioSource jumpSrc;
    [SerializeField] private AudioClip mxJump, pcJump, fhJump, wahooJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultSpeed = moveSpeed;
        maxDefaultSpeed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Speed change
        if (AnimationScript.isMX)
        {
            maxSpeed = 6;
            maxSprintSpeed = 8;
        } else if (AnimationScript.isPCrawler)
        {
            maxSpeed = 5.25f;
            maxSprintSpeed = 7.25f;
        } else if (AnimationScript.isFH)
        {
            maxSpeed = 4;
            maxSprintSpeed = 6;

        }


        isRunning = isSprinting;

        bool wasOnGround = onGround;
        onGround = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, groundLayer);

        if (!wasOnGround && onGround)
        {
            StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        }

        Jump();
        WahooJump();
        animator.SetBool("onGround", onGround);
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        

        if (isSprinting)
        {
            moveSpeed = sprintSpeed;
            maxSpeed = maxSprintSpeed;
        } else
        {
            moveSpeed = defaultSpeed;
            maxSpeed = maxDefaultSpeed;
        }

        if (isCrouching)
        {

            // Changing Collider size based on Current Form (MX, FH, PCrawler)
            if (AnimationScript.isFH)
            {
                moveSpeed = 0;
                collider.offset = new Vector2(0.008428961f, -0.4910533f);
                collider.size = new Vector2(0.7583675f, 1.012235f);
            } else if (AnimationScript.isPCrawler || AnimationScript.isFH)
            {
                moveSpeed = 0;
                collider.offset = new Vector2(0.04976797f, -0.480615f);
                collider.size = new Vector2(0.9978762f, 1.017434f);
          //  } else if (AnimationScript.isMX)
          //  {
            //    moveSpeed = 0;
         //       collider.offset = new Vector2(-0.001805902f, -0.04933187f);
          //      collider.size = new Vector2(1.844248f, 1.994027f);
          }
            
        }
        else
        {
           

       //     if (AnimationScript.isFH)
       //     {
        //        collider.offset = new Vector2(0.0118157f, -0.2317408f);
       //         collider.size = new Vector2(0.836907f, 1.629209f);
       //     }
            if (AnimationScript.isPCrawler || AnimationScript.isFH)
            {
                collider.offset = new Vector2(0.04976797f, -0.238775f);
                collider.size = new Vector2(0.9978762f, 1.501114f);
            }
            else if (AnimationScript.isMX)
            {
                collider.offset = new Vector2(-0.001805902f, 0.2736198f);
                collider.size = new Vector2(1.844248f, 2.63993f);
            }
        }
       



        if (direction.x != 0)
        {

            isMoving = true;
        }
        else
        {
            StartCoroutine("waitForRun", 0);
        }

       // If the transform animation is not playing, then  we can move
        if (!AnimationScript.isTransforming)
        {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < -0.3f) || (direction.x < 0 && rb.velocity.x > 0.3f);

       // Sliding Animator Bool
        if (changingDirections || changingDirections && isMoving == false)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }
        } else
        {
            rb.velocity = Vector2.zero;
        }

        // If sliding, increase linear drag
        if (isSliding)
        {
            linearDrag = 1.75f;
        } else
        {
            linearDrag = 5;
        }
    }
    void FixedUpdate()
    {
        if (!AnimationScript.isTransforming)
        {

            moveCharacter(direction.x);

            // OLD JUMP METHOD //

           // if (jumpTimer > Time.time && onGround)
           // {
             //   if (AnimationScript.isMX)
             //   {
                    // WAHOO JUMP
               //     if (Input.GetKey(KeyCode.V))
               //     {
               //         jumpSpeed = jumpSpeedMX * 1.7f;
              //          Jump();
              //          jumpSrc.PlayOneShot(wahooJump);
              //      } else

              //      jumpSpeed = jumpSpeedMX;
             //       Jump();
             //       jumpSrc.PlayOneShot(mxJump);
             //   } else if (AnimationScript.isPCrawler)
            //    {
             //      jumpSpeed = jumpSpeedPC;
             //      Jump();
             //      jumpSrc.PlayOneShot(pcJump);
             //  } else if (AnimationScript.isFH)
             //  {
             //      jumpSpeed = jumpSpeedFH;
             //      Jump();
             //      jumpSrc.PlayOneShot(fhJump);
             //  }

           // }

          modifyPhysics();

       }
      }
    void moveCharacter(float horizontal)
    {

        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            

            if (onGround && AnimationScript.isFH)
            {
                Flip();
            } else if (!AnimationScript.isFH)
            {
                Flip();
            } 
        } 
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("vertical", rb.velocity.y);
    }
    void Jump()
    {
        // OLD JUMP METHOD //
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        //rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        //jumpTimer = 0;
        //StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
    
        if (onGround && Input.GetKeyDown(KeyCode.Y))
        {
            _isWahooJumping = false;
            if (AnimationScript.isMX)
            {
                _isJumping = true;
                jumpTime = jumpStartTime;
                /////////////////////////////////
                jumpSpeed = jumpSpeedMX;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                jumpSrc.PlayOneShot(mxJump);
                /////////////////////////////////
                StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
            }
            else if (AnimationScript.isFH)
            {
                _isJumping = true;
                jumpTime = jumpStartTime;
                /////////////////////////////////
                jumpSpeed = jumpSpeedFH;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                jumpSrc.PlayOneShot(fhJump);
                /////////////////////////////////
                StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
            } else if (AnimationScript.isPCrawler)
            {
                _isJumping = true;
                jumpTime = jumpStartTime;
                /////////////////////////////////
                jumpSpeed = jumpSpeedPC;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                jumpSrc.PlayOneShot(pcJump);
                /////////////////////////////////
                StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
            }
        } 

        if (Input.GetKey(KeyCode.Y) && _isJumping == true)
        {
            if (jumpTime > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            _isJumping = false;
        }
    }

    void WahooJump()
    {
        // OLD JUMP METHOD //
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        //rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        //jumpTimer = 0;
        //StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));

        if (onGround && Input.GetKeyDown(KeyCode.V) && AnimationScript.isMX)
        {
            _isJumping = true;
            _isWahooJumping = true;
            /////////////////////////////////
            jumpSpeed = jumpSpeedMX * 2.95f;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpSrc.PlayOneShot(wahooJump);
            /////////////////////////////////
            StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
        }

        if (Input.GetKey(KeyCode.V) && AnimationScript.isMX && _isJumping == true)
        {
            if (jumpTime > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.V) && AnimationScript.isMX)
        {
            _isJumping = false;
        }
    }

    void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
           isJumping = false;
           isWahooJumping = false;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;

            if (_isWahooJumping)
            {
                isWahooJumping = true;
            } else
            {
                isJumping = true;
            }
            

            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Z))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }

       
    }
    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + colliderOffset, circleRadius);
    }

    IEnumerator waitForRun()
    {
            yield return new WaitForSeconds(0.2f);
            isMoving = false;

    }

}
