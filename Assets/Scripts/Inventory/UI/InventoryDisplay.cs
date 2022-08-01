using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem; // Todo: Set using singleton

    protected InventorySystem inventorySystem;
    protected Dictionary<UIInventorySlot, InventorySlot> slotDictionary; // Pair up the UI slots with the system slots
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<UIInventorySlot, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start() { }

    public abstract void AssignSlot(InventorySystem invToDisplay, int offset); // Implimented in child classes

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) // Slot value in slot system
            {
                slot.Key.UpdateUISlot(updatedSlot); // Slot key for UI value
            }
        }
    }

    public void SlotClicked(UIInventorySlot clickedUISlot)
    {
        bool isShiftPressed = Input.GetKey(KeyCode.LeftShift);

        // Dose the clicked slot have item and dose the mouse have no item
        if (clickedUISlot.AssingedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            // If holding shift, then split the stack
            if (isShiftPressed && clickedUISlot.AssingedInventorySlot.SplitStack(out InventorySlot halfStackSlot)) //Split stack
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else // Pick up the item in the clicked slot
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssingedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
        }

        // Clicked slot dosen't have an item and mouse dose have an item, then place mouse item in free slot
        if (clickedUISlot.AssingedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssingedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
            return;
        }

        // If both items are the same, then combine them
        // If the slot stack size + mouse stack size > the max stack size the, then take from mouse
        // If diffrent items, then swap the items

        // Do both slots have an item
        if (clickedUISlot.AssingedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            bool isSameItem = clickedUISlot.AssingedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;

            // If both items are the same, then combine them
            if (isSameItem && clickedUISlot.AssingedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                clickedUISlot.AssingedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
            }
            else if (isSameItem && !clickedUISlot.AssingedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                if (leftInStack < 1) SwapSlots(clickedUISlot); // Stack is full so swap the items
                else // Slot stack not at max, so take from mouse inventory
                {
                    int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
                    clickedUISlot.AssingedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();

                    var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
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
