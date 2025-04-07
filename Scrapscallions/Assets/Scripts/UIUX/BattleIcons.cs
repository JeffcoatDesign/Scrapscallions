using Scraps.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleIcons : MonoBehaviour
{
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image enemyIcon;

    private void Awake()
    {
        GameManager.PlayerRobotSpawned += OnPlayerSpawned;
        GameManager.OpponentRobotSpawned += OnOpponentSpawned;
    }

    private void OnDisable()
    {
        GameManager.PlayerRobotSpawned -= OnPlayerSpawned;
        GameManager.OpponentRobotSpawned -= OnOpponentSpawned;
    }

    private void OnOpponentSpawned(Robot robot)
    {
        enemyIcon.sprite = robot.head.Sprite;
    }

    private void OnPlayerSpawned(Robot robot)
    {
        playerIcon.sprite = robot.head.Sprite;
    }

    void Start()
    {
        playerIcon.sprite = ArenaManager.Instance.playerRobot.head.Sprite;
        enemyIcon.sprite = ArenaManager.Instance.opponentRobot.head.Sprite;
    }
}
