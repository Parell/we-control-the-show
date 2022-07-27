using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int secondaryInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;

    public InventorySystem SecondaryInventorySystem => SecondaryInventorySystem;

    protected override void Awake()
    {
        base.Awake();

        secondaryInventorySystem = new InventorySystem(secondaryInventorySize);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            OnDynamicInventoryDisplayRequested?.Invoke(secondaryInventorySystem);
        }
    }
}
