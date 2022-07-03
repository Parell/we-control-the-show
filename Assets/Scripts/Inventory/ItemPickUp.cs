using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public float pickUpRadius = 1f;
    public InventoryItemData ItemData;

    SphereCollider myCollider;

    void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = pickUpRadius;
    }

    void OnTriggerEnter(Collider collider)
    {
        var inventory = collider.transform.GetComponent<InventoryHolder>();

        if (!inventory) return;

        if (inventory.InventorySystem.AddToInventory(ItemData, 1))
        {
            //Pop animation and sound?
            Destroy(gameObject);
        }
    }
}
