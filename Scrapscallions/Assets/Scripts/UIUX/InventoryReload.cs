using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

                if (inventoryManager.myRobot.head == null && inventoryItem.GetComponentInChildren<DragDrop>().botPart is RobotPartHead)
                {
                    Debug.Log("No Head");
                    inventoryItem.GetComponentInChildren<DragDrop>().ResetDragDrop();
                }
                else if (inventoryManager.myRobot.body == null && inventoryItem.GetComponentInChildren<DragDrop>().botPart is RobotPartBody)
                {
                    Debug.Log("No Body");
                    inventoryItem.GetComponentInChildren<DragDrop>().ResetDragDrop();
                }
                else if (inventoryManager.myRobot.leftArm == null && inventoryItem.GetComponentInChildren<DragDrop>().botPart is RobotPartArm)
                {
                    Debug.Log("No Left Arm");
                    inventoryItem.GetComponentInChildren<DragDrop>().ResetDragDrop();
                }
                else if (inventoryManager.myRobot.rightArm == null && inventoryItem.GetComponentInChildren<DragDrop>().botPart is RobotPartArm)
                {
                    Debug.Log("No Right Arm");
                    inventoryItem.GetComponentInChildren<DragDrop>().ResetDragDrop();
                }
                else if (inventoryManager.myRobot.legs == null && inventoryItem.GetComponentInChildren<DragDrop>().botPart is RobotPartLegs)
                {
                    Debug.Log("No Legs");
                    inventoryItem.GetComponentInChildren<DragDrop>().ResetDragDrop();
                }
                else if (inventoryItemID != inventoryManager.myRobot.head.ItemID
                      && inventoryItemID != inventoryManager.myRobot.rightArm.ItemID
                      && inventoryItemID != inventoryManager.myRobot.leftArm.ItemID
                      && inventoryItemID != inventoryManager.myRobot.body.ItemID
                      && inventoryItemID != inventoryManager.myRobot.legs.ItemID)
                {
                    Debug.Log("Item " + inventoryItemID + " was not equipped");
                    inventoryItem.GetComponentInChildren<DragDrop>().ResetDragDrop();
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
            foreach (DragDrop inventoryItem in GetComponentsInChildren<DragDrop>())
            {
                inventoryItem.GetComponent<Image>().raycastTarget = false;
            }
        }
    }
}
