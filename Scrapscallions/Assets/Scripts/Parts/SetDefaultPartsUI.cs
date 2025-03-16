using Scraps;
using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDefaultPartsUI : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public DragDrop[] allItems;
    [SerializeField, Header("Equip Regions")] private ItemSlot headSlot;
    [SerializeField] private ItemSlot bodySlot;
    [SerializeField] private ItemSlot leftArmSlot;
    [SerializeField] private ItemSlot rightArmSlot;
    [SerializeField] private ItemSlot legsSlot;
    [SerializeField] private DisplayRobot m_displayRobot;
    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        allItems = FindObjectsOfType<DragDrop>();

        m_displayRobot.gameObject.SetActive(true);

        foreach(DragDrop dd in allItems)
        {
            if (dd.gameObject.layer != 12)
            {
                if (dd.botPart is RobotPartHead && dd.botPart.ItemID == inventoryManager.myRobot.head.ItemID)
                    headSlot.ForceEquip(dd);
                else if (dd.botPart is RobotPartBody && dd.botPart.ItemID == inventoryManager.myRobot.body.ItemID)
                    bodySlot.ForceEquip(dd);
                else if (dd.botPart is RobotPartArm)
                {
                    if (dd.botPart.ItemID == inventoryManager.myRobot.leftArm.ItemID)
                        leftArmSlot.ForceEquip(dd);
                    else if (dd.botPart.ItemID == inventoryManager.myRobot.rightArm.ItemID)
                        rightArmSlot.ForceEquip(dd);
                }
                else if (dd.botPart is RobotPartLegs)
                    legsSlot.ForceEquip(dd);
            }

            m_displayRobot.Display(InventoryManager.Instance.myRobot);
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
