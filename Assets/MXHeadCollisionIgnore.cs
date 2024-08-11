using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MXHeadCollisionIgnore : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("whatIsGround") || collision.collider.gameObject.layer == LayerMask.NameToLayer("DStoneBlock"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("whatIsGround") || collision.collider.gameObject.layer == LayerMask.NameToLayer("DStoneBlock"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("whatIsGround") || collision.collider.gameObject.layer == LayerMask.NameToLayer("DStoneBlock"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
