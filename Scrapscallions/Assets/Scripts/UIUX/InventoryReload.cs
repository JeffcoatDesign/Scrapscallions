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
    [SerializeField] private GameObject tooExpensiveAlert;
    private Shop itemShop;
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
                SetDefaultPartsUI.Instance.SetParts();
                inventoryPopulated = true;
            }
            foreach (ItemSlot inventoryItem in GetComponentsInChildren<ItemSlot>())
            {
                inventoryItemID = inventoryItem.GetComponentInChildren<DragDrop>().botPart.ItemID;
                if (inventoryItemID != inventoryManager.myRobot.head.ItemID
                      && inventoryItemID != inventoryManager.myRobot.rightArm.ItemID
                      && inventoryItemID != inventoryManager.myRobot.leftArm.ItemID
                      && inventoryItemID != inventoryManager.myRobot.body.ItemID
                      && inventoryItemID != inventoryManager.myRobot.legs.ItemID)
                    inventoryItem.GetComponentInChildren<DragDrop>().ResetDragDrop();
                else
                    inventoryItem.GetComponentInChildren<DragDrop>().DisableDragDrop();

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
            foreach (ItemSlot inventoryItem in GetComponentsInChildren<ItemSlot>())
            {
                if (inventoryItem.gameObject.layer != 6)
                {
                    itemShop = inventoryItem.GetComponentInChildren<DragDrop>().AddComponent<Shop>();
                    itemShop.shopInventory = this;
                    itemShop.tooExpensiveAlert = tooExpensiveAlert;
                }
            }
        }
    }


    public void ClearInventory()
    {
        foreach (ItemSlot inventoryItem in GetComponentsInChildren<ItemSlot>())
        {
            Destroy(inventoryItem.gameObject);
        }
    }
}
