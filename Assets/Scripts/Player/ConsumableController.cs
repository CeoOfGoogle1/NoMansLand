using UnityEngine;

public class ConsumableController : MonoBehaviour
{
    Consumable consumable;
    ItemStack stack;

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

        if (Input.GetMouseButtonDown(0))
        {
            consumable.Use();
            stack.Count--;

            if (stack.Count <= 0)
                Clear();
        }
    }
}
