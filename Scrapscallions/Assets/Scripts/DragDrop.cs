using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    public Image itemImage;
    public ItemSlot homeSlot;
    public Vector3 homePosition;
    public bool dropped;
    public bool draggable;
    public DragDrop dragDropOrigin;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        homeSlot = GetComponentInParent<ItemSlot>();
        itemImage = GetComponent<Image>();
        draggable = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (draggable)
        {
            Debug.Log("OnBeginDrag");
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
        Debug.Log("OnEndDrag");
        if (!dropped)
            ResetDragDrop();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        if (draggable)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void ResetDragDrop()
    {
        Debug.Log("Reset " + gameObject.name);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        draggable = true;
        homePosition = homeSlot.GetComponent<RectTransform>().position;
        GetComponent<RectTransform>().position = homePosition;
    }

    public void ResetItemSlotDragDrop()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        draggable = true;
        homePosition = homeSlot.GetComponent<RectTransform>().position;
        GetComponent<RectTransform>().position = homePosition;
        dragDropOrigin = null;
        gameObject.SetActive(false);
    }
}
