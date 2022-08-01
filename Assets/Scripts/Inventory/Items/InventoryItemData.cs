using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item", fileName = "Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID = -1;
    public string DisplayName;
    [TextArea(4, 4)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize;
    public int coinValue;
    public GameObject ItemPrefab;
}
