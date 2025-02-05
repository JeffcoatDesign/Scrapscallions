using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private string itemTag;
    public DragDrop slotDragDrop;
    public DragDrop itemOccupiedBy;
    private DragDrop secondArmDragDrop;

    public void OnDrop(PointerEventData eventData) 
    {
        Debug.Log("On Drop");
        eventData.pointerDrag.GetComponent<DragDrop>().dropped = true;
        itemTag = eventData.pointerDrag.tag;
        if (eventData.pointerDrag != null)
        {
            if (tag == "Trash")
                ItemSlotDragDropTrash(eventData);
            if (tag != itemTag || gameObject.layer == 3)
                eventData.pointerDrag.GetComponent<DragDrop>().ResetDragDrop();
            else
            {
                eventData.pointerDrag.GetComponent<RectTransform>().position = eventData.pointerDrag.GetComponent<DragDrop>().homeSlot.GetComponent<RectTransform>().position;
                itemOccupiedBy = eventData.pointerDrag.GetComponent<DragDrop>();
                ItemSlotDragDropEnable(eventData);
            }
            if (tag == itemTag && eventData.pointerDrag.GetComponent<DragDrop>().dragDropOrigin != null)
            {
                secondArmDragDrop = eventData.pointerDrag.GetComponent<DragDrop>().dragDropOrigin;
                eventData.pointerDrag.GetComponent<DragDrop>().dragDropOrigin.dragDropOrigin = eventData.pointerDrag.GetComponent<DragDrop>().dragDropOrigin;
                secondArmDragDrop.ResetItemSlotDragDrop();
            }
        }
    }

    public void ItemSlotDragDropEnable(PointerEventData eventData)
    {
        slotDragDrop.gameObject.SetActive(true);
        slotDragDrop.GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
        slotDragDrop.dragDropOrigin = itemOccupiedBy;
    }

    public void ItemSlotDragDropTrash(PointerEventData eventData)
    {
        Debug.Log("On Trash");
        eventData.pointerDrag.GetComponent<DragDrop>().dragDropOrigin.ResetDragDrop();
        eventData.pointerDrag.GetComponent<DragDrop>().ResetItemSlotDragDrop();
    }
}
