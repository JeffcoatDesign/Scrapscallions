using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryItemPrefab;
    private DragDrop itemDragDrop;
    private string[] possibleTags = {"Head", "Body", "Arm", "Leg"};
    private string[] possibleHeads = {"SC4r", "Bowling", "Toaster"};
    private string[] possibleBodies = {"Oil", "Bear", "Box"};
    private string[] possibleArms = {"Saw", "Blaster", "Flamethrower"};
    private string[] possibleLegs = {"Reegular", "Hover", "Roomba"};
    [SerializeField] private Sprite[] itemSprites;
    [SerializeField] private RobotPart[] itemParts;
    private Sprite chosenSprite;
    private string chosenName;
    private int randNumUse;
    private int randNumSpriteUse;

    void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    public void AddToInventory()
    {
        GameObject myInventoryItem = Instantiate(inventoryItemPrefab);
        itemDragDrop = myInventoryItem.GetComponentInChildren<DragDrop>();
        itemDragDrop.canvas = GetComponentInParent<Canvas>();
        myInventoryItem.transform.SetParent(transform, false);
        randNumUse = Random.Range(0, 4);
        randNumSpriteUse = Random.Range(0, 3);
        itemDragDrop.gameObject.tag = possibleTags[randNumUse];
        switch(randNumUse)
        {
            case 0:
                chosenName = possibleHeads[randNumSpriteUse];
                chosenSprite = itemSprites[randNumSpriteUse];
                itemDragDrop.botPart = new RobotPartHead();
                break;
            case 1:
                chosenName = possibleBodies[randNumSpriteUse];
                chosenSprite = itemSprites[randNumSpriteUse + 3];
                itemDragDrop.botPart = new RobotPartBody();
                break;
            case 2:
                chosenName = possibleArms[randNumSpriteUse];
                chosenSprite = itemSprites[randNumSpriteUse + 6];
                itemDragDrop.botPart = new RobotPartArm();
                break;
            case 3:
                chosenName = possibleLegs[randNumSpriteUse];
                chosenSprite = itemSprites[randNumSpriteUse + 9];
                itemDragDrop.botPart = new RobotPartLegs();
                break;
        }

        itemDragDrop.gameObject.name = chosenName;
        itemDragDrop.botPart.PartName = chosenName;
        itemDragDrop.GetComponent<Image>().sprite = chosenSprite;
        itemDragDrop.botPart.Sprite = chosenSprite;
    }
}
