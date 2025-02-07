using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    //The DragDrop's Tag
    private string itemTag;
    //The DragDrop attached to the item slot
    public DragDrop slotDragDrop;
    //The DragDrop occupying the ItemSlot
    public DragDrop itemOccupiedBy;
    private DragDrop secondArmDragDrop;
    //The DragDrop dropped into the ItemSlot
    private DragDrop dragDropInQuestion;

    public void OnDrop(PointerEventData eventData) 
    {
        Debug.Log("On Drop");
        //Check if DragDrop is null
        if (eventData.pointerDrag != null)
        {
            dragDropInQuestion = eventData.pointerDrag.GetComponent<DragDrop>();
            dragDropInQuestion.dropped = true;
            itemTag = eventData.pointerDrag.tag;
            //Check if DragDrop is put in the trash
            if (tag == "Trash")
            {
                if (dragDropInQuestion.dragDropOrigin != null)
                    ItemSlotDragDropTrash();
                else
                    dragDropInQuestion.ResetDragDrop();
            }
            else if (tag != itemTag || gameObject.layer == 3)
                dragDropInQuestion.ResetDragDrop();
            else if(tag == "Arm")
            {
                if (dragDropInQuestion.dragDropOrigin == null)
                {
                    if (itemOccupiedBy != null)
                        itemOccupiedBy.ResetDragDrop();
                    eventData.pointerDrag.GetComponent<RectTransform>().position = dragDropInQuestion.homeSlot.GetComponent<RectTransform>().position;
                    itemOccupiedBy = dragDropInQuestion;
                    ItemSlotDragDropEnable(eventData);
                }
                else
                {
                    if (itemOccupiedBy != null)
                        itemOccupiedBy.ResetDragDrop();
                    itemOccupiedBy = dragDropInQuestion.dragDropOrigin;
                    dragDropInQuestion.ResetItemSlotDragDrop();
                    ArmItemSlotDragDropEnable(eventData);
                }
            }
            else
            {
                if (itemOccupiedBy != null)
                    itemOccupiedBy.ResetDragDrop();
                eventData.pointerDrag.GetComponent<RectTransform>().position = dragDropInQuestion.homeSlot.GetComponent<RectTransform>().position;
                itemOccupiedBy = dragDropInQuestion;
                ItemSlotDragDropEnable(eventData);
            }
        }
    }

    public void ArmItemSlotDragDropEnable(PointerEventData eventData)
    {
        slotDragDrop.gameObject.SetActive(true);
        slotDragDrop.GetComponent<Image>().sprite = dragDropInQuestion.GetComponent<Image>().sprite;
        slotDragDrop.dragDropOrigin = itemOccupiedBy;
    }

    public void ItemSlotDragDropEnable(PointerEventData eventData)
    {
        slotDragDrop.gameObject.SetActive(true);
        slotDragDrop.GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
        slotDragDrop.dragDropOrigin = itemOccupiedBy;
    }

    public void ItemSlotDragDropTrash()
    {
        Debug.Log("On Trash");
        dragDropInQuestion.slotOccupying.itemOccupiedBy = null;
        dragDropInQuestion.slotOccupying = dragDropInQuestion.homeSlot;
        dragDropInQuestion.dragDropOrigin.ResetDragDrop();
        dragDropInQuestion.ResetItemSlotDragDrop();
    }
}
