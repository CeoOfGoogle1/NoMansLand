using UnityEngine;

public class PlayerThirst : MonoBehaviour
{
    [SerializeField] private float thirstGainPerMinute = 1;
    [SerializeField] private float thirstPassiveHealThreshold = 50;
    private float maxThirst = 100;
    private float currentThirst = 0;
    public float thirstSpeedFactor = 1f;
    public float thirstStaminaFactor = 1f;
    PlayerHealth playerHealth;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        GainThirst();
        ThirstDebuffs();

        if (currentThirst >= maxThirst)
        {
            MaxThirstReached();
        }
    }

    private void GainThirst()
    {
        currentThirst += thirstGainPerMinute * Time.deltaTime / 60f;
        currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
    }

    private void ThirstDebuffs()
    {
        thirstSpeedFactor = 1 - (currentThirst / maxThirst);
        thirstStaminaFactor = 1 - (currentThirst / maxThirst);

        if (currentThirst >= thirstPassiveHealThreshold)
        {
            playerHealth.isThirsty = true;
        }
        else if (currentThirst < thirstPassiveHealThreshold)
        {
            playerHealth.isThirsty = false;
        }
    }

    public void Drink(float drinkAmount)
    {
        drinkAmount *= 3;
        currentThirst -= drinkAmount;
    }

    private void MaxThirstReached()
    {
        //dies
    }
}
