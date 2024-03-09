using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    Rigidbody2D rb;

    [Header("Player Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float walkSpeed;

    [SerializeField] private float _maxVelocity = 8;

    [SerializeField] private float _maxSprintVelocity = 16;

    [SerializeField] private float _sqrMaxVelocity;

    [SerializeField] private bool _isSprinting;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        SetMaxVelocity(maxVelocity: _maxVelocity);
        Movement();

    }

    void SetMaxVelocity(float maxVelocity)
    {
        maxVelocity = _maxVelocity;
        _sqrMaxVelocity = maxVelocity * maxVelocity;
    }

    void Movement()
    {
        // Clamping the rigidbody velocity
        Vector2 velocity = rb.velocity;

        if (velocity.sqrMagnitude > _sqrMaxVelocity)
        {
            rb.velocity = velocity.normalized * _maxVelocity;
        }

        // Movement Mechanic
        float inputX = Input.GetAxisRaw("Horizontal");

        Vector2 movementRB = new Vector2(inputX * _moveSpeed * Time.deltaTime, transform.position.y);

        rb.AddForce(movementRB, ForceMode2D.Force);

        // Sprinting

        _isSprinting = false ? true : Input.GetKey(KeyCode.Y);

        if (_isSprinting)
        {
            _maxVelocity = 16;
            _moveSpeed = sprintSpeed;
        }
        else
        {
            _maxVelocity = 8;
            _moveSpeed = walkSpeed;
        }
    }


}