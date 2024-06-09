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
    [SerializeField] private float sprintSpeed = 20f ;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] [HideInInspector] private float moveSpeed = 10f;
    [SerializeField] [HideInInspector] private bool speedUp = false;
    [SerializeField] [HideInInspector] private bool facingRight = true;
    public bool isSprinting;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpSpeedMX = 15f, jumpSpeedPC = 12.5f, jumpSpeedFH = 10f; //the jump force based on what character the player is
    [SerializeField] [HideInInspector] private bool _isJumping = false;
    [SerializeField] [HideInInspector] private float jumpStartTime;
    [SerializeField] [HideInInspector] private float jumpTime;
    [SerializeField] [HideInInspector] private float jumpSpeed;
    [SerializeField] [HideInInspector] private float jumpCoolDown = 0.1f;
    [SerializeField] [HideInInspector] private bool _isWahooJumping;
    [SerializeField] [HideInInspector] private bool _isFalling;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask voidLayer;
    [SerializeField] private GameObject characterHolder;
    [SerializeField] private BoxCollider2D collider;


    [Header("Physics")]
    [SerializeField] [HideInInspector] private float maxSpeed = 7f;
    [SerializeField] [HideInInspector] private float maxSprintSpeed = 12f, maxSprintSpeedBIG = 10f;
    [SerializeField] [HideInInspector] private float maxDefaultSpeed;
    [SerializeField] [HideInInspector] private float linearDrag = 4f;
    [SerializeField] [HideInInspector] private float gravity = 1f;
    [SerializeField] [HideInInspector] private float fallMultiplier = 5f;

    [Header("Collision")]
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private Vector3 headColliderOffset;
    [SerializeField] private float circleRadius = 0.15f;
    [SerializeField] [HideInInspector] private float circleRadiusFH = 0.15f, circleRadiusPCrawler = 0.15f, circleRadiusMX = 0.56f;
    public bool onGround = false;
    public bool onVoid = false;
    public bool headCollided;

    [Header("Character Animator")]
    [SerializeField] private PlayerAnimation AnimationScript;
    public bool isRunning;
    public bool isMoving;
    public bool isFalling;
    public bool isLanding; // for future MX land animation
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

        if (isCrouching)
        {

            // Changing Collider size based on Current Form (MX, FH, PCrawler)

            if (AnimationScript.isPCrawler || AnimationScript.isFH)
            {
                moveSpeed = 0;
                collider.offset = new Vector2(0.0118157f, -0.4482942f);
                collider.size = new Vector2(0.836907f, 1.210171f);

            }

        }
        else
        {
            if (AnimationScript.isPCrawler || AnimationScript.isFH)
            {
                collider.offset = new Vector2(0.004783392f, -0.229393f);
                collider.size = new Vector2(0.6538078f, 1.624514f);

            }

            if (AnimationScript.isMX)
            {
                collider.offset = new Vector2(-0.001805902f, 0.2736198f);
                collider.size = new Vector2(1.844248f, 2.63993f);

            }
        }

       if (!isMoving && isCrouching) // Can't move if crouching
                return;



            if (onGround)
            {
                if (Input.GetKey(KeyCode.X))
                {
                    StartCoroutine(SpeedUpMethod());
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
                maxSprintSpeed = 9;
            }

            else if (AnimationScript.isPCrawler)
            {
                maxSpeed = 5.25f;
                maxSprintSpeed = 6.5f;
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
            
            // Head Bump Detection (When mario hits something with his head)
            headCollided = Physics2D.OverlapCircle(transform.position - headColliderOffset, circleRadius - 0, groundLayer); // This is to indicate if mario's head bumped into something

            if (headCollided) // If it did, 
            {
                jumpTime = 0; // stop jumping. 
                Debug.Log("Head Collided.");
            }

           

            // The default and WahooJump used as 2 seperate methods. Y input is in Jump() and V input is in WahooJump() ONLY. 
            if (!AnimationScript.isTransforming || AnimationScript.isTransforming && AnimationScript.isPCrawler)
            {
                Jump();
                WahooJump();        
           }

            // Direction Input Detection (observable in the Inspector)
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


            // Change movement speed depending on Sprinting Boolean
            if (isSprinting)
            {
                if (speedUp)
                {
                    if (AnimationScript.isFH)
                    {
                        moveSpeed = sprintSpeed * 0.43f;
                        maxSpeed = maxSprintSpeed;
                    }
                    else
                    {
                        moveSpeed = sprintSpeed * 0.43f;
                        maxSpeed = maxSprintSpeedBIG;
                    } 

                } else
                {
                    
                        moveSpeed = sprintSpeed;
                }
            }
            else
            {
                speedUp = false;
                moveSpeed = defaultSpeed;
                maxSpeed = maxDefaultSpeed;
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
            if (!AnimationScript.isTransforming || AnimationScript.isTransforming && onVoid || AnimationScript.isTransforming && AnimationScript.isPCrawler)
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
            if (isRunning || isCrouching)
            {
                linearDrag = 1.0f;
            } else
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

        // change circleradius for grounddetection (FH, PCRAWLER, MX)
        if (AnimationScript.isFH)
        {
            circleRadius = circleRadiusFH;
        } else if (AnimationScript.isPCrawler)
        {
            circleRadius = circleRadiusPCrawler;
        } else if (AnimationScript.isMX)
        {
            circleRadius = circleRadiusMX;
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

    // I think these speak for themselves.
    void Jump()
    {
        // OLD JUMP METHOD //
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        //rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        //jumpTimer = 0;
        //StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));

            if (!onGround)
            {
                _isFalling = true;
            }

            if (onGround && Input.GetKeyDown(KeyCode.Y) && !onVoid)
            {
                    _isFalling = false;
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
                    _isFalling = false;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                    jumpTime -= Time.deltaTime;
                }

                else
                {
                    _isJumping = false;
                }

                if (isSprinting && isMoving)
                {
                    jumpSpeedFH = 8.35f;
                } else
                {
                jumpSpeedFH = 7.75f;
                }
            }

        // This prevents the bug that often makes mario skip a jump animation (DO NOT FUCKING TOUCH THIS TOOK ME HOURS TO FIX)
        if (Input.GetKeyUp(KeyCode.Y))
        {
            _isJumping = false;
            jumpTime = 0;
        }
        
        if (!onGround && !_isFalling)
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

        if (!onGround)
        {
            _isFalling = true;
        }

        if (onVoid && Input.GetKeyDown(KeyCode.V) && AnimationScript.isMX)
        {
            _isFalling = false;
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
                _isFalling = false;
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

        if (_isWahooJumping && rb.velocity.y <= 0)
        {
            isLanding = true;
        }

    }

    // Crucial for the player physics and movement. In summary, the method holds all the functions for managing gravity and the jump booleans
    void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround && rb.velocity.y <= 0.1f)
        {
            _isWahooJumping = false;
        }

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
           _isFalling = false;
            isLanding = false;
        }

        else
        {
            


            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;


            if (_isWahooJumping)
            {
                isWahooJumping = true;
            } else if (_isJumping)
            {
                isJumping = true;
            } else if (_isFalling)
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

    // 'Speeding up overtime' Function : After a specific time has passed while running, enable the speedUp boolean (which makes it possible to increase maxSprintSpeed in upper codelines.) 
    IEnumerator SpeedUpMethod()
    {
        if (AnimationScript.isFH)
        {

            if (isRunning) { 
            yield return new WaitForSeconds(1f);

            speedUp = true;
            }
        }

        if (AnimationScript.isPCrawler)
        {
            if (isRunning)
            {
                yield return new WaitForSeconds(1f);

                speedUp = true;
            }
        }

        if (AnimationScript.isMX)
        {
            if (isRunning)
            {
                yield return new WaitForSeconds(1f);

                speedUp = true;
            }
        }
    }

    // Flips the player sprite when changing Directions.
    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    // Makes the 'CharacterHolder' Object literally squeeze when jumping and landing.
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
