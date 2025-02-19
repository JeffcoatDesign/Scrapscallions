using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Scraps.Parts;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
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

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        homeSlot = GetComponentInParent<ItemSlot>();
        itemImage = GetComponent<Image>();
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
                    GetComponentInParent<BotPartsEquip>().equippedHead = null;
                    break;
                case "Body":
                    GetComponentInParent<BotPartsEquip>().equippedBody = null;
                    break;
                case "Left Arm":
                    GetComponentInParent<BotPartsEquip>().equippedLArm = null;
                    break;
                case "Right Arm":
                    GetComponentInParent<BotPartsEquip>().equippedRArm = null;
                    break;
                case "Legs":
                    GetComponentInParent<BotPartsEquip>().equippedLegs = null;
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
