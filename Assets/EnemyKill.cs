using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{
    [Header("Lucas")]
    public bool isLucas;

    [Header("Collision")]
    [SerializeField] private Vector3 feetColliderOffset;
    [SerializeField] private float feetRadius;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask MXHeadLayer;
    public bool collidingEnemyHead;
    public bool collidingMXHead;

    [Header("Components")]
    [SerializeField] PlayerAnimation plyAnim;

    private void Awake()
    {
      
        if (!isLucas)
        {
            plyAnim = GetComponent<PlayerAnimation>();
        }
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
        // Get the StunManager from MX 
        Collider2D MXController = Physics2D.OverlapCircle(transform.position + feetColliderOffset, feetRadius, MXHeadLayer);
        // Boolean for if colliding with the enemy head
        collidingEnemyHead = Physics2D.OverlapCircle(transform.position + feetColliderOffset, feetRadius, enemyLayer) && !hit.gameObject.GetComponent<EnemyBehaviour>().isDead;
        collidingMXHead = Physics2D.OverlapCircle(transform.position + feetColliderOffset, feetRadius, MXHeadLayer) && !StunManager.isStunned;

        // if "colliding with mx's head" then : 
        if (collidingMXHead && isLucas)
        {
            MXController.GetComponentInChildren<StunManager>().Stun();
        }


        // if "colliding with the enemy head" then :
        if (collidingEnemyHead && isLucas)
        {
            if (hit.gameObject.GetComponent<EnemyBehaviour>().isGoomba)
            {
                hit.gameObject.GetComponent<EnemyBehaviour>().squash = true;
                hit.gameObject.GetComponent<EnemyBehaviour>().isDead = true;
            }

            if (hit.gameObject.GetComponent<EnemyBehaviour>().isKoopa)
            {
                if (!hit.gameObject.GetComponent<EnemyBehaviour>().isShell)
                {
                    hit.gameObject.GetComponent<EnemyBehaviour>().isShell = true;
                }


                hit.gameObject.GetComponent<EnemyBehaviour>().isRolling = !hit.gameObject.GetComponent<EnemyBehaviour>().isRolling;

            }
        }

        else if (collidingEnemyHead && GetComponent<PlayerAnimation>().isFH)
        {
            if (hit.gameObject.GetComponent<EnemyBehaviour>().isGoomba)
            {
                hit.gameObject.GetComponent<EnemyBehaviour>().squash = true;
                hit.gameObject.GetComponent<EnemyBehaviour>().isDead = true;
            }

            if (hit.gameObject.GetComponent<EnemyBehaviour>().isKoopa)
            {
               if (!hit.gameObject.GetComponent<EnemyBehaviour>().isShell)
               {
                    hit.gameObject.GetComponent<EnemyBehaviour>().isShell = true;
               }


               hit.gameObject.GetComponent<EnemyBehaviour>().isRolling =!hit.gameObject.GetComponent<EnemyBehaviour>().isRolling;
               
            }
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

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !collision.gameObject.GetComponent<EnemyBehaviour>().isDead && !plyAnim.isFH && !isLucas)
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

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && !collision.collider.gameObject.GetComponent<EnemyBehaviour>().isDead && !plyAnim.isFH && !isLucas)
        {
            collision.collider.gameObject.GetComponent<EnemyBehaviour>().isDead = true;

        }

        // ignore collision if enemy is dead
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && collision.collider.gameObject.GetComponent<EnemyBehaviour>().isDead)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && !collision.collider.gameObject.GetComponent<EnemyBehaviour>().isDead && !plyAnim.isFH && !isLucas)
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
