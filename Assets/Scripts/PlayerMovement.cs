using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    Rigidbody2D rb;

    [Header("Player stats")]

    [SerializeField] private float _maxSpeed = 20;
    [SerializeField] private float _minSpeed;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _currentSpeed; // For changing speed
    [SerializeField] private float _SprintMultiplier = 0.015f; // For changing speed

    [SerializeField] private float _slideAtSpeed; // The specfic moving speed to start sliding method at
    [SerializeField] private float _slideTimer;
    [SerializeField] private bool _slideTimerEnabled;
    [SerializeField] private float _slideAmount;


    [SerializeField] private bool isSprinting;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _minSpeed = _moveSpeed;
    }


    private void FixedUpdate()
    {
        Movement();
        Flip();
    }

    void Movement()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0);



        rb.velocity = movement.normalized * Time.fixedDeltaTime * _moveSpeed;

        //Sprinting
        isSprinting = false ? true : Input.GetKey(KeyCode.X);
    }

    void Flip()
    {
        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            spriteRenderer.flipX = true;
        } else if (inputX < 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}