using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private GameObject inventoryParent;
    private DragDrop itemDragDrop;
    [SerializeField] private RobotPart[] itemParts;
    private RobotPart chosenPart;
    public int money;
    public TextMeshProUGUI moneyLabel;

    void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        UpdateMoney();
    }

    public void AddToInventory()
    {
        //Instantiates an item into the inventory
        GameObject myInventoryItem = Instantiate(inventoryItemPrefab);
        itemDragDrop = myInventoryItem.GetComponentInChildren<DragDrop>();
        myInventoryItem.transform.SetParent(inventoryParent.transform, false);
        itemDragDrop.canvas = FindAnyObjectByType<Canvas>().GetComponent<Canvas>();

        //Randomly creates a RobotPart from a list of RobotPart Prefabs formed in the inspector, and sets the DragDrop's tag accordingly
        chosenPart = itemParts[Random.Range(0, 12)];
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
    }

    public void AddToInventory(RobotPart robotPart)
    {
        //Instantiates given item into the inventory
        GameObject myInventoryItem = Instantiate(inventoryItemPrefab);
        itemDragDrop = myInventoryItem.GetComponentInChildren<DragDrop>();
        myInventoryItem.transform.SetParent(inventoryParent.transform, false);
        itemDragDrop.canvas = FindAnyObjectByType<Canvas>().GetComponent<Canvas>();

        //Sets the DragDrop's tag according to the robotPart's type
        if (robotPart is RobotPartHead)
        {
            itemDragDrop.gameObject.tag = "Head";
        }
        else if (robotPart is RobotPartBody)
        {
            itemDragDrop.gameObject.tag = "Body";
        }
        else if (robotPart is RobotPartArm)
        {
            itemDragDrop.gameObject.tag = "Arm";
        }
        else if (robotPart is RobotPartLegs)
        {
            itemDragDrop.gameObject.tag = "Leg";
        }

        //Set up the DragDrop's variables based on the created RobotPart
        itemDragDrop.botPart = robotPart;
        itemDragDrop.gameObject.name = itemDragDrop.botPart.PartName;
        itemDragDrop.GetComponent<Image>().sprite = itemDragDrop.botPart.Sprite;

        //Update player money
        money -= itemDragDrop.botPart.Price;
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        moneyLabel.text = money.ToString();
    }

    public void GiveMoney(int moneyToAdd)
    {
        money += moneyToAdd;
    }
}
