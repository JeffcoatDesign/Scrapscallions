using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private string itemTag;
    public bool occupied;

    public void OnDrop(PointerEventData eventData) 
    {
        Debug.Log("On Drop");
        eventData.pointerDrag.GetComponent<DragDrop>().dropped = true;
        itemTag = eventData.pointerDrag.tag;
        if (eventData.pointerDrag != null)
        {
            if (tag != itemTag || occupied)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().position = eventData.pointerDrag.GetComponent<DragDrop>().homePosition;
            }
            else
            {
                eventData.pointerDrag.GetComponent<DragDrop>().homeSlot = this;
                eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
                eventData.pointerDrag.GetComponent<DragDrop>().homePosition = GetComponent<RectTransform>().position;
                occupied = true;
            }
        }
        eventData.pointerDrag.GetComponent<DragDrop>().dropped = false;
    }
}
