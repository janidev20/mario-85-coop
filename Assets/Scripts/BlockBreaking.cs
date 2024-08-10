using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreaking : MonoBehaviour
{
    [Header("Break Effect")]
    // Instantiate Break Effect (when MX collides with a breakable object)
    [SerializeField] private GameObject BlockBreakEffect;
    [SerializeField] private List<LayerMask> breakableObjects;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the object of the collision has a specific Layer:
        if (collision.gameObject.layer == LayerMask.NameToLayer("BrickBlock"))
        {
            Instantiate(BlockBreakEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            BlocksCounter.BrickBlock += 1;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("StoneBlock"))
        {
            Instantiate(BlockBreakEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            BlocksCounter.StoneBlock += 1;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("EmptyBlock"))
        {
            Instantiate(BlockBreakEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            BlocksCounter.EmptyBlock += 1;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Pipe"))
        {
            Instantiate(BlockBreakEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            BlocksCounter.Pipe += 1;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("QMblock"))
        {
            Instantiate(BlockBreakEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            BlocksCounter.QMBlock += 1;
        }
    }
}
