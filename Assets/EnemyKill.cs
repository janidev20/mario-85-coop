using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{

    [Header("Collision")]
    [SerializeField] private bool enableWahooSound; // This is for Pipe Crawler, when he kills a goomba while jumping. (SET THIS FOR TOP COLLIDER)
    
    public bool collidingEnemyHead;
    [SerializeField] private Vector3 feetColliderOffset;
    [SerializeField] private float feetRadius;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Components")]
    [SerializeField] PlayerAnimation plyAnim;

    private void Awake()
    {
        plyAnim = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        EnemySquash();
    }

    void EnemySquash()
    {
        // Get the object's collider that the overlapbox is colliding with.
        Collider2D hit = Physics2D.OverlapCircle(transform.position + feetColliderOffset, feetRadius, enemyLayer);
        collidingEnemyHead = Physics2D.OverlapCircle(transform.position + feetColliderOffset, feetRadius, enemyLayer) && !hit.gameObject.GetComponent<EnemyBehaviour>().isDead;

        if (collidingEnemyHead && GetComponent<PlayerAnimation>().isFH)
        {
            hit.gameObject.GetComponent<EnemyBehaviour>().squash = true;
            hit.gameObject.GetComponent<EnemyBehaviour>().isDead = true;
        }

        if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy") && hit.gameObject.GetComponent<EnemyBehaviour>().isDead)
        {
            Physics2D.IgnoreCollision(hit, GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !collision.gameObject.GetComponent<EnemyBehaviour>().isDead && !plyAnim.isFH)
        {
            collision.gameObject.GetComponent<EnemyBehaviour>().isDead = true;

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && collision.gameObject.GetComponent<EnemyBehaviour>().isDead)
        {
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && !collision.collider.gameObject.GetComponent<EnemyBehaviour>().isDead && !plyAnim.isFH)
        {
            collision.collider.gameObject.GetComponent<EnemyBehaviour>().isDead = true;

        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && collision.collider.gameObject.GetComponent<EnemyBehaviour>().isDead)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + feetColliderOffset, feetRadius);
    }

}
