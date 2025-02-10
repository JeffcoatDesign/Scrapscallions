using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryItemPrefab;
    private DragDrop itemDragDrop;
    private string[] possibleTags = { "Head", "Body", "Arm", "Leg" };
    void Awake()
    {
        GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y - 175, GetComponent<RectTransform>().position.z);
    }

    public void AddToInventory()
    {
        GameObject myInventoryItem = Instantiate(inventoryItemPrefab);
        itemDragDrop = myInventoryItem.GetComponent<DragDrop>();
        myInventoryItem.transform.SetParent(transform, false);
        itemDragDrop.tag = possibleTags[Random.Range(0, 3)];
    }
}
