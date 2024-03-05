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

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveSlide;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal"); // Reference of the left and right input 

        Vector2 movement = new Vector2(inputX * _moveSpeed, 0);

        movement *= Time.deltaTime;

        transform.Translate(movement);

    }

}
 