using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Scraps.Parts;
using TMPro;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Canvas canvas;
    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    public Image itemImage;
    public ItemSlot homeSlot;
    public Vector3 homePosition;
    public bool dropped;
    public bool draggable;
    public DragDrop dragDropOrigin;
    public ItemSlot slotOccupying;
    public RobotPart botPart;
    public GameObject toolTip;
    public TextMeshProUGUI[] itemDescription;
    private RobotPartArm botPartToArm;
    private RobotPartHead botPartToHead;
    private RectTransform toolTipPos;
    private InventoryManager inventoryManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        homeSlot = GetComponentInParent<ItemSlot>();
        itemImage = GetComponent<Image>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        if (toolTip != null)
        {
            itemDescription = toolTip.GetComponentsInChildren<TextMeshProUGUI>();
            toolTipPos = toolTip.GetComponent<RectTransform>();
        }
        draggable = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            //Debug.Log("OnBeginDrag");
            canvasGroup.alpha = .5f;
            canvasGroup.blocksRaycasts = false;
            itemImage.maskable = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(draggable)
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        if (!dropped)
            ResetDragDrop();
        else
            itemImage.maskable = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
        if (draggable)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Does nothing if DragDrop doesn't have a ToolTip
        if (toolTip == null)
            return;

        //Checks and sets the text field of the ToolTip according to the botPart type
        if (tag == "Arm")
        {
            botPartToArm = (RobotPartArm)botPart;
            itemDescription[3].gameObject.SetActive(true);
            itemDescription[4].gameObject.SetActive(true);
            itemDescription[3].text = "Attack Speed: " + botPartToArm.AttackSpeed;
            itemDescription[4].text = "Damage: " + botPartToArm.AttackDamage;
        }
        else if (tag == "Head")
        {
            botPartToHead = (RobotPartHead)botPart;
            itemDescription[3].gameObject.SetActive(true);
            itemDescription[3].text = "Quirks: "; //Need quirks done for quirk description
            itemDescription[4].gameObject.SetActive(false);
        }
        else
        {
            itemDescription[3].gameObject.SetActive(false);
            itemDescription[4].gameObject.SetActive(false);
        }
        itemDescription[0].text = tag + ": \"" + botPart.PartName + "\"";
        itemDescription[1].text = "Maximum HP: " + botPart.MaxHP.ToString();
        itemDescription[2].text = "Price: $" + botPart.Price.ToString();
        toolTip.SetActive(true);

        //Moves ToolTip position if it's in a position that it can go off screen
        if ((int)toolTipPos.position.x == 1848)
            toolTipPos.position = new Vector2(toolTipPos.position.x - 400, toolTipPos.position.y);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toolTip == null)
            return;
        toolTip.SetActive(false);
    }

    public void ResetDragDrop()
    {
        //Debug.Log("Reset " + gameObject.name);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        dropped = false;
        draggable = true;
        itemImage.maskable = true;
        homePosition = homeSlot.GetComponent<RectTransform>().position;
        GetComponent<RectTransform>().position = homePosition;
        if(GetComponentInParent<ItemSlot>().gameObject.layer != 3)
        {
            switch (GetComponentInParent<ItemSlot>().gameObject.name)
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
        }
    }

    //Specifically for resetting DragDrops that are children of the Equip Region Slots
    public void ResetItemSlotDragDrop()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        draggable = true;
        dropped = false;
        homePosition = homeSlot.GetComponent<RectTransform>().position;
        GetComponent<RectTransform>().position = homePosition;
        dragDropOrigin = null;
        gameObject.SetActive(false);
    }
}
