using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Scraps.Utilities;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    internal static InventoryManager Instance;
    internal event Action<int> MoneyChanged;

    [SerializeField] private GameObject inventoryItemPrefab;
    public GameObject inventoryParent;
    private DragDrop itemDragDrop;
    public List<RobotPart> itemParts;
    public int money;
    public int overallItemID = 0;

    public Robot myRobot;
    [SerializeField] private Robot defaultRobot;
    public bool IsFullyEquipped { get => myRobot.body != null && myRobot.head != null && myRobot.leftArm != null && myRobot.rightArm != null && myRobot.legs != null; }
    public bool isFirstTime = true;
    public bool canSell = true;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            tag = "InventoryParent";
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetUpInventory();
    }

    public void SetUpInventory()
    {
        money = 100;
        isFirstTime = true;
        myRobot = defaultRobot;
        myRobot = myRobot.Copy();
        AddToInventory(myRobot.head);
        AddToInventory(myRobot.body);
        AddToInventory(myRobot.leftArm);
        AddToInventory(myRobot.rightArm);
        AddToInventory(myRobot.legs);

        if(SetDefaultPartsUI.Instance != null)
            SetDefaultPartsUI.Instance.SetParts();

        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        inventoryParent.GetComponent<InventoryReload>().inventoryPopulated = true;
    }

    public void NewGame()
    {
        foreach (DragDrop dragDrop in SetDefaultPartsUI.Instance.GetComponentsInChildren<DragDrop>())
            dragDrop.ResetItemSlotDragDrop();
        inventoryParent.GetComponent<InventoryReload>().ResetInventory();
        overallItemID = 0;
        foreach(RobotPart r in itemParts)
            Debug.Log(r);
        itemParts.Clear();
        foreach (RobotPart r in itemParts)
            Debug.Log(r);
        inventoryParent.GetComponent<InventoryReload>().ClearInventory();
        SetUpInventory();
    }

    public void AddToInventory(RobotPart robotPart)
    {
        if (robotPart != null)
        {
            overallItemID++;
            if(inventoryParent != null/* && inventoryParent.GetComponentInParent<Shop>() == null*/)
                InstantiateInventoryItem(robotPart, inventoryParent);

            //Update player money
            MoneyChanged?.Invoke(money);

            robotPart.ItemID = overallItemID;
            itemParts.Add(robotPart);
        }
    }

    public void RemoveFromInventory(DragDrop dragDrop)
    {
        RobotPart robotPart = dragDrop.botPart;
        if (robotPart != null)
        {
            Debug.Log("Item Parts Count = " + itemParts.Count);
            //Make sure selling the part in question won't give you an unusuable bot
            if (itemParts.Count <= 5)
                canSell = false;
            else
            {
                Debug.Log("Current part ID = " + robotPart.ItemID);
                Debug.Log("myRobot head ID = " + myRobot.head.ItemID);
                Debug.Log("myRobot body ID = " + myRobot.body.ItemID);
                Debug.Log("myRobot larm ID = " + myRobot.leftArm.ItemID);
                Debug.Log("myRobot rarm ID = " + myRobot.rightArm.ItemID);
                Debug.Log("myRobot legs ID = " + myRobot.legs.ItemID);
                //Check if part being sold is equipped
                if ((robotPart.ItemID == myRobot.head.ItemID) ||
                   (robotPart.ItemID == myRobot.body.ItemID) ||
                   (robotPart.ItemID == myRobot.leftArm.ItemID) ||
                   (robotPart.ItemID == myRobot.rightArm.ItemID) ||
                   (robotPart.ItemID == myRobot.legs.ItemID))
                    canSell = false;

                //Check if selling part would remove any of that kind of part
                //Kind of redundant but just making sure
                int numHeads = 0;
                int numBodies = 0;
                int numArms = 0;
                int numLegs = 0;
                foreach (RobotPart r in itemParts)
                {
                    if (r is RobotPartHead)
                        numHeads++;
                    else if (r is RobotPartBody)
                        numBodies++;
                    else if (r is RobotPartArm)
                        numArms++;
                    else if (r is RobotPartLegs)
                        numLegs++;
                    else
                        Debug.Log("Uh oh!");
                }
                Debug.Log("heads = " + numHeads);
                Debug.Log("bodies = " + numBodies);
                Debug.Log("arms = " + numArms);
                Debug.Log("legs = " + numLegs);
                if ((numHeads <= 1 && robotPart is RobotPartHead) ||
                   (numBodies <= 1 && robotPart is RobotPartBody) ||
                   (numArms <= 2 && robotPart is RobotPartArm) ||
                   (numLegs <= 1 && robotPart is RobotPartLegs))
                    canSell = false;
            }

            //Sell if part elligble to be sold
            if (canSell)
            {
                foreach (RobotPart rp in itemParts)
                {
                    if (rp.ItemID == robotPart.ItemID)
                    {
                        itemParts.Remove(rp);
                        break;
                    }
                }
                if (inventoryParent != null /*&& inventoryParent.GetComponentInParent<Shop>() != null*/)
                {
                    foreach (ItemSlot itemSlot in inventoryParent.GetComponentsInChildren<ItemSlot>())
                    {
                        if (itemSlot.GetComponentInChildren<DragDrop>().botPart.ItemID == robotPart.ItemID)
                        {
                            Destroy(itemSlot.gameObject);
                        }
                    }
                }
                money += robotPart.Price;
                //Update player money
                MoneyChanged?.Invoke(money);
            }
            else
            {
                Debug.Log("Couldn't Sell");
                dragDrop.ResetDragDrop();
                canSell = true;
            }
        }
    }

    public void InstantiateInventoryItem(RobotPart robotPart, GameObject inventoryParentToReload)
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