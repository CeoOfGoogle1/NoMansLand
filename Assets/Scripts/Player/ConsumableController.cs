using UnityEngine;

public class ConsumableController : MonoBehaviour
{
    Consumable consumable;
    ItemStack stack;
    public float increaseAmount;
    public enum ConsumableType
    {
        Drink,
        Food,
        Bandage,
    }
    public ConsumableType consumableType;
    public PlayerHunger playerHunger;
    public PlayerThirst playerThirst;
    public PlayerHealth playerHealth;

    void Awake()
    {
        playerHunger = GetComponent<PlayerHunger>();
        playerThirst = GetComponent<PlayerThirst>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void SetConsumable(Consumable c, ItemStack s)
    {
        consumable = c;
        stack = s;
    }

    public void Clear()
    {
        consumable = null;
        stack = null;
    }

    void Update()
    {
        if (consumable == null) return;

        increaseAmount = consumable.increaseAmount;
        consumableType = (ConsumableType)(int)consumable.consumableType;

        if (Input.GetMouseButtonDown(0))
        {
            if (consumableType == ConsumableType.Food)
            {
                playerHunger.Eat(increaseAmount);
            }
            else if (consumableType == ConsumableType.Drink)
            {
                playerThirst.Drink(increaseAmount);
            }
            else if (consumableType == ConsumableType.Bandage)
            {
                playerHealth.AddHealthToLowest(increaseAmount);
            }

            stack.Count--;

            if (stack.Count <= 0)
                Clear();
        }
    }
}
