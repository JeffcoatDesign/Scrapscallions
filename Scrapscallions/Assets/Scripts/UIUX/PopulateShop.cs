using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateShop : MonoBehaviour
{
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private GameObject inventoryParent;
    private DragDrop itemDragDrop;
    [SerializeField] private RobotPart[] itemParts;
    private List<RobotPart> itemPartsChosen = new List<RobotPart>();
    private RobotPart chosenPart;
    [SerializeField] private InventoryManager inventoryManager;

    void OnEnable()
    {
        if(itemPartsChosen != null)
            itemPartsChosen.RemoveRange(0, itemPartsChosen.Count);
        itemPartsChosen.AddRange(itemParts);
        foreach (ItemSlot inventoryItem in GetComponentsInChildren<ItemSlot>())
            Destroy(inventoryItem.gameObject);
        for(int i = 0; i < 6; i++)
        {
            //Instantiates an item into the inventory
            GameObject myInventoryItem = Instantiate(inventoryItemPrefab);
            itemDragDrop = myInventoryItem.GetComponentInChildren<DragDrop>();
            myInventoryItem.transform.SetParent(inventoryParent.transform, false);
            itemDragDrop.canvas = GetComponentInParent<Canvas>();

            //Randomly creates a RobotPart from a list of RobotPart Prefabs formed in the inspector, and sets the DragDrop's tag accordingly
            chosenPart = itemPartsChosen[Random.Range(0, itemPartsChosen.Count)];
            if (chosenPart is RobotPartHead)
            {
                itemDragDrop.gameObject.tag = "Head";
            }
            else if (chosenPart is RobotPartBody)
            {
                itemDragDrop.gameObject.tag = "Body";
            }
            else if (chosenPart is RobotPartArm)
            {
                itemDragDrop.gameObject.tag = "Arm";
            }
            else if (chosenPart is RobotPartLegs)
            {
                itemDragDrop.gameObject.tag = "Leg";
            }

            //Set up the DragDrop's variables based on the created RobotPart
            itemDragDrop.botPart = chosenPart;
            itemDragDrop.gameObject.name = itemDragDrop.botPart.PartName;
            itemDragDrop.GetComponent<Image>().sprite = itemDragDrop.botPart.Sprite;
            itemPartsChosen.Remove(chosenPart);
            //Set layer to be Shop
            itemDragDrop.gameObject.layer = 6;
            itemDragDrop.homeSlot.gameObject.layer = 6;
        }
    }
}
