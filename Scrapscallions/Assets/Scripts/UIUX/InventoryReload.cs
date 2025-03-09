using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryReload : MonoBehaviour
{
    public InventoryManager inventoryManager;
    [SerializeField] private bool isShop;
    void OnEnable()
    {
        ResetInventory();
    }

    public void ResetInventory()
    {
        int inventoryItemID;
        foreach (ItemSlot inventoryItem in GetComponentsInChildren<ItemSlot>())
        {
            if (!isShop)
            {
                inventoryItemID = inventoryItem.GetComponentInChildren<DragDrop>().botPart.ItemID;
                if (   (inventoryManager.myRobot.head != null && inventoryItemID != inventoryManager.myRobot.head.ItemID)
                    && (inventoryManager.myRobot.rightArm != null && inventoryItemID != inventoryManager.myRobot.rightArm.ItemID)
                    && (inventoryManager.myRobot.leftArm != null && inventoryItemID != inventoryManager.myRobot.leftArm.ItemID)
                    && (inventoryManager.myRobot.body != null && inventoryItemID != inventoryManager.myRobot.body.ItemID)
                    && (inventoryManager.myRobot.legs != null && inventoryItemID != inventoryManager.myRobot.legs.ItemID))
                    Destroy(inventoryItem.gameObject);
            }
            else
                Destroy(inventoryItem.gameObject);
        }
        foreach (RobotPart inventoryItem in inventoryManager.itemParts)
        {
            if (!isShop)
            {
                inventoryItemID = inventoryItem.ItemID;
                if (   (inventoryManager.myRobot.head != null && inventoryItemID != inventoryManager.myRobot.head.ItemID)
                    && (inventoryManager.myRobot.rightArm != null && inventoryItemID != inventoryManager.myRobot.rightArm.ItemID)
                    && (inventoryManager.myRobot.leftArm != null && inventoryItemID != inventoryManager.myRobot.leftArm.ItemID)
                    && (inventoryManager.myRobot.body != null && inventoryItemID != inventoryManager.myRobot.body.ItemID)
                    && (inventoryManager.myRobot.legs != null && inventoryItemID != inventoryManager.myRobot.legs.ItemID))
                    inventoryManager.ReloadToInventory(inventoryItem, gameObject);
            }
            else
                inventoryManager.ReloadToInventory(inventoryItem, gameObject);
        }
    }
}
