using System;
using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    [SerializeField] private float hungerGainPerMinute = 1;
    [SerializeField] private float hungerPassiveHealThreshold = 50;
    private float maxHunger = 100;
    private float currentHunger = 0;
    public float hungerSpeedFactor = 1f;
    public float hungerStaminaFactor = 1f;
    PlayerHealth playerHealth;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        GainHunger();
        HungerDebuffs();

        if (currentHunger >= maxHunger)
        {
            MaxHungerReached();
        }

        //Debug.Log("Current Hunger: " + currentHunger);
    }

    private void GainHunger()
    {
        currentHunger += hungerGainPerMinute * Time.deltaTime / 60f;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }

    private void HungerDebuffs()
    {
        hungerSpeedFactor = 1 - (currentHunger / maxHunger);
        hungerStaminaFactor = 1 - (currentHunger / maxHunger);

        if (currentHunger >= hungerPassiveHealThreshold)
        {
            playerHealth.isHungry = true;
        }
        else if (currentHunger < hungerPassiveHealThreshold)
        {
            playerHealth.isHungry = false;
        }
    }

    public void Eat(float foodAmount)
    {
        foodAmount *= 3;
        currentHunger -= foodAmount;
    }

    private void MaxHungerReached()
    {
        //dies
    }
}
