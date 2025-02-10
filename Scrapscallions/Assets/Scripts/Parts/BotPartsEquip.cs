using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPartsEquip : MonoBehaviour
{
    [SerializeField] private ItemSlot headEquipSlot;
    [SerializeField] private ItemSlot bodyEquipSlot;
    [SerializeField] private ItemSlot lArmEquipSlot;
    [SerializeField] private ItemSlot rArmEquipSlot;
    [SerializeField] private ItemSlot legsEquipSlot;

    public RobotPart equippedHead;
    public RobotPart equippedBody;
    public RobotPart equippedLArm;
    public RobotPart equippedRArm;
    public RobotPart equippedLegs;

    public void BuildBot()
    {
        equippedHead = headEquipSlot.itemOccupiedBy.botPart;
        equippedBody = bodyEquipSlot.itemOccupiedBy.botPart;
        equippedLArm = lArmEquipSlot.itemOccupiedBy.botPart;
        equippedRArm = rArmEquipSlot.itemOccupiedBy.botPart;
        equippedLegs = legsEquipSlot.itemOccupiedBy.botPart;
    }
}
