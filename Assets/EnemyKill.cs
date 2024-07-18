using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{

    [Header("Collision")]
    [SerializeField] private Vector3 feetColliderOffset;
    [SerializeField] private float feetRadius;
    [SerializeField] private LayerMask enemyLayer;
    public bool collidingEnemyHead;

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

    // Player (False Hero) stomp on head method
    void EnemySquash()
    {
        // Get the object's collider that the overlapbox is colliding with.
        Collider2D hit = Physics2D.OverlapCircle(transform.position + feetColliderOffset, feetRadius, enemyLayer);
        // Boolean for if colliding with the enemy head
        collidingEnemyHead = Physics2D.OverlapCircle(transform.position + feetColliderOffset, feetRadius, enemyLayer) && !hit.gameObject.GetComponent<EnemyBehaviour>().isDead;

        // if "colliding with the enemy head" then :
        if (collidingEnemyHead && GetComponent<PlayerAnimation>().isFH)
        {
            hit.gameObject.GetComponent<EnemyBehaviour>().squash = true;
            hit.gameObject.GetComponent<EnemyBehaviour>().isDead = true;
        }

        // IGNORE COLLISION if the enemy is dead, 
        if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy") && hit.gameObject.GetComponent<EnemyBehaviour>().isDead)
        {
            Physics2D.IgnoreCollision(hit, GetComponent<Collider2D>());
        }
    }

    // Kill method for Pipe Crawler and MX
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !collision.gameObject.GetComponent<EnemyBehaviour>().isDead && !plyAnim.isFH)
        {
            collision.gameObject.GetComponent<EnemyBehaviour>().isDead = true;

        }
        
        // ignore collision if enemy is dead 
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

        // ignore collision if enemy is dead
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && collision.collider.gameObject.GetComponent<EnemyBehaviour>().isDead)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());

        }
    }

    // Draw the Sphere detections on Unity Preview
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + feetColliderOffset, feetRadius);
    }

}
