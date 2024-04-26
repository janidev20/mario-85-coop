using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreaking : MonoBehaviour
{
    // Instantiate Break Effect (when MX collides with a breakable object)
    [SerializeField] private GameObject BlockBreakEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the object of the collision has a 'Breakable' tag
        if (collision.gameObject.CompareTag("Breakable"))
        {
            Instantiate(BlockBreakEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
