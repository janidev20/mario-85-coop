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
    public bool isSprinting;

    [Header("Vertical Movement")]
    public float jumpSpeedMX = 15f, jumpSpeedPC = 12.5f, jumpSpeedFH = 10f; //the jump force based on what character the player is
    public float jumpSpeed;
    public float jumpCoolDown = 0.35f;
   [SerializeField] private float jumpStartTime;
   [SerializeField] private float jumpTime;
   [SerializeField] [HideInInspector] private bool _isJumping = false;
    private bool _isWahooJumping;
    private bool _isFalling;

    [Header("Components")]
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public LayerMask voidLayer;
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
    public bool onVoid = false;
    public bool headCollided;
    public float circleRadius = 0.15f;
    public Vector3 colliderOffset;

    [Header("Character Animator")]
    [SerializeField] private PlayerAnimation AnimationScript;
    public bool isRunning;
    public bool isMoving;
    public bool isFalling;
    public bool isJumping;
    public bool isWahooJumping;
    public bool isSliding;
    public bool isCrouching => Input.GetKey(KeyCode.DownArrow) && onGround && Input.GetAxis("Horizontal") <= 0.9f && Input.GetAxis("Horizontal") >= -0.9f; // Is Crouchig Boolean depending on input and smoothing input. The horizontal input thingys here are crucial to smooth movement.


    [Header("Audio")]
    [SerializeField] private AudioSource jumpSrc;
    [SerializeField] private AudioClip mxJump, pcJump, fhJump, wahooJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultSpeed = moveSpeed;
        maxDefaultSpeed = maxSpeed;
    }

    
    void Update()
    {
            if (!isMoving && isCrouching) // Can't move if crouching
                return;

            if (onGround)
            {
                if (Input.GetKey(KeyCode.X))
                {
                    isSprinting = true;
                }
                else
                {
                    isSprinting = false;
                }
            }

            // Speed change
            if (AnimationScript.isMX)  
            {
                maxSpeed = 6;
                maxSprintSpeed = 8;
            }

            else if (AnimationScript.isPCrawler)
            {
                maxSpeed = 5.25f;
                maxSprintSpeed = 7.25f;
            }
            else if (AnimationScript.isFH)
            {
                maxSpeed = 4;
                maxSprintSpeed = 6;

            }

            //Run Animation boolean
            isRunning = isSprinting;

            // Ground Detection
            bool wasOnGround = onGround;
            onGround = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, groundLayer);
            onVoid = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, voidLayer);
            
            // Head Bump Detection (When mario hits something with his head/hand)
            headCollided = Physics2D.OverlapCircle(transform.position - colliderOffset, circleRadius, groundLayer); // This is to indicate if mario's head bumped into something

            if (headCollided) // If it did, 
            {
                jumpTime = 0; // stop jumping. 
            }

           

            // The default and WahooJump used as 2 seperate methods. Y input is in Jump() and V input is in WahooJump() ONLY. 
            if (!AnimationScript.isTransforming)
            {
                Jump();
                WahooJump();        
           }

            // Direction Input Detection (observable in the Inspector)
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


            // Change movement speed depending on Sprinting Boolean
            if (isSprinting)
            {
                moveSpeed = sprintSpeed;
                maxSpeed = maxSprintSpeed;
            }
            else
            {
                moveSpeed = defaultSpeed;
                maxSpeed = maxDefaultSpeed;
            }

            if (isCrouching)
            {

                // Changing Collider size based on Current Form (MX, FH, PCrawler)
                // Comment 2 : I HONESTLY HAVE NO FUCKING IDEA WHAT I DID HERE BUT IT SEEMS TO WORK, DO NOT TOUCH IT!!!!!
                if (AnimationScript.isFH)
                {
                    moveSpeed = 0;
                    collider.offset = new Vector2(0.008428961f, -0.4910533f);
                    collider.size = new Vector2(0.7583675f, 1.012235f);
                }
                else if (AnimationScript.isPCrawler || AnimationScript.isFH)
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



            // Some Animation Debug for Sliding (Prevents slide animation stuttering)
            if (direction.x != 0)
            {

                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            // If the transform animation is not playing, OR we're on the void we are transforming (into MX) then  we can move
            if (!AnimationScript.isTransforming || AnimationScript.isTransforming && onVoid)
            {
                bool changingDirections = (direction.x > 0 && rb.velocity.x < -0.3f) || (direction.x < 0 && rb.velocity.x > 0.3f);

                // Slide Animation Boolean
                if (changingDirections || changingDirections && isMoving == false)
                {
                    isSliding = true;
                }
                else
                {
                    isSliding = false;
                }
            }
            else
            {
                // If we ARE transforming, then set X velocity to 0 and keep the Y velocity at it's default pace (This prevents MX from floating in mid-air while transforming on a floating block as he destroys it)
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            // If sliding, increase linear drag
            if (isSliding)
            {
                linearDrag = 1.75f;
            }
            else
            {
                linearDrag = 5;
            }

        // jump squeeze couroutine method (no idea what this is but don't touch it maybe)
        if (!wasOnGround && onGround)
        {
            StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        }
    }
    void FixedUpdate()
    {
            moveCharacter(direction.x);
            modifyPhysics();  
      }

    // Movement Logic
    void moveCharacter(float horizontal)
    {
        if (!isCrouching)
        {
            rb.AddForce(Vector2.right * horizontal * moveSpeed);   

            if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
            {
                // Flip() only when on ground, and is FH (False Hero).
                if (onGround && AnimationScript.isFH)
                {
                    Flip();
                }
                else if (!AnimationScript.isFH)
                {
                    Flip();
                }
            }
            
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }
    }
    void Jump()
    {
        // OLD JUMP METHOD //
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        //rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        //jumpTimer = 0;
        //StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));

            if (onGround && Input.GetKeyDown(KeyCode.Y) && !onVoid)
            {
                    _isWahooJumping = false;
                    _isJumping = true;
                    jumpTime = jumpStartTime;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                    StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));

                    if (AnimationScript.isMX)
                    {
                        jumpSpeed = jumpSpeedMX;
                        jumpSrc.PlayOneShot(mxJump);
                    }

                    else if (AnimationScript.isFH)
                    {
                        jumpSpeed = jumpSpeedFH;
                        jumpSrc.PlayOneShot(fhJump);
                    }

                    else if (AnimationScript.isPCrawler)
                    {
                        jumpSpeed = jumpSpeedPC;
                        jumpSrc.PlayOneShot(pcJump);
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

        // This prevents the bug that often makes mario skip a jump animation (DO NOT FUCKING TOUCH THIS TOOK ME HOURS TO FIX)
        if (Input.GetKeyUp(KeyCode.Y))
        {
            _isJumping = false;
            jumpTime = 0;
        }
        
        if (!onGround)
        {
            _isJumping = true;
        }
    }

    void WahooJump()
    {
        // OLD JUMP METHOD //
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        //rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        //jumpTimer = 0;
        //StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));


        if (onVoid && Input.GetKeyDown(KeyCode.V) && AnimationScript.isMX)
        {
            _isJumping = true;
            _isWahooJumping = true;
            /////////////////////////////////
            jumpSpeed = jumpSpeedMX * 2.95f;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpSrc.PlayOneShot(wahooJump);
            jumpSrc.PlayOneShot(mxJump);
            /////////////////////////////////
            StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
        }

        else if (Input.GetKey(KeyCode.V) && AnimationScript.isMX && _isJumping == true)
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

    // Crucial for the player physics and movement. In summary, the method holds all the functions for managing gravity and the jump booleans
    void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            if (jumpTime <= 0.015f)
            {
                jumpTime = 0;
            }

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
           isFalling = false;
        }

        else
        {
            _isFalling = true;
           


            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;


            if (_isWahooJumping)
            {
                isWahooJumping = true;
            } else if (_isJumping)
            {
                isJumping = true;
            } else
            {
                isFalling = true;
            }
            

            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Y))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }

       
    }

    // Flips the player sprite when changing Directions.
    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    // No idea what it does, something with jumping, just leave it.
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

}
