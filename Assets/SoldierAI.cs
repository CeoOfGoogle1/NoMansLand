using System;
using UnityEngine;
using UnityEngine.AI;

public class SoldierAI : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;
    SoldierSurroundingCheck soldierSurroundingCheck;

    [Header("Parameters")]
    [SerializeField] SoldierSide soldierSide = SoldierSide.BadGuys;
    [SerializeField] float maxHealth;
    [SerializeField] int maxAmmo;
    [SerializeField] float shootDistance = 20f;
    [SerializeField] GameObject retreatPoint;
    [SerializeField] private float retreatPointReachedDistance;
    [Header("CurrentState")]
    [SerializeField] private SoldierBehaviour currentBehaviour;
    [SerializeField] private float currentHealth;
    [SerializeField] private int currentAmmo;
    [SerializeField] bool isInSafety;
    [Header("MovementState")]

    [SerializeField] bool isRunning;
    [SerializeField] bool isShooting;
    [SerializeField] bool isHealing;

    public void Initialize(SoldierSide side)
    {
        soldierSide = side;
    }


    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        soldierSurroundingCheck = GetComponentInChildren<SoldierSurroundingCheck>();

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
        if(currentBehaviour == SoldierBehaviour.Relax || currentBehaviour == SoldierBehaviour.Shoot || currentBehaviour == SoldierBehaviour.Heal)
        {
            navMeshAgent.SetDestination(transform.position);
        }
        else if(currentBehaviour == SoldierBehaviour.Retreat)
        {
            navMeshAgent.SetDestination(retreatPoint.transform.position);
        }
        else if(currentBehaviour == SoldierBehaviour.RunToDestination)
        {
            navMeshAgent.SetDestination(soldierSurroundingCheck.GetClosestEnemy().transform.position);
        }
    }

    private void UpdateBehaviour()
    {
        isInSafety = Vector3.Distance(transform.position, retreatPoint.transform.position) < retreatPointReachedDistance;

        if (currentAmmo <= 0 || currentHealth <= 20 && !isInSafety)
        {
            currentBehaviour = SoldierBehaviour.Retreat;
        }
        else if(currentAmmo <= 0 || currentHealth <= 20 && isInSafety)
        {
            currentBehaviour = SoldierBehaviour.Heal;
        }
        else if (soldierSurroundingCheck.HasEnemies())
        {
            if(Vector3.Distance(transform.position, soldierSurroundingCheck.GetClosestEnemy().transform.position) > shootDistance)
            {
                currentBehaviour = SoldierBehaviour.RunToDestination;
            }
            else
            {
               currentBehaviour = SoldierBehaviour.Shoot; 
            }            
        }
        else
        {
            currentBehaviour = SoldierBehaviour.Relax;
        }
    }

    void UpdateMovement()
    {
        isRunning = false;
        isShooting = false;
        isHealing = false;

        if(currentBehaviour == SoldierBehaviour.Relax)
        {
            return; 
        }
        else if(currentBehaviour == SoldierBehaviour.Heal)
        {
            isHealing = true;            
        }
        else if(currentBehaviour == SoldierBehaviour.RunToDestination || currentBehaviour == SoldierBehaviour.Retreat)
        {
            isRunning = true;
        }
        else if(currentBehaviour == SoldierBehaviour.Shoot)
        {
            isShooting = true;

            transform.LookAt(new Vector3(soldierSurroundingCheck.GetClosestEnemy().transform.position.x, transform.position.y, soldierSurroundingCheck.GetClosestEnemy().transform.position.z));
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
               GetComponentInChildren<ParticleSystem>().Stop(); 
            }  
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isShooting", isShooting);
    }

    public SoldierSide GetSoldierSide()
    {
        return soldierSide;
    }
}


public enum SoldierBehaviour
{
    Relax,
    Heal,
    RunToDestination,
    Retreat,
    Shoot
}

public enum SoldierSide
{
    GoodGuys,
    BadGuys
}