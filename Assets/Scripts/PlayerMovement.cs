using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float jumpDelay = 0.25f;
    private float jumpTimer;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;
    public CapsuleCollider2D collider;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float maxSprintSpeed = 12f;
    public float maxDefaultSpeed;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;

    [Header("Character Animator")]
    [SerializeField] private PlayerAnimation AnimationScript;
    public bool isRunning;
    public bool isJumping;
    public bool isSliding;
    public bool isCrouching => Input.GetKey(KeyCode.DownArrow);


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
        bool wasOnGround = onGround;
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        if (!wasOnGround && onGround)
        {
            StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.V) && AnimationScript.isMX)
        {
            jumpTimer = Time.time + jumpDelay;
        }
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
            moveSpeed = 0;
            collider.offset = new Vector2(0, -0.5f);
            collider.size = new Vector2(1f, 0f);
        }
        else
        {
            collider.offset = new Vector2(0, 0f);
            collider.size = new Vector2(1f, 2f);
        }


        if (direction.x != 0)
        {

            isRunning = true;
        }
        else
        {
            StartCoroutine("waitForRun", 0);
        }

       
        if (!AnimationScript.isTransforming)
        {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < -0.3f) || (direction.x < 0 && rb.velocity.x > 0.3f);

        if (changingDirections || changingDirections && isRunning == false)
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
            if (jumpTimer > Time.time && onGround)
            {
                if (AnimationScript.isMX)
                {
                    // WAHOO JUMP
                    if (Input.GetKey(KeyCode.V))
                    {
                        jumpSpeed = jumpSpeedMX * 1.7f;
                        Jump();
                        jumpSrc.PlayOneShot(wahooJump);
                    } else

                    jumpSpeed = jumpSpeedMX;
                    Jump();
                    jumpSrc.PlayOneShot(mxJump);
                } else if (AnimationScript.isPCrawler)
                {
                    jumpSpeed = jumpSpeedPC;
                    Jump();
                    jumpSrc.PlayOneShot(pcJump);
                } else if (AnimationScript.isFH)
                {
                    jumpSpeed = jumpSpeedFH;
                    Jump();
                    jumpSrc.PlayOneShot(fhJump);
                }

            }

            modifyPhysics();

        }
    }
    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
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
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
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
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

    IEnumerator waitForRun()
    {
            yield return new WaitForSeconds(0.2f);
            isRunning = false;

    }
}
