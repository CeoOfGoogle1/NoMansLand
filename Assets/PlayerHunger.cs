using System;
using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    [Header("Values")]
    [SerializeField, Tooltip("Percents Per Second")] private float defaultHungerDecraseRate = 1; 
    private float hunger = 1; // 0 - 1 (0 is hungry)

    void Update()
    {
        LoseHunger();
    }

    private void LoseHunger()
    {
        hunger -= Time.deltaTime * defaultHungerDecraseRate / 100;
    }

    public float GetHunger()
    {
        return hunger;
    }
}
