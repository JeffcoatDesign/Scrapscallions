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

    public SFXPlayer sfxPlayer;

    void Start()
    {
        sfxPlayer = FindAnyObjectByType<SFXPlayer>();
    }

    public void OnDrop(PointerEventData eventData) 
    {
        if (eventData.pointerDrag != null)
        {
            dragDropInQuestion = eventData.pointerDrag.GetComponent<DragDrop>();
            if (dragDropInQuestion != null)
            {
                dragDropInQuestion.dropped = true;
                itemTag = eventData.pointerDrag.tag;
                if(this == dragDropInQuestion.homeSlot)
                {
                    dragDropInQuestion.ResetDragDrop();
                }
                //Check if DragDrop is put in the trash
                else if (tag == "Trash")
                {
                    //Check if DragDrop is dragged from Equip Region or Inventory and react accordingly
                    if (dragDropInQuestion.dragDropOrigin != null)
                        ItemSlotDragDropTrash();
                    else
                        dragDropInQuestion.ResetDragDrop();
                }
                //Check if DragDrop is the correct Part Type and not being dragged to a different inventory slot
                else if (tag != itemTag || gameObject.layer == 3)
                    dragDropInQuestion.ResetDragDrop();
                else
                {
                    sfxPlayer.EquipPart();

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

                    switch (gameObject.name)
                    {
                        case "Head":
                            InventoryManager.Instance.myRobot.head = (RobotPartHead)itemOccupiedBy.botPart;
                            break;
                        case "Body":
                            InventoryManager.Instance.myRobot.body = (RobotPartBody)itemOccupiedBy.botPart;
                            break;
                        case "Left Arm":
                            InventoryManager.Instance.myRobot.leftArm = (RobotPartArm)itemOccupiedBy.botPart;
                            break;
                        case "Right Arm":
                            InventoryManager.Instance.myRobot.rightArm = (RobotPartArm)itemOccupiedBy.botPart;
                            break;
                        case "Legs":
                            InventoryManager.Instance.myRobot.legs = (RobotPartLegs)itemOccupiedBy.botPart;
                            break;
                    }

                    slotDragDrop.botPart = dragDropInQuestion.botPart;
                }
            }
        }
    }

    public void ForceEquip(DragDrop ddInQuestion)
    {
        ddInQuestion.dropped = true;
        itemTag = ddInQuestion.tag;
        //Check if DragDrop is put in the trash
        if (tag == "Trash")
        {
            //Check if DragDrop is dragged from Equip Region or Inventory and react accordingly
            if (ddInQuestion.dragDropOrigin != null)
                ItemSlotDragDropTrash();
            else
                ddInQuestion.ResetDragDrop();
        }
        //Check if DragDrop is the correct Part Type and not being dragged to a different inventory slot
        else if (tag != itemTag || gameObject.layer == 3)
            ddInQuestion.ResetDragDrop();
        else
        {
            //Check if part is specifically an Arm and being dragged from an Equip Region
            if (tag == "Arm" && ddInQuestion.dragDropOrigin != null)
            {
                //Check if Equip Region Slot is filled, and replace with the Part In Question if so
                if (itemOccupiedBy != null)
                    itemOccupiedBy.ResetDragDrop();
                //Clear the previous Equip Region's item and set the current Equip Region's item
                ddInQuestion.homeSlot.itemOccupiedBy = null;
                itemOccupiedBy = ddInQuestion.dragDropOrigin;
                ddInQuestion.ResetItemSlotDragDrop();
                ItemSlotDragDropEnable();
            }
            else
            {
                //Check if Equip Region Slot is filled, and replace with the Part In Question if so
                if (itemOccupiedBy != null)
                    itemOccupiedBy.ResetDragDrop();
                //Send the DragDrop dragged from the inventory back to the inventory, create a reference to it on the Equip Region, and enable the Equip Region's DragDrop
                ddInQuestion.GetComponent<RectTransform>().position = ddInQuestion.homeSlot.GetComponent<RectTransform>().position;
                itemOccupiedBy = ddInQuestion;
                ItemSlotDragDropEnable(ddInQuestion);
            }

            switch (gameObject.name)
            {
                case "Head":
                    InventoryManager.Instance.myRobot.head = (RobotPartHead)itemOccupiedBy.botPart;
                    break;
                case "Body":
                    InventoryManager.Instance.myRobot.body = (RobotPartBody)itemOccupiedBy.botPart;
                    break;
                case "Left Arm":
                    InventoryManager.Instance.myRobot.leftArm = (RobotPartArm)itemOccupiedBy.botPart;
                    break;
                case "Right Arm":
                    InventoryManager.Instance.myRobot.rightArm = (RobotPartArm)itemOccupiedBy.botPart;
                    break;
                case "Legs":
                    InventoryManager.Instance.myRobot.legs = (RobotPartLegs)itemOccupiedBy.botPart;
                    break;
            }
            slotDragDrop.botPart = ddInQuestion.botPart;
        }
    }
    public void ItemSlotDragDropEnable()
    {
        //Enable the Equip Region's DragDrop, set the sprite to that of the DragDrop dragged in from the inventory, and set the OccupiedBy var
        slotDragDrop.gameObject.SetActive(true);
        slotDragDrop.GetComponent<Image>().sprite = dragDropInQuestion.GetComponent<Image>().sprite;
        slotDragDrop.dragDropOrigin = itemOccupiedBy;
    }

    public void ItemSlotDragDropEnable(DragDrop ddInQuestion)
    {
        //Enable the Equip Region's DragDrop, set the sprite to that of the DragDrop dragged in from the inventory, and set the OccupiedBy var
        slotDragDrop.gameObject.SetActive(true);
        slotDragDrop.GetComponent<Image>().sprite = ddInQuestion.GetComponent<Image>().sprite;
        slotDragDrop.dragDropOrigin = itemOccupiedBy;
    }

    public void ItemSlotDragDropTrash()
    {
        sfxPlayer.Trash();
        //Clear the Equip Region's variables, and reset all related DragDrops
        switch (dragDropInQuestion.slotOccupying.gameObject.name)
        {
            case "Head":
                InventoryManager.Instance.myRobot.head = null;
                break;
            case "Body":
                InventoryManager.Instance.myRobot.body = null;
                break;
            case "Left Arm":
                InventoryManager.Instance.myRobot.leftArm = null;
                break;
            case "Right Arm":
                InventoryManager.Instance.myRobot.rightArm = null;
                break;
            case "Legs":
                InventoryManager.Instance.myRobot.legs = null;
                break;
        }
        dragDropInQuestion.slotOccupying.itemOccupiedBy = null;
        dragDropInQuestion.slotOccupying = dragDropInQuestion.homeSlot;
        dragDropInQuestion.dragDropOrigin.ResetDragDrop();
        dragDropInQuestion.ResetItemSlotDragDrop();
    }
}
