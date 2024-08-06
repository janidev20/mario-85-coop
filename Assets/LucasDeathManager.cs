using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucasDeathManager : MonoBehaviour
{

   [Header("Components")]
   [SerializeField] Animator animator;
   [SerializeField] SpriteRenderer sr;
   [SerializeField] Collider2D cd;

   [Header("Booleans")]
   [SerializeField] private bool didYouDieYet = false;

   [Header("Audio")]
   [SerializeField] private AudioSource SRC;
   [SerializeField] private AudioClip DeathSound;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!didYouDieYet)
        {
            if (LucasController.LucasIsDead)
            {
                animator.SetTrigger("dead");
                sr.sortingLayerID = 0;
                cd.enabled = false;
                SRC.PlayOneShot(DeathSound);
                didYouDieYet = true;
            }
        }
    }
}
