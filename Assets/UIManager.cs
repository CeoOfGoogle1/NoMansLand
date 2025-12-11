using System;

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image healthBarFill;
    [SerializeField] Image hungerBarFill;
    GameObject localPlayer;

    void Start()
    {
        localPlayer = FindAnyObjectByType<PlayerController>().gameObject;
    }

    void Update()
    {
        UpdateHungerBar();

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBarFill.fillAmount = localPlayer.GetComponent<PlayerHealth>().GetHealth();
    }

    private void UpdateHungerBar()
    {
        hungerBarFill.fillAmount = localPlayer.GetComponent<PlayerHunger>().GetHunger();
    }
}
