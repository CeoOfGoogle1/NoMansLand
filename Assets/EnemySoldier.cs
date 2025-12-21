using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemySoldier : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;

    GameObject player;

    [Header("Parameters")]
    [SerializeField] float maxHealth;
    [SerializeField] int maxAmmo;
    [SerializeField] float playerReachedDistance;
    [SerializeField] GameObject retreatPoint;
    [SerializeField] private float retreatPointReachedDistance;
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

        navMeshAgent = GetComponent<NavMeshAgent>();

        player = FindAnyObjectByType<MovementController>().gameObject;

        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
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
        if(currentBehaviour == EnemySoldierBehaviour.Relax || currentBehaviour == EnemySoldierBehaviour.Shoot || currentBehaviour == EnemySoldierBehaviour.Heal)
        {
            navMeshAgent.SetDestination(transform.position);
        }
        else if(currentBehaviour == EnemySoldierBehaviour.Retreat)
        {
            navMeshAgent.SetDestination(retreatPoint.transform.position);
        }
        else if(currentBehaviour == EnemySoldierBehaviour.RunToDestination)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    private void UpdateBehaviour()
    {
        isInSafety = Vector3.Distance(transform.position, retreatPoint.transform.position) < retreatPointReachedDistance;

        if (currentAmmo <= 0 || currentHealth <= 20 && !isInSafety)
        {
            currentBehaviour = EnemySoldierBehaviour.Retreat;
        }
        else if(currentAmmo <= 0 || currentHealth <= 20 && isInSafety)
        {
            currentBehaviour = EnemySoldierBehaviour.Heal;
        }
        else if(Vector3.Distance(transform.position, player.transform.position) > playerReachedDistance)
        {
            currentBehaviour = EnemySoldierBehaviour.RunToDestination;
        }
        else
        {
            currentBehaviour = EnemySoldierBehaviour.Shoot;
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

            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
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