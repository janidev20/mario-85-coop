using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeToDestroy;


    private void Start()
    {
        StartCoroutine(SelfDestroy());
    }


    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(timeToDestroy);

         Destroy(this.gameObject);
        
    }
}
