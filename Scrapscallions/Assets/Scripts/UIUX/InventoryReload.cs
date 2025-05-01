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
    public bool inventoryPopulated;
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
                foreach (ItemSlot inventoryItem in GetComponentsInChildren<ItemSlot>())
                {
                    Destroy(inventoryItem.gameObject);
                }
                foreach (RobotPart inventoryItem in inventoryManager.itemParts)
                {
                    inventoryItemID = inventoryItem.ItemID;
                    inventoryManager.InstantiateInventoryItem(inventoryItem, gameObject);
                }
                InventoryManager.Instance.SDPUI.SetParts();
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
                DragDrop dd = inventoryItem.GetComponentInChildren<DragDrop>();
                if ((dd.botPart.ItemID == InventoryManager.Instance.myRobot.head.ItemID) ||
                    (dd.botPart.ItemID == InventoryManager.Instance.myRobot.body.ItemID) ||
                    (dd.botPart.ItemID == InventoryManager.Instance.myRobot.leftArm.ItemID) ||
                    (dd.botPart.ItemID == InventoryManager.Instance.myRobot.rightArm.ItemID) ||
                    (dd.botPart.ItemID == InventoryManager.Instance.myRobot.legs.ItemID))
                {
                    dd.canvasGroup.alpha = 0.5f;
                    dd.draggable = false;
                }
                if (inventoryItem.gameObject.layer != 6)
                {
                    itemShop = dd.AddComponent<Shop>();
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
