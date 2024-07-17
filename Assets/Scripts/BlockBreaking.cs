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
        if (collision.gameObject.layer == LayerMask.NameToLayer("BrickBlock") || 
            collision.gameObject.layer == LayerMask.NameToLayer("QMblock") || 
            collision.gameObject.layer == LayerMask.NameToLayer("StoneBlock") || 
            collision.gameObject.layer == LayerMask.NameToLayer("EmptyBlock") || 
            collision.gameObject.layer == LayerMask.NameToLayer("Pipe"))
        {
            Instantiate(BlockBreakEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }

       
    }
}
