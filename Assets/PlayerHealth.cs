using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerHealthIsZero;

    private float health = 1; // 0 - 1

    void Update()
    {

    }

    public void AddHealth(float addAmount)
    {
        // can be negative

        health += addAmount;

        if (health <= 0)
        {
            OnPlayerHealthIsZero?.Invoke();
        }
    }

    public void SetHealth(float newHealth)
    {
        health = newHealth;

        if (health <= 0)
        {
            OnPlayerHealthIsZero?.Invoke();
        }
    }

    public float GetHealth()
    {
        return health;
    }
}
