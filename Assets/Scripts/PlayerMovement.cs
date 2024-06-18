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
    [SerializeField] private float sprintSpeed = 20f;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] [HideInInspector] private float moveSpeed = 10f;
    [SerializeField] [HideInInspector] private bool speedUp = false;
    [SerializeField] [HideInInspector] private bool facingRight = true;
    public bool isSprinting;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpSpeedMX = 14.5f, jumpSpeedPC = 14.5f, jumpSpeedFH = 10f; //the jump force based on what character the player is
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
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] [HideInInspector] private float maxSprintSpeed = 12f;
    [SerializeField] [HideInInspector] private float maxDefaultSpeed;
    [SerializeField] [HideInInspector] private float linearDrag = 4f;
    [SerializeField] [HideInInspector] private float gravity = 1f;
    [SerializeField] [HideInInspector] private float fallMultiplier = 5f;

    [Header("Collision")]
    [SerializeField] private Vector3 colliderOffset, colliderOffsetMX;
    [SerializeField] private Vector3 headColliderOffset;
    [SerializeField] private float circleRadius = 0.15f;
    [SerializeField] [HideInInspector] private float circleRadiusFH = 0.15f, circleRadiusPCrawler = 0.15f, circleRadiusMX = 0.8f;
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
    public bool isCrouching => Input.GetKey(KeyCode.DownArrow) && onGround && Input.GetAxis("Horizontal") <= 0.9f && Input.GetAxis("Horizontal") >= -0.9f && !_isJumping; // Is Crouchig Boolean depending on input and smoothing input. The horizontal input thingys here are crucial to smooth movement.


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


        if (isCrouching && !isMoving && !AnimationScript.isPCrawler) // Can't move if crouching + if player isnt pcrawler (cause he doesnt have crouch anim lol)
            return;


        if (isCrouching && !AnimationScript.isPCrawler)
        {
            maxSpeed = 0f;
            // Changing Collider size based on Current Form (MX, FH, PCrawler)

            if (AnimationScript.isFH)
            {
                defaultSpeed = 0;
                collider.offset = new Vector2(0.0118157f, -0.4482942f);
                collider.size = new Vector2(0.836907f, 1.210171f);

            }

        }
        else if (!isCrouching)
        {

            if (AnimationScript.isFH)
            {
                collider.offset = new Vector2(0.004783392f, -0.229393f);
                collider.size = new Vector2(0.6538078f, 1.624514f);

            }

            if (AnimationScript.isMX)
            {
                // collider.offset = new Vector2(0.004046202f, 0.747394f);
                // collider.size = new Vector2(2.041561f, 3.244714f);

                collider.offset = new Vector2(0.004046202f, 0.6428025f);
                collider.size = new Vector2(2.041561f, 3.453897f);
            }
        }



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

        if (AnimationScript.isPCrawler || AnimationScript.isMX)
        {
            maxDefaultSpeed = 5.25f;
            maxSprintSpeed = 7.55f;
        }
        if (AnimationScript.isFH)
        {
            maxDefaultSpeed = 4;
            maxSprintSpeed = 6.56f;

        }

        //Run Animation boolean
        isRunning = isSprinting;

        // Ground Detection, circleRadius value is changed already based on character in a different codelist below
        bool wasOnGround = onGround;
        onVoid = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, voidLayer);
        if (AnimationScript.isMX)
        {
            onGround = Physics2D.OverlapCircle(transform.position + colliderOffsetMX, circleRadius, groundLayer);
        } else
            onGround = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, groundLayer);

        // Head Bump Detection (When mario hits something with his head)
        headCollided = Physics2D.OverlapCircle(transform.position - headColliderOffset, circleRadius - 0, groundLayer); // This is to indicate if mario's head bumped into something

        if (headCollided) // If it did, 
        {
            jumpTime = 0; // stop jumping. 
            Debug.Log("Head Collided.");
        }



        // The default and WahooJump used as 2 seperate methods. Y input is in Jump() and V input is in WahooJump() ONLY. 
        if (!AnimationScript.isTransforming)
        {
            Jump();
            WahooJump();
        }

        // Direction Input Detection (observable in the Inspector)
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


        // Smoothly change the maxSpeed based on isSprinting boolean
        if (isSprinting)
        {
            maxSpeed = maxSprintSpeed;
        }
        else if (!isSprinting)
        {
            if (isSliding)
            {
                maxSpeed = slideSpeed;
            }

            else if (isMoving)
            {
                if (maxSpeed > maxDefaultSpeed)
                {
                    maxSpeed -= 0.075f;
                }
            }

            else if (!isMoving)
            {
                maxSpeed = maxDefaultSpeed;
            }

            // Modify moveSpeed based on sliding boolean
           
            
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
                if (isRunning || isCrouching)
                {
                    linearDrag = 0.75f;
                } else
                    linearDrag = 1.6f;
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
                jumpSpeed = jumpSpeedMX;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * (jumpSpeed * 2.8f), ForceMode2D.Impulse);
                jumpSrc.PlayOneShot(wahooJump);
                jumpSrc.PlayOneShot(mxJump);
                /////////////////////////////////
                StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
            }


            if (_isWahooJumping && rb.velocity.y <= -2)
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
                isJumping = false;
                isWahooJumping = false;
                isFalling = false;
                _isFalling = false;
                isLanding = false;

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
