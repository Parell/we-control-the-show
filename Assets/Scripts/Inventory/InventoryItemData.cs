using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item", fileName = "New_Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID;
    public string DisplayName;
    [TextArea(4, 4)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize;
    public int coinValue;
}
