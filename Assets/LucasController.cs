using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucasController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontal;

    [Header("Sprite")]
    [SerializeField] private bool facingRight = true;

    [Header("AI")]
    [SerializeField] private bool didntMove = false;
    [SerializeField] public bool canMove = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            animator.SetBool("run", true);
            didntMove = true;
            MoveLeft();
        }   
    }   

    public void MoveLeft()
    {
        if (!didntMove) {
            horizontal = 0;
                } else
        {
            horizontal =- 1;
        }

        rb.AddForce(Vector2.right * horizontal * moveSpeed);
        
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }


}
