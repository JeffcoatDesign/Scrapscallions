using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventoryReset : MonoBehaviour
{
    public GameObject inventoryOrigin;
    void OnEnable()
    {
        ResetInventory();
    }

    public void ResetInventory()
    {
        foreach (ItemSlot inventoryItem in GetComponentsInChildren<ItemSlot>())
            Destroy(inventoryItem.gameObject);
        foreach (ItemSlot inventoryItem in inventoryOrigin.GetComponentsInChildren<ItemSlot>())
        {
            GameObject myInventoryItem = Instantiate(inventoryItem.gameObject);
            myInventoryItem.transform.SetParent(transform, false);
            myInventoryItem.GetComponentInChildren<DragDrop>().ResetDragDrop();
            myInventoryItem.GetComponentInChildren<DragDrop>().canvas = FindAnyObjectByType<Canvas>().GetComponent<Canvas>();
        }
    }
}
