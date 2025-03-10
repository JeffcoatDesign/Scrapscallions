using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryReload : MonoBehaviour
{
    public InventoryManager inventoryManager;
    [SerializeField] private bool isShop;
    [SerializeField] private bool inventoryPopulated;
    int inventoryItemID;

    private void Start()
    {
        if(isShop)
            inventoryManager = GameObject.FindWithTag("InventoryParent").GetComponent<InventoryManager>();
    }
    void OnEnable()
    {
        ResetInventory();
    }

    public void ResetInventory()
    {
        if (!isShop)
        {
            if (!inventoryManager.isFirstTime && !inventoryPopulated)
            {
                foreach (RobotPart inventoryItem in inventoryManager.itemParts)
                {
                    inventoryItemID = inventoryItem.ItemID;
                    inventoryManager.InstantiateInventoryItem(inventoryItem, gameObject);
                }
                inventoryPopulated = true;
            }
            foreach (ItemSlot inventoryItem in GetComponentsInChildren<ItemSlot>())
            {
                inventoryItemID = inventoryItem.GetComponentInChildren<DragDrop>().botPart.ItemID;
                if ((inventoryManager.myRobot.head != null && inventoryItemID != inventoryManager.myRobot.head.ItemID)
                    && (inventoryManager.myRobot.rightArm != null && inventoryItemID != inventoryManager.myRobot.rightArm.ItemID)
                    && (inventoryManager.myRobot.leftArm != null && inventoryItemID != inventoryManager.myRobot.leftArm.ItemID)
                    && (inventoryManager.myRobot.body != null && inventoryItemID != inventoryManager.myRobot.body.ItemID)
                    && (inventoryManager.myRobot.legs != null && inventoryItemID != inventoryManager.myRobot.legs.ItemID))
                {
                    Debug.Log("Item " + inventoryItemID + " was not equipped");
                }
                else
                {
                    Debug.Log("Item " + inventoryItemID + " was equipped");
                    inventoryItem.GetComponentInChildren<DragDrop>().DisableDragDrop();
                }

            }
        }
        else
        {
            foreach (ItemSlot inventoryItem in GetComponentsInChildren<ItemSlot>())
            {
                Destroy(inventoryItem.gameObject);
            }
            foreach (RobotPart inventoryItem in inventoryManager.itemParts)
            {
                inventoryManager.InstantiateInventoryItem(inventoryItem, gameObject);
            }
        }
    }
}
