using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float maxVelocity;
    [SerializeField] [HideInInspector] private float sqrMaxVelocity;

    [Header("Raycasting")]

    [SerializeField] private float jumpForce;

    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    [SerializeField] LayerMask groundMask;

    [SerializeField] private float jumpTimeCounter;
    [SerializeField] private float jumpTime;
    [SerializeField] private bool isJumping;

    private void Awake()
    {
        SetMaxVelocity(maxVelocity);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
       

        // clamp speed

        Vector2 velocity = rb.velocity;

        velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if (velocity.sqrMagnitude > sqrMaxVelocity)
        {
            rb.velocity = velocity.normalized * maxVelocity;
        }

        // movement logic

        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);

        rb.AddForce(movement, ForceMode2D.Force);

    }

    void Jump()
    {
        Vector2 velocity = rb.velocity;
        
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundMask);

        if (isGrounded && Input.GetKeyDown(KeyCode.Z))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            velocity.y += 1 * jumpForce;
            rb.velocity += velocity;
        }

        if (Input.GetKey(KeyCode.Z) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                velocity.y += 1 * jumpForce;
                rb.velocity += velocity;
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            isJumping = false;
        }
    }

    void SetMaxVelocity(float _maxVelocity)
    {
        _maxVelocity = maxVelocity;
        sqrMaxVelocity = _maxVelocity * _maxVelocity;
    }
}
