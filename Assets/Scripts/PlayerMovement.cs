using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Cutscene")]

    [Header("Horizontal Movement")]
    [SerializeField] private float slideSpeed;
    [SerializeField] [HideInInspector] private float moveSpeed = 30f; // The acceleration speed.
    [SerializeField] [HideInInspector] private float defaultSpeed; // used for moveSpeed or smth idk.
    [SerializeField] [HideInInspector] private bool speedUp = false;
    [SerializeField] [HideInInspector] private bool facingRight = true;
    public Vector2 direction;
    public bool isSprinting;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpSpeedMX = 14.5f, jumpSpeedPC = 14.5f, jumpSpeedFH = 10f; //the jump force based on what character the player is
    [SerializeField] private bool _enableJumping = true; // this is used for cooldown coroutine when player breaks block. (with jump/headbump) 
    [SerializeField] private bool _isJumping = false;
    [SerializeField] private float jumpStartTime;
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float fallSpeed;
    [SerializeField] [HideInInspector] private bool _isWahooJumping;
    public bool _isFalling;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private List<LayerMask> blockLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask voidLayer;
    [SerializeField] private GameObject characterHolder;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private BoxCollider2D fallDetect;


    [Header("Physics")]
    /// Max speed values
    [SerializeField] private float maxSpeedFH = 4f, maxSpeedBigger = 5.25f;
    [SerializeField] private float maxSprintSpeedFH = 6.56f, maxSprintSpeedBigger = 7.0f;
    [SerializeField] [HideInInspector] private float maxSpeed;              // (this will be changed according to what we are (FH, Pcrawelr , MX)
    [SerializeField] [HideInInspector] private float maxSprintSpeed = 12f;  // (this will be changed according to what we are (FH, Pcrawelr , MX)
    [SerializeField] [HideInInspector] private float maxDefaultSpeed;       // (this will be changed according to what we are (FH, Pcrawelr , MX)
    /// </Max speed values>
  
    [SerializeField] [HideInInspector] private float linearDrag = 4f;
    [SerializeField] [HideInInspector] private float gravity = 1f;
    [SerializeField] [HideInInspector] private float fallMultiplier = 5f;

    [Header("Collision")]
    [SerializeField] private Vector3 colliderOffset, colliderOffsetMX;
    [SerializeField] private Vector3 headColliderBoxOffset;
    [SerializeField] private Vector3 headColliderBoxSize;
    [SerializeField] private float headColliderCircleRadius = 0.15f;
    [SerializeField] private float circleRadiusFH = 0.15f, circleRadiusPCrawler = 0.15f, circleRadiusMX = 0.8f;
    [SerializeField] [HideInInspector] private float circleRadius = 0.15f; // circleRadius will be changed according to what we are (FH, Pcrawelr , MX)
    public bool onGround = false;
    public bool onVoid = false;
    public bool headCollided;

    [Header("Character Animator")]
    [SerializeField] private PlayerAnimation AnimationScript;
    public bool isRunning;
    public bool isMoving; 
    public bool isFalling;
    public bool isLanding;
    public bool isJumping;
    public bool isWahooJumping;
    public bool isSliding;
    public bool isCrouching => UserInput.instance.Crouch && onGround && Input.GetAxis("Horizontal") <= 0.9f && Input.GetAxis("Horizontal") >= -0.9f && !_isJumping; // Is Crouchig Boolean depending on input and smoothing input. The horizontal input thingys here are crucial to smooth movement.


    [Header("Audio")]
    [SerializeField] private VoiceManager VM;
    [SerializeField] private AudioSource jumpSrc;
    [SerializeField] private AudioClip bumpSound;
    [SerializeField] private AudioClip mxJump, pcJump, fhJump;

    [Header("Enemy Script")]
    [SerializeField] private EnemyKill ek;

    [Header("Intro")]
    public bool cantMove = false;

    private void Start()
    {
        Flip();
        
        rb = GetComponent<Rigidbody2D>();
        ek = GetComponent<EnemyKill>();
        defaultSpeed = moveSpeed;
        maxDefaultSpeed = maxSpeed;
    }


     void Update()
    {

        ChangeHeight();
        ChangeSpeed();
        CheckGrounds();
        HeadCollision();
        SlidingManage();
        CircleRadiusManagement();

        if (VoidFallCounter.fellInVoid || VoidFallCounter.LucasFellInVoid || LucasController.LucasIsDead || MXCutsceneManager.pause)
        {
            jumpSrc.mute = true;
            direction.x = 0;
            rb.velocity = new Vector2(0, 0);
            rb.isKinematic = true;
        } else if (!MXCutsceneManager.pause)
        {

            jumpSrc.mute = false;
            rb.isKinematic = false;
        }

        if (isCrouching && !isMoving && !AnimationScript.isPCrawler) // Can't move if crouching + if player isnt pcrawler (cause he doesnt have crouch anim lol)
            return;

        if (StunManager.isStunned || GameManager.cutScenePlaying || !   GameManager.gameStarted || LucasEscape.Escaped || VoidFallCounter.fellInVoid || VoidFallCounter.LucasFellInVoid)
            return;

        VoiceHandler();

        // The default and WahooJump used as 2 seperate methods. Y input is in Jump() and V input is in WahooJump() ONLY. 
        if (!AnimationScript.isTransforming)
        {
            Jump();
            WahooJump();
        }

        // Direction Input Detection (observable in the Inspector)
        
            direction = new Vector2(direction.x, Input.GetAxisRaw("Vertical"));
   
        if (UserInput.instance.MoveLeft)
        {
            direction.x = -1;
        } else if (UserInput.instance.MoveRight)
        {
            direction.x = 1;
        } else
        {
            direction.x = 0;
        }
        

    }
     void FixedUpdate()
     {
         modifyPhysics();


        if (StunManager.isStunned && GameManager.gameStarted)
        {
            direction.x = 0;
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x - 0.3f, rb.velocity.y);
            }
            return;
        }

        moveCharacter(direction.x);
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
        
            EnemyBump();

            if (headCollided && !onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(-Vector2.up * fallSpeed, ForceMode2D.Impulse);
                jumpTime = 0;
            }
       
            if (!onGround && !_isJumping)
            {
                _isFalling = true;
            }

            if (onGround && UserInput.instance.JumpJustPressed && !onVoid)
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


            if (UserInput.instance.JumpBeingHeld && _isJumping == true)
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
            if (UserInput.instance.JumpReleased)
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

            if (onVoid && UserInput.instance.SuperJump && AnimationScript.isMX)
            {
                _isFalling = false;
                _isJumping = true;
                _isWahooJumping = true;
                /////////////////////////////////
                jumpSpeed = jumpSpeedMX;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * (jumpSpeed * 2.95f), ForceMode2D.Impulse);
                VM.WahooJump();
                jumpSrc.PlayOneShot(mxJump);

                /////////////////////////////////
                StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
            }


            if (_isWahooJumping && rb.velocity.y <= -2)
            {
                isLanding = true;
            }

    }

    // add extra force upwards when stomping on enemy's head
     void EnemyBump()
     {
        if (ek.collidingEnemyHead)
        {
            if (UserInput.instance.JumpBeingHeld)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * (jumpSpeed * 0.6f), ForceMode2D.Impulse);
                jumpTime = 0;
            }

            else
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * fallSpeed, ForceMode2D.Impulse);
            jumpTime = 0;
        }
     } 

    IEnumerator forceWahooJumpSpeed()
    {
        jumpSpeed = jumpSpeedMX;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * (jumpSpeed * 2.95f), ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);
    }

     void ChangeHeight()
        {
             if (isCrouching && !AnimationScript.isPCrawler)
             {
                 maxSpeed = 0f;
                 // Changing Collider size based on Current Form (MX, FH, PCrawler)

                 if (AnimationScript.isFH)
                 {
                     defaultSpeed = 0;
                     collider.size = new Vector2(0.836907f, 1.210171f);

                 }

             }
            else if (!isCrouching)
            {
                if (AnimationScript.isPCrawler)
                {
                    collider.offset = new Vector2(-0.007822976f, -0.229393f);
                    collider.size = new Vector2(0.6913055f, 1.620345f);
                }

            if (AnimationScript.isFH)
                 {
                     collider.offset = new Vector2(-0.007822976f, -0.229393f);
                     collider.size = new Vector2(0.6913055f, 1.620345f);

                 }

                 if (AnimationScript.isMX)
                 {
                     // collider.offset = new Vector2(0.004046202f, 0.747394f);
                     // collider.size = new Vector2(2.041561f, 3.244714f);

                     collider.offset = new Vector2(0.004046202f, 0.6428025f);
                     collider.size = new Vector2(2.041561f, 3.453897f);
                 }
            }
        }
     void ChangeSpeed()
        {
            bool isPressingButton = false;
            bool canPress = true;

            if (onGround)
            {
                if (Application.isMobilePlatform)
                {
                if (UserInput.instance.RunPhone)
                {
                    isSprinting = !isSprinting;
                }
                }
              
                else if (!Application.isMobilePlatform)
                {
                    if (UserInput.instance.Run)
                    {
                        isSprinting = true;
                    }
                else
                {
                    isSprinting = false;
                }

            }
        }

            if (onGround)
            {
                if (AnimationScript.isPCrawler || AnimationScript.isMX)
                {
                    maxDefaultSpeed = maxSpeedBigger;
                    maxSprintSpeed = maxSprintSpeedBigger;
                }
                if (AnimationScript.isFH)
                {
                    maxDefaultSpeed = maxSpeedFH;
                    maxSprintSpeed = maxSprintSpeedFH;

                }

            }   else if (onVoid)
        {
            maxDefaultSpeed = 5.85f;
            maxSprintSpeed = 6.20f;
        }

        //Run Animation boolean
        isRunning = isSprinting;

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

            }
        }
     void CheckGrounds()
        {
            // Ground Detection, circleRadius value is changed already based on character in a different codelist below
            bool wasOnGround = onGround;
            onVoid = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, voidLayer);
            if (AnimationScript.isMX)
            {
                onGround = Physics2D.OverlapCircle(transform.position + colliderOffsetMX, circleRadius, groundLayer);
            }
            else
                onGround = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, groundLayer);

            // jump squeeze couroutine method (no idea what this is but don't touch it maybe)
            if (!wasOnGround && onGround)
            {
                StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
            }
        }
     void HeadCollision()
        {
        // Head Bump Detection (When mario hits something with his head)
        // headCollided = Physics2D.OverlapCircle(transform.position - headColliderOffset, headColliderCircleRadius, groundLayer) || Physics2D.OverlapCircle(transform.position - headColliderOffsetB, headColliderCircleRadius, groundLayer); // This is to indicate if mario's head bumped into something

        headCollided = Physics2D.OverlapBox(transform.position - headColliderBoxOffset, headColliderBoxSize, 0, blockLayer[0]);

            if (headCollided) // If it did, 
            {
                jumpSrc.PlayOneShot(bumpSound);
                Debug.Log("Head Collided.");
            }
        }
     void SlidingManage()
        {
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
                    linearDrag = 2f;
                }
                else
                    linearDrag = 1.6f;
            }
            else
            {
                linearDrag = 5;
            }
        }
     void CircleRadiusManagement()
        {
            // change circleradius for grounddetection (FH, PCRAWLER, MX)
            if (AnimationScript.isFH)
            {
                circleRadius = circleRadiusFH;
            }
            else if (AnimationScript.isPCrawler)
            {
                circleRadius = circleRadiusPCrawler;
            }
            else if (AnimationScript.isMX)
            {
                circleRadius = circleRadiusMX;
            }
        }
     void VoiceHandler()
        {
            if (UserInput.instance.Talk)
            {
                if (AnimationScript.isFH)
                {
                    VM.RandomFH();
                }
                else if (AnimationScript.isPCrawler)
                    {
                        VM.RandomPC();
                    }
                else
                {
                    VM.RandomMX();
                }
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

            if (isFalling && !isJumping && AnimationScript.isPCrawler)
            {
                VM.Fall();
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("cutsceneTrigger"))
        {
            MXCutsceneManager.startCutscene = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("voidKiller"))
        {
            VoidFallCounter.fellInVoid = true;
        } else
        {
            VoidFallCounter.fellInVoid = false;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Stop")) {
            cantMove = true;
        } else
        {
            cantMove = false;
        }
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
    
    // Draw the OverLap Sphere/Box detections on Unity Preview
    private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + colliderOffset, circleRadius);
           // Gizmos.DrawSphere(transform.position - headColliderOffset, headColliderCircleRadius);
            //Gizmos.DrawSphere(transform.position - headColliderOffsetB, headColliderCircleRadius);
            Gizmos.DrawWireCube(transform.position - headColliderBoxOffset, headColliderBoxSize);
        }
}
