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
            if (dragDropInQuestion != null && dragDropInQuestion.botPart.Price <= InventoryManager.Instance.money)
            {
                sfxPlayer.Buy();
                dragDropInQuestion.dropped = true;
                Debug.Log("Buying " + dragDropInQuestion.botPart);

                InventoryManager.Instance.money -= dragDropInQuestion.botPart.Price;
                InventoryManager.Instance.AddToInventory(dragDropInQuestion.botPart);
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