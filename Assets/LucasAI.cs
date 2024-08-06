using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LucasController))]

public class LucasAI : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] LucasController movementScript;

    [Header("Layers")]
    [SerializeField] LayerMask JumpBig;
    [SerializeField] LayerMask JumpSmall;
    [SerializeField] LayerMask JumpDanger; // When MX is too close

    [Header("Control")]
    [SerializeField] private bool moveLeft;
    [SerializeField] private bool moveRight;
    [SerializeField] private bool run;



}
