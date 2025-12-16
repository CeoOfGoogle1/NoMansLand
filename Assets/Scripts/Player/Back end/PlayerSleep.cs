using Unity.VisualScripting;
using UnityEngine;

public class PlayerSleep : MonoBehaviour
{
    [SerializeField] private float tirednessGainPerMinute = 1;
    [SerializeField] private float sleepPassiveHealThreshold = 50;
    [SerializeField] private float sleepHeal = 1;
    private float maxTiredness = 100;
    private float currentTiredness = 0;
    public float tirednessSpeedFactor = 1f;
    public float tirednessStaminaFactor = 1f;
    PlayerHealth playerHealth;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        GainTiredness();
        TirednessDebuffs();

        if (currentTiredness >= maxTiredness)
        {
            MaxTirednessReached();
        }
    }

    private void GainTiredness()
    {
        currentTiredness += tirednessGainPerMinute * Time.deltaTime / 60f;
        currentTiredness = Mathf.Clamp(currentTiredness, 0, maxTiredness);
    }

    private void TirednessDebuffs()
    {
        tirednessSpeedFactor = 1 - (currentTiredness / maxTiredness);
        tirednessStaminaFactor = 1 - (currentTiredness / maxTiredness);

        if (currentTiredness >= sleepPassiveHealThreshold)
        {
            playerHealth.isTired = true;
        }
        else if (currentTiredness < sleepPassiveHealThreshold)
        {
            playerHealth.isTired = false;
        }
    }

    public void Sleep(float sleepDurationInMinutes)
    {
        sleepDurationInMinutes *= 3;
        currentTiredness -= sleepDurationInMinutes;
        playerHealth.AddHealthToAll(sleepHeal * sleepDurationInMinutes);
    }

    private void MaxTirednessReached()
    {
        //dies
    }
}
