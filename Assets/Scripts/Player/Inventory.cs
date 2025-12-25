using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ItemStack
{
    public ItemData Item;
    public int Count;
}
public class Inventory : MonoBehaviour
{
    public List<ItemStack> items = new List<ItemStack>();
    public int selectedIndex;
    public ItemController itemController;
    public float totalWeight = 0;
    public float weightSpeedFactor = 1f;
    public float weightStaminaFactor = 1f;

    void Update()
    {
        HandleSelectionInput();
    }

    void HandleSelectionInput()
    {
        if (items.Count == 0) return;

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            selectedIndex += scroll > 0 ? 1 : -1;
            selectedIndex = Mathf.Clamp(selectedIndex, 0, items.Count - 1);
            itemController.SelectItem(Selected);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) Select(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Select(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Select(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Select(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Select(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) Select(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) Select(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) Select(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) Select(8);
    }

    void Select(int index)
    {
        selectedIndex = Mathf.Clamp(index, 0, items.Count - 1);
        itemController.SelectItem(Selected);
    }

    public ItemStack Selected =>
        items.Count > 0 ? items[Mathf.Clamp(selectedIndex, 0, items.Count - 1)] : null;

    public void AddItem(ItemData item, int amount)
    {
        if (item.isStackable)
        {
            var stack = items.Find(i => i.Item == item);
            if (stack != null) stack.Count += amount;
            else items.Add(new ItemStack { Item = item, Count = amount });
        }
        else
        {
            for (int i = 0; i < amount; i++)
                items.Add(new ItemStack { Item = item, Count = 1 });
        }

        RecalculateWeight();
    }

    public void RemoveItem(ItemData item, int count)
    {
        var stack = items.Find(i => i.Item == item);
        if (stack != null)
        {
            if (stack.Count > count)
            {
                stack.Count -= count;
            }
            else
            {
                items.Remove(stack);
            }
        }

        RecalculateWeight();
    }

    void RecalculateWeight()
    {
        totalWeight = 0;
        foreach (var i in items)
            totalWeight += i.Item.weight * i.Count;

        Debug.Log("Total Inventory Weight: " + totalWeight);

        WeightDebuffs();
    }

    private void WeightDebuffs()
    {
        // Assuming max carry weight is 100 for calculation
        float maxCarryWeight = 100f;
        weightSpeedFactor = 1 - (totalWeight / maxCarryWeight);
        weightStaminaFactor = 1 - (totalWeight / maxCarryWeight);
    }
}
