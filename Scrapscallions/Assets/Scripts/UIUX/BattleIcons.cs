using Scraps.Gameplay;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleIcons : MonoBehaviour
{
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image enemyIcon;

    void Start()
    {
        playerIcon.sprite = ArenaManager.Instance.playerRobot.head.Sprite;
        enemyIcon.sprite = ArenaManager.Instance.opponentRobot.head.Sprite;
    }
}
