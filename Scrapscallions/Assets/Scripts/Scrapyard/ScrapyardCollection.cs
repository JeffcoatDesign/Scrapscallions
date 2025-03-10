using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapyardCollection : MonoBehaviour
{
    public List<RobotPart> collectedParts = new();

    internal void AddLoot()
    {
        if (collectedParts.Count < 1) return;
        foreach (RobotPart part in collectedParts)
        {
            part.CurrentHP = part.MaxHP;
            InventoryManager.Instance.AddToInventory(part);
        }
    }

    public void GetPartFromRobot(Robot robot)
    {
        int index = Random.Range(1, 6);
        RobotPart part;
        switch (index)
        {
            case 1:
                part = Instantiate(robot.head);
                break;
            case 2:
                part = Instantiate(robot.leftArm);
                break;
            case 3:
                part = Instantiate(robot.rightArm);
                break;
            case 4:
                part = Instantiate(robot.legs);
                break;
            case 5:
                part = Instantiate(robot.body);
                break;
            default: return;
        }
        collectedParts.Add(part);
    }
}
