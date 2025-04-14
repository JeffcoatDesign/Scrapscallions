using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Scraps.Parts;
using UnityEditor.Experimental.GraphView;

public class Shop : MonoBehaviour, IDropHandler
{
    //The DragDrop dropped into the ItemSlot
    private DragDrop dragDropInQuestion;
    public InventoryReload shopInventory;
    public GameObject tooExpensiveAlert;
    bool inInventory = false;
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

            foreach (RobotPart part in InventoryManager.Instance.itemParts)
            {
                if (part.ItemID == dragDropInQuestion.botPart.ItemID)
                {
                    inInventory = true;
                }
            }

            if(inInventory)
            {
                dragDropInQuestion.ResetDragDrop();
            }
            else if (dragDropInQuestion != null && dragDropInQuestion.botPart.Price <= InventoryManager.Instance.money)
            {
                sfxPlayer.Buy();
                dragDropInQuestion.dropped = true;
                Debug.Log("Buying " + dragDropInQuestion.botPart);

                InventoryManager.Instance.money -= dragDropInQuestion.botPart.Price;
                InventoryManager.Instance.AddToInventory(dragDropInQuestion.botPart);
                Destroy(dragDropInQuestion.GetComponentInParent<ItemSlot>().gameObject);
                shopInventory.ResetInventory();

                FirstTimeShop FTS = FindAnyObjectByType<FirstTimeShop>();
                if (FTS != null)
                    FTS.EndTutorial();
            }
            else
            {
                dragDropInQuestion.ResetDragDrop();
                tooExpensiveAlert.SetActive(true);
                Invoke("ClearAlert", 1f);
            }
            inInventory = false;
        }
    }

    public void ClearAlert()
    {
        tooExpensiveAlert.SetActive(false);
    }
}