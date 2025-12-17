using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Transform itemMount;
    GunController gunController;
    ConsumableController consumableController;
    GameObject currentObject;

    public void SelectItem(ItemStack stack)
    {
        ClearCurrent();

        if (stack == null) return;

        currentObject = Instantiate(stack.Item.itemPrefab, itemMount);

        switch (stack.Item.itemType)
        {
            case ItemType.Gun:
                gunController = GetComponent<GunController>();
                gunController.SetGun(currentObject.GetComponent<Gun>());
            break;

            case ItemType.Consumable:
                consumableController = GetComponent<ConsumableController>();
                consumableController.SetConsumable(currentObject.GetComponent<Consumable>(), stack);
            break;
        }
    }

    void ClearCurrent()
    {
        if (currentObject)
            Destroy(currentObject);

        gunController?.Clear();
        consumableController?.Clear();
    }
}
