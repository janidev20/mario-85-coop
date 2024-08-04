using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucasController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float linearDrag;
    [SerializeField] private Vector2 direction;
    public bool isRunning;
    public bool isMoving;
    public bool isSliding;
    public bool isJumping;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpSpeed = 8.35f; //the jump force based on what character the player is
    [SerializeField] private bool _enableJumping = true; // this is used for cooldown coroutine when player breaks block. (with jump/headbump)
    [SerializeField] private float jumpStartTime;
    [SerializeField] private float jumpTime;
    [SerializeField] private float fallSpeed;

    [Header("Physics")]

    [SerializeField] private float maxSpeed;
    [SerializeField] private float _maxRunSpeed = 6.5f;
    [SerializeField] private float _maxWalkSpeed = 4f;
    [SerializeField] [HideInInspector] private float gravity = 1f;
    [SerializeField] [HideInInspector] private float fallMultiplier = 5f;

    [Header("Collision")]
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private Vector3 headColliderBoxOffset;
    [SerializeField] private Vector3 headColliderBoxSize;
    [SerializeField] private float circleRadius = 0.15f; // circleRadius will be changed according to what we are (FH, Pcrawelr , MX)
    public bool onGround = false;
    public bool headCollided;

    [Header("Components")]

    [SerializeField] private List<LayerMask> blockLayer;
    [SerializeField] private LayerMask groundLayer;

    [Header("Sprite")]
    [SerializeField] private bool facingRight = true;

    [Header("Intro Movement")]
    [SerializeField] public bool runLeft = false;

    [Header("Audio")]
    [SerializeField] private AudioSource audSRC;
    [SerializeField] private AudioClip jumpSound, bumpSound;


    [Header("Enemy Script")]
    [SerializeField] private EnemyKill ek;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ek = GetComponent<EnemyKill>();
    }

    private void Update()
    {

        Jump();
        HeadCollision();
        SlidingManage();
        EnemyBump();
        CheckGrounds();
        ChangeMaxSpeed();
        CheckForInput();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        moveCharacter(direction.x);
        modifyPhysics();


        if (runLeft)
        {
            direction.x = -1;
        }

       
    }   

    void CheckForInput()
    {

        // Horizontal Input
        direction.x = Input.GetAxisRaw("Horizontal");

        // Running
        if (Input.GetKey(KeyCode.JoystickButton5))
        {
            isRunning = true;
        } else
        {
            isRunning = false;
        }
    }

    void ChangeMaxSpeed()
    {
        maxSpeed = isRunning ? _maxRunSpeed : _maxWalkSpeed;
    }

    void CheckGrounds()
    {
        // Ground Detection, circleRadius value is changed already based on character in a different codelist below
        onGround = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, groundLayer);
        bool wasOnGround = onGround;
        wasOnGround = Physics2D.OverlapCircle(transform.position + colliderOffset, circleRadius, groundLayer);

    }

    void Jump()
        {
            if (headCollided && !onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(-Vector2.up * fallSpeed, ForceMode2D.Impulse);
                jumpTime = 0;
            }


        if (onGround && Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                isJumping = true;
                jumpTime = jumpStartTime;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

                audSRC.PlayOneShot(jumpSound);
            }


            if (Input.GetKey(KeyCode.JoystickButton1) && isJumping == true)
            {

                if (jumpTime > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                    jumpTime -= Time.deltaTime;


                }

                else
                {
                    isJumping = false;
                }

            }

            // This prevents the bug that often makes mario skip a jump animation (DO NOT FUCKING TOUCH THIS TOOK ME HOURS TO FIX)
            if (Input.GetKeyUp(KeyCode.JoystickButton1))
            {
                isJumping = false;
                jumpTime = 0;
            }

            if (!onGround)
            {
                isJumping = true;
            }
        }

    void HeadCollision()
    {
        // Head Bump Detection (When mario hits something with his head)
        // headCollided = Physics2D.OverlapCircle(transform.position - headColliderOffset, headColliderCircleRadius, groundLayer) || Physics2D.OverlapCircle(transform.position - headColliderOffsetB, headColliderCircleRadius, groundLayer); // This is to indicate if mario's head bumped into something

        headCollided = Physics2D.OverlapBox(transform.position - headColliderBoxOffset, headColliderBoxSize, 0, blockLayer[0]);

        if (headCollided) // If it did, 
        {
            Debug.Log("LUCAS Head Collided.");
            audSRC.PlayOneShot(bumpSound);
            
        }
    }

    void EnemyBump()
    {
        if (ek.collidingEnemyHead)
        {
            if (Input.GetKey(KeyCode.JoystickButton1))
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

    public void moveCharacter(float horizontal)
    {

        rb.AddForce(direction * moveSpeed * Time.deltaTime);
        
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
    }

    void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);


        if (onGround)
        {
            isJumping = false;

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




            isJumping = true;


            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.JoystickButton1))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
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


        // If sliding, increase linear drag
        if (isSliding)
        {
            if (isRunning)
            {
                linearDrag = 2f;

            }
            else
                linearDrag = 1.6f;
        }
        else
        {
            linearDrag = 5f;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + colliderOffset, circleRadius);
        Gizmos.DrawWireCube(transform.position - headColliderBoxOffset, headColliderBoxSize);
    }
}
