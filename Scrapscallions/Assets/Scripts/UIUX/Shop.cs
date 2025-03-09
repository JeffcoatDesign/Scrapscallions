using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Scraps.Parts;

public class Shop : MonoBehaviour, IDropHandler
{
    //The DragDrop dropped into the ItemSlot
    private DragDrop dragDropInQuestion;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private InventoryReload shopInventory;
    [SerializeField] private GameObject tooExpensiveAlert;

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
            if (dragDropInQuestion.botPart.Price <= inventoryManager.money)
            {
                sfxPlayer.Buy();
                dragDropInQuestion.dropped = true;
                inventoryManager.AddToInventory(dragDropInQuestion.botPart);
                inventoryManager.money -= dragDropInQuestion.botPart.Price;
                Destroy(dragDropInQuestion.GetComponentInParent<ItemSlot>().gameObject);
                shopInventory.ResetInventory();
            }
            else
            {
                dragDropInQuestion.ResetDragDrop();
                tooExpensiveAlert.SetActive(true);
                Invoke("ClearAlert", 1f);
            }
        }
    }

    public void ClearAlert()
    {
        tooExpensiveAlert.SetActive(false);
    }
}