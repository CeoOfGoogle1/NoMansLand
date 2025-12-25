using UnityEngine;

public class Consumable : MonoBehaviour
{
    public float increaseAmount;
    public enum ConsumableType
    {
        Drink,
        Food,
        Bandage,
    }
    public ConsumableType consumableType;
}
