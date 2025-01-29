using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private string itemTag;
    private ItemSlot homeSlot;

    public void OnDrop(PointerEventData eventData) 
    {
        eventData.pointerDrag.GetComponent<DragDrop>().dropped = true;
        itemTag = eventData.pointerDrag.tag;
        homeSlot = eventData.pointerDrag.GetComponent<DragDrop>().homeSlot;
        if (eventData.pointerDrag != null)
        {
            if (tag != itemTag && this != homeSlot)
                eventData.pointerDrag.GetComponent<RectTransform>().position = eventData.pointerDrag.GetComponent<DragDrop>().homePosition;
            else
            {
                eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
                eventData.pointerDrag.GetComponent<DragDrop>().homePosition = GetComponent<RectTransform>().position;
            }
        }
        eventData.pointerDrag.GetComponent<DragDrop>().dropped = false;
    }
}
