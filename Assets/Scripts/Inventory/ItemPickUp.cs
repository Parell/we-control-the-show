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
        var inventory = collider.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventory) return;

        if (inventory.AddToInventory(ItemData, 1))
        {
            // Todo: Pick up animation and sound?
            Destroy(gameObject);
        }
    }
}
