using Scraps.AI.GOAP;
using Scraps.Utilities;
using Scraps.Cinematic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace Scraps.Gameplay
{
    public class ArenaManager : GameManager
    {
        public Transform playerSpawnPoint;
        public Transform opponentSpawnPoint;
        public static event Action<string> AnnounceWinner;
        [SerializeField] private LootTable m_lootTable;
        [SerializeField] private GameObject m_playerIndicator;
        [SerializeField] private int m_prizeMoney = 15;

        private void Start()
        {
            if (InventoryManager.Instance == null)
            {
                LoadMenu();
                return;
            }

            //Copy the robots
            playerRobot = InventoryManager.Instance.myRobot.Copy();
            opponentRobot = opponentRobot.Copy();

            SpawnRobot(playerRobot, playerSpawnPoint, opponentRobot, true);
            SpawnRobot(opponentRobot, opponentSpawnPoint, playerRobot, false);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.Group);

            StartCoroutine(StartCountdown());
        }

        protected override void OnPlayerLost()
        {
            Debug.Log("Chassi Won!");
            AnnounceWinner?.Invoke("Chassi");

            SlowTime();

            m_opponentAgent.kinematic.DisableMovement();

            CinematicManager.instance.SetSingleTarget(opponentRobot.bodyController.transform);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.SingleTarget);

            Invoke(nameof(LoadMenu), 3f);
        }

        protected override void OnPlayerWon()
        {
            Debug.Log("Scrapscallions Won!");
            AnnounceWinner?.Invoke("The Scrapscallions");
            InventoryManager.Instance.money += m_prizeMoney;
            m_playerAgent.kinematic.DisableMovement();

            SlowTime();

            CinematicManager.instance.SetSingleTarget(playerRobot.bodyController.transform);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.SingleTarget);

            Invoke(nameof(LoadMenu), 3f);
        }

        private void Update()
        {
            //Go back to menu if softlocked
            if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.L))
            { 
                LoadMenu();
            }
        }
    }
}