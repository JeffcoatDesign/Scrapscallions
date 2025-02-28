using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Scraps.Parts;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private string itemTag;
    //The DragDrop attached to the item slot
    public DragDrop slotDragDrop;
    //The DragDrop currently occupying the ItemSlot
    public DragDrop itemOccupiedBy;
    //The DragDrop dropped into the ItemSlot
    private DragDrop dragDropInQuestion;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    public void OnDrop(PointerEventData eventData) 
    {
        if (eventData.pointerDrag != null)
        {
            dragDropInQuestion = eventData.pointerDrag.GetComponent<DragDrop>();
            dragDropInQuestion.dropped = true;
            itemTag = eventData.pointerDrag.tag;
            //Check if DragDrop is put in the trash
            if (tag == "Trash")
            {
                //Check if DragDrop is dragged from Equip Region or Inventory and react accordingly
                if (dragDropInQuestion.dragDropOrigin != null)
                    ItemSlotDragDropTrash();
                else
                    dragDropInQuestion.ResetDragDrop();
            }
            //Check if DragDrop is the correct Part Type and not being dragged to a diffferent inventory slot
            else if (tag != itemTag || gameObject.layer == 3)
                dragDropInQuestion.ResetDragDrop();
            else
            {
                //Check if part is specifically an Arm and being dragged from an Equip Region
                if (tag == "Arm" && dragDropInQuestion.dragDropOrigin != null)
                {
                    //Check if Equip Region Slot is filled, and replace with the Part In Question if so
                    if (itemOccupiedBy != null)
                        itemOccupiedBy.ResetDragDrop();
                    //Clear the previous Equip Region's item and set the current Equip Region's item
                    dragDropInQuestion.homeSlot.itemOccupiedBy = null;
                    itemOccupiedBy = dragDropInQuestion.dragDropOrigin;
                    dragDropInQuestion.ResetItemSlotDragDrop();
                    ItemSlotDragDropEnable();
                }
                else
                {
                    //Check if Equip Region Slot is filled, and replace with the Part In Question if so
                    if (itemOccupiedBy != null)
                        itemOccupiedBy.ResetDragDrop();
                    //Send the DragDrop dragged from the inventory back to the inventory, create a reference to it on the Equip Region, and enable the Equip Region's DragDrop
                    eventData.pointerDrag.GetComponent<RectTransform>().position = dragDropInQuestion.homeSlot.GetComponent<RectTransform>().position;
                    itemOccupiedBy = dragDropInQuestion;
                    ItemSlotDragDropEnable();
                }

                switch(gameObject.name)
                {
                    case "Head":
                        inventoryManager.equippedHead = itemOccupiedBy.botPart;
                        break;
                    case "Body":
                        inventoryManager.equippedBody = itemOccupiedBy.botPart;
                        break;
                    case "Left Arm":
                        inventoryManager.equippedLArm = itemOccupiedBy.botPart;
                        break;
                    case "Right Arm":
                        inventoryManager.equippedRArm = itemOccupiedBy.botPart;
                        break;
                    case "Legs":
                        inventoryManager.equippedLegs = itemOccupiedBy.botPart;
                        break;
                }
            }
        }
    }

    public void ItemSlotDragDropEnable()
    {
        //Enable the Equip Region's DragDrop, set the sprite to that of the DragDrop dragged in from the inventory, and set the OccupiedBy var
        slotDragDrop.gameObject.SetActive(true);
        slotDragDrop.GetComponent<Image>().sprite = dragDropInQuestion.GetComponent<Image>().sprite;
        slotDragDrop.dragDropOrigin = itemOccupiedBy;
    }

    public void ItemSlotDragDropTrash()
    {
        //Clear the Equip Region's variables, and reset all related DragDrops
        switch (dragDropInQuestion.slotOccupying.gameObject.name)
        {
            case "Head":
                inventoryManager.equippedHead = null;
                break;
            case "Body":
                inventoryManager.equippedBody = null;
                break;
            case "Left Arm":
                inventoryManager.equippedLArm = null;
                break;
            case "Right Arm":
                inventoryManager.equippedRArm = null;
                break;
            case "Legs":
                inventoryManager.equippedLegs = null;
                break;
        }
        dragDropInQuestion.slotOccupying.itemOccupiedBy = null;
        dragDropInQuestion.slotOccupying = dragDropInQuestion.homeSlot;
        dragDropInQuestion.dragDropOrigin.ResetDragDrop();
        dragDropInQuestion.ResetItemSlotDragDrop();
    }
}
