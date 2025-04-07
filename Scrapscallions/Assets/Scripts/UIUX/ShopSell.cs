using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSell : MonoBehaviour, IDropHandler
{
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
                sfxPlayer.Buy();
                dragDropInQuestion.dropped = true;
                Debug.Log("Selling " + dragDropInQuestion.botPart);
                InventoryManager.Instance.RemoveFromInventory(dragDropInQuestion);
                dragDropInQuestion.GetComponentInParent<InventoryReload>().ResetInventory();
                Destroy(dragDropInQuestion.GetComponentInParent<ItemSlot>().gameObject);
            }
        }
    }
}
