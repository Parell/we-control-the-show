using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryHolder : MonoBehaviour
{
    [SerializeField] int inventorySize;
    [SerializeField] protected InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;

    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
    }
}
