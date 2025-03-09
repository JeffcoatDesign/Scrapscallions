using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Scraps.Utilities;

public class InventoryManager : MonoBehaviour
{
    internal static InventoryManager Instance;
    internal event Action<int> MoneyChanged;
    internal event Action InventoryChanged;

    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private GameObject inventoryParent;
    private DragDrop itemDragDrop;
    public List<RobotPart> itemParts;
    private RobotPart chosenPart;
    public int money;
    public int overallItemID = 0;

    public Robot myRobot;
    public bool IsFullyEquipped { get => myRobot.body != null && myRobot.head != null && myRobot.leftArm != null && myRobot.rightArm != null && myRobot.legs != null; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        //myRobot = myRobot.Copy();
        AddToInventory(myRobot.head);
        AddToInventory(myRobot.body);
        AddToInventory(myRobot.leftArm);
        AddToInventory(myRobot.rightArm);
        AddToInventory(myRobot.legs);

        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
    }

    public void AddToInventory(RobotPart robotPart)
    {
        if (robotPart != null)
        {
            overallItemID++;
            if (inventoryParent != null)
            {
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
                robotPart.ItemID = overallItemID;
                itemDragDrop.botPart = robotPart;
                itemDragDrop.gameObject.name = itemDragDrop.botPart.PartName;
                itemDragDrop.GetComponent<Image>().sprite = itemDragDrop.botPart.Sprite;

                //Update player money
                MoneyChanged?.Invoke(money);
            }
            itemParts.Add(robotPart);
        }
    }

    public void ReloadToInventory(RobotPart robotPart, GameObject inventoryParentToReload)
    {
        if (inventoryParentToReload != null)
        {
            //Instantiates given item into the inventory
            GameObject myInventoryItem = Instantiate(inventoryItemPrefab);
            itemDragDrop = myInventoryItem.GetComponentInChildren<DragDrop>();
            myInventoryItem.transform.SetParent(inventoryParentToReload.transform, false);
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
        }
    }

    public void GiveMoney(int moneyToAdd)
    {
        money += moneyToAdd;
        MoneyChanged?.Invoke(money);
    }
}
