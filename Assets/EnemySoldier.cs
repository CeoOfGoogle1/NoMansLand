using System;
using UnityEngine;

public class EnemySoldier : MonoBehaviour
{
    Animator animator;

    [Header("Parameters")]
    [SerializeField] float maxHealth;
    [SerializeField] int maxAmmo;
    [SerializeField] float playerReachedDistance;
    [SerializeField] GameObject retreatPoint;
    [Header("CurrentState")]
    [SerializeField] private EnemySoldierBehaviour currentBehaviour;
    [SerializeField] private float currentHealth;
    [SerializeField] private int currentAmmo;
    [SerializeField] bool isInSafety;
    [Header("MovementState")]

    [SerializeField] bool isRunning;
    [SerializeField] bool isShooting;
    [SerializeField] bool isHealing;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        UpdateMovement();

        UpdateAnimator();

        UpdateBehaviour();

        Act();
    }

    private void Act()
    {
        
    }

    private void UpdateBehaviour()
    {
        if (currentAmmo <= 0 || currentHealth <= 20)
        {
            
        }
    }

    void UpdateMovement()
    {
        isRunning = false;
        isShooting = false;
        isHealing = false;

        if(currentBehaviour == EnemySoldierBehaviour.Relax)
        {
            return; 
        }
        else if(currentBehaviour == EnemySoldierBehaviour.Heal)
        {
            isHealing = true;            
        }
        else if(currentBehaviour == EnemySoldierBehaviour.RunToDestination || currentBehaviour == EnemySoldierBehaviour.Retreat)
        {
            isRunning = true;
        }
        else if(currentBehaviour == EnemySoldierBehaviour.Shoot)
        {
            isShooting = true;
        }

        if (isHealing)
        {
            if(!GetComponentInChildren<ParticleSystem>().isPlaying)
            {
               GetComponentInChildren<ParticleSystem>().Play(true); 
            }
        }
        else
        {
            if(GetComponentInChildren<ParticleSystem>().isPlaying)
            {
                Debug.Log("Should stop palying anim");
               GetComponentInChildren<ParticleSystem>().Stop(); 
            }  
        }
    }

    private void UpdateAnimator()
    {
        
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isShooting", isShooting);
    }
}


public enum EnemySoldierBehaviour
{
    Relax,
    Heal,
    RunToDestination,
    Retreat,
    Shoot
}