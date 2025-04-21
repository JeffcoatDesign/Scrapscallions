using Scraps;
using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDefaultPartsUI : MonoBehaviour
{
    public static SetDefaultPartsUI Instance;
    public ItemSlot[] allItems;
    [SerializeField, Header("Equip Regions")] private ItemSlot headSlot;
    [SerializeField] private ItemSlot bodySlot;
    [SerializeField] private ItemSlot leftArmSlot;
    [SerializeField] private ItemSlot rightArmSlot;
    [SerializeField] private ItemSlot legsSlot;
    [SerializeField] private DisplayRobot m_displayRobot;

    public void Awake()
    {
        Instance = this;
    }

    public void SetParts()
    {
        allItems = InventoryManager.Instance.inventoryParent.GetComponentsInChildren<ItemSlot>();
        bool left = true;

        //m_displayRobot.gameObject.SetActive(true);

        foreach (ItemSlot IS in allItems)
        {
            DragDrop dd = IS.GetComponentInChildren<DragDrop>();
            if (dd.gameObject.layer != 12)
            {
                if (dd.botPart is RobotPartHead)
                    headSlot.ForceEquip(dd);
                else if (dd.botPart is RobotPartBody)
                    bodySlot.ForceEquip(dd);
                else if (dd.botPart is RobotPartArm)
                {
                    if (left)
                        leftArmSlot.ForceEquip(dd);
                    else 
                        rightArmSlot.ForceEquip(dd);
                    left = false;
                }
                else if (dd.botPart is RobotPartLegs)
                    legsSlot.ForceEquip(dd);
            }

            //m_displayRobot.Display(InventoryManager.Instance.myRobot);
        }
    }

    private void OnEnable()
    {
        m_displayRobot.gameObject.SetActive(true);
        m_displayRobot.Display(InventoryManager.Instance.myRobot);
    }

    private void OnDisable()
    {
        m_displayRobot.gameObject.SetActive(false);
    }
}