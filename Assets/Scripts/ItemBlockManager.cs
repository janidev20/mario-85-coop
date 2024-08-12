using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlockManager : MonoBehaviour
{
    [Header("BrickBlockCheck")]
    public bool isBrickBlock;

    [Header("Items")]
    [SerializeField] private bool isCoin = true;
    
    [SerializeField] private GameObject coin;

    [Header("Interaction")]
    public bool hit;
    public bool hitDestroy;

    [Header("Audio")]
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip coinSound;
    bool didPlay;

    [Header("Animation")]
    [SerializeField] Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
    }

    private void Update()
    {
        BrickBlockBump();
        GiveItem(null);
    }

    void GiveItem(GameObject item)
    {
        if (!hit || isBrickBlock)
            return;


        if (isCoin)
        {
            item = coin;
        }

        
        if (!didPlay)
        {
            animator.SetTrigger("hit");
            Instantiate(item, transform.position, Quaternion.identity, transform);
            src.PlayOneShot(coinSound);
            didPlay = true;
        }
    }
    
    public void BrickBlockBump()
    {
        if (!isBrickBlock)
            return;

        if (hit)
        {
            animator.SetBool("hit", true);
            StartCoroutine(BrickBlockIdleReset());
        }
    }

    IEnumerator BrickBlockIdleReset()
    {
        yield return new WaitForSeconds(0.155f);

        animator.SetBool("hit", false);
        hit = false;

    }
}
