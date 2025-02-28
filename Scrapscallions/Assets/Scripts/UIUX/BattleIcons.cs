using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleIcons : MonoBehaviour
{
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image enemyIcon;
    public InventoryManager inventoryManager;

    void OnEnable()
    {
        Debug.Log(playerIcon);
        Debug.Log(inventoryManager);
        playerIcon.sprite = inventoryManager.equippedHead.Sprite;
    }
}
