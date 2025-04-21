using System.Collections;
using System.Collections.Generic;
using Scraps.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class FirstTimeCustomizingBot : MonoBehaviour
{
    public Robot previousBot;
    [SerializeField] Button close;
    private void OnEnable()
    {
        close.interactable = false;
        previousBot = InventoryManager.Instance.myRobot.Copy();
    }
    public void Update()
    {
        if (previousBot.head.ItemID != InventoryManager.Instance.myRobot.head.ItemID ||
            previousBot.body.ItemID != InventoryManager.Instance.myRobot.body.ItemID ||
            previousBot.rightArm.ItemID != InventoryManager.Instance.myRobot.rightArm.ItemID ||
            previousBot.leftArm.ItemID != InventoryManager.Instance.myRobot.leftArm.ItemID ||
            previousBot.legs.ItemID != InventoryManager.Instance.myRobot.legs.ItemID)
            EndTutorial();
    }
    public void EndTutorial()
    {
        GetComponentInParent<TutorialPopup>().Close();
        close.interactable = true;
        Destroy(this);
    }
}
