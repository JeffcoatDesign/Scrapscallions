using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventoryReset : MonoBehaviour
{
    public GameObject inventoryOrigin;
    void OnEnable()
    {
        foreach (ItemSlot inventoryItem in GetComponentsInChildren<ItemSlot>())
            Destroy(inventoryItem.gameObject);
        foreach (ItemSlot inventoryItem in inventoryOrigin.GetComponentsInChildren<ItemSlot>())
        {
            GameObject myInventoryItem = Instantiate(inventoryItem.gameObject);
            myInventoryItem.transform.SetParent(transform, false);
            myInventoryItem.GetComponentInChildren<DragDrop>().ResetDragDrop();
        }
    }
}
