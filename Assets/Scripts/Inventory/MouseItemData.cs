using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Todo: Make into singleton
public class MouseItemData : MonoBehaviour
{
    public float _dropOffset = 1f;
    public Image ItemSprite;
    public Text ItemCount;
    public InventorySlot AssignedInventorySlot;

    private Transform _playerTransform;

    void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemSprite.preserveAspect = true;
        ItemCount.text = "";

        // Todo: Get referance from game manager
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (_playerTransform == null) Debug.Log("Player not found");
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();
    }

    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignedInventorySlot.ItemData.Icon;
        ItemCount.text = AssignedInventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    void Update()
    {
        // Todo: Add controller support
        if (AssignedInventorySlot.ItemData != null) // If has item, then follow the mouse
        {
            transform.position = Input.mousePosition;

            if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
            {
                // Todo: Drop item on the ground

                if (AssignedInventorySlot.ItemData.ItemPrefab != null)
                {
                    Instantiate(AssignedInventorySlot.ItemData.ItemPrefab,
                     _playerTransform.position + _playerTransform.forward * _dropOffset, Quaternion.identity);
                }

                if (AssignedInventorySlot.StackSize > 1)
                {
                    AssignedInventorySlot.AddToStack(-1);
                    UpdateMouseSlot();
                }
                else
                {
                    ClearSlot();
                }
            }
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    public static bool IsMouseOverUI()
    {
        PointerEventData eventDataCurrentPostion = new PointerEventData(EventSystem.current);
        eventDataCurrentPostion.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPostion, results);
        return results.Count > 0;
    }
}
