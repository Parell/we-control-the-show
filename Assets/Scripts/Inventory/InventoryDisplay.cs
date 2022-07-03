using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem; //Change: Set using singleton
    protected InventorySystem inventorySystem;
    protected Dictionary<UIInventorySlot, InventorySlot> slotDictionary;
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<UIInventorySlot, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(InventorySystem invToDisplay);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }

    public void SlotClicked(UIInventorySlot clickedUISlot)
    {
        bool isShiftPressed = Input.GetKeyDown(KeyCode.LeftShift);

        if (clickedUISlot.AssingedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            if (isShiftPressed)
            {

            }
            else
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssingedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
        }

        if (clickedUISlot.AssingedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssingedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
        }

        if (clickedUISlot.AssingedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            bool isSameItem = clickedUISlot.AssingedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;

            if (isSameItem && clickedUISlot.AssingedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                clickedUISlot.AssingedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
            }
            else if (isSameItem && !clickedUISlot.AssingedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                if (leftInStack < 1) SwapSlots(clickedUISlot); //Stack is full so swap items
                else //Slot stack not make take from mouse inventory
                {
                    int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
                    clickedUISlot.AssingedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();

                    var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                }
            }
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
            }
        }
    }

    void SwapSlots(UIInventorySlot clickedUISlot)
    {
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();

        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssingedInventorySlot);

        clickedUISlot.ClearSlot();
        clickedUISlot.AssingedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
