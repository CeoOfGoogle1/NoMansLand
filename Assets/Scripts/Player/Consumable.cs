using UnityEngine;

public class Consumable : MonoBehaviour
{
    public float increaseAmount;

    public void Use()
    {
        Debug.Log("Consumable used.");
    }
}
