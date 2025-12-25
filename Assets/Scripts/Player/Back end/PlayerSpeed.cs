using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{
    [SerializeField] private float staminaRegenPerSecond = 5;
    [SerializeField] private float staminaDrainPerSecond = 10;
    private float maxStamina = 100;
    private float initialMaxStamina;
    private float currentStamina;
    public bool canRun = true;
    private MovementController movementController;
    private PlayerHealth playerHealth;
    // weight
    private PlayerSleep playerSleep;
    private PlayerHunger playerHunger;
    private PlayerThirst playerThirst;
    private Inventory inventory;

    void Awake()
    {
        movementController = GetComponent<MovementController>();
        playerHealth = GetComponent<PlayerHealth>();
        // weight
        playerSleep = GetComponent<PlayerSleep>();
        playerHunger = GetComponent<PlayerHunger>();
        playerThirst = GetComponent<PlayerThirst>();
        inventory = GetComponent<Inventory>();

        initialMaxStamina = maxStamina;
        currentStamina = maxStamina;
    }

    void Update()
    {
        maxStamina = initialMaxStamina * GetStaminaFactor();
        DrainStamina();
        RegenerateStamina();
        CheckIfCanRun();

        //Debug.Log("Current Stamina: " + currentStamina);

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    private void DrainStamina()
    {
        if (movementController.isRunning && canRun)
        {
            currentStamina -= staminaDrainPerSecond * Time.deltaTime;
        }
    }

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenPerSecond * Time.deltaTime;
        }
    }

    private void CheckIfCanRun()
    {
        if (currentStamina <= 0)
        {
            canRun = false;
        }
        else if (currentStamina >= maxStamina * 0.2f)
        {
            canRun = true;
        }
    }

    public float GetSpeedFactor()
    {
        float speedFactor = 1f
            * playerHealth.legHealthSpeedFactor
            * inventory.weightSpeedFactor
            * playerSleep.tirednessSpeedFactor
            * playerHunger.hungerSpeedFactor
            * playerThirst.thirstSpeedFactor;

        return Mathf.Clamp(speedFactor, 0.1f, 1f);
    }

    public float GetStaminaFactor()
    {
        float staminaFactor = 1f
            * playerHealth.legHealthStaminaFactor
            * inventory.weightStaminaFactor
            * playerSleep.tirednessStaminaFactor
            * playerHunger.hungerStaminaFactor
            * playerThirst.thirstStaminaFactor;

        return Mathf.Clamp(staminaFactor, 0.1f, 1f);
    }
}
