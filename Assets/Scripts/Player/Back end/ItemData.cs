using UnityEngine;

public enum ItemType
{
    Gun,
    Consumable
}

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public float weight;
    public bool isStackable;
    public int maxStackSize;
    public ItemType itemType;
    public GameObject itemPrefab;
}
