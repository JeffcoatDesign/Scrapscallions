using Scraps.AI.GOAP;
using Scraps.Utilities;
using Scraps.Cinematic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Scraps.Parts;
using Scraps.UI;

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
        [SerializeField] private float m_timeUntilWinner = 3f;
        [SerializeField] private List<RobotPart> m_prizeParts;
        [SerializeField] private Transform m_prizeParent;
        [SerializeField] private CollectionItem m_prizePrefab;
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

            SlowTime();

            opponentAgent.kinematic.DisableMovement();
            PostProcessingManager.Instance.ShowVignette();

            CinematicManager.instance.SetSingleTarget(opponentRobot.bodyController.transform);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.SingleTarget);

            Invoke(nameof(ShowPlayerLost), m_timeUntilWinner);
        }

        protected override void OnPlayerWon()
        {
            Debug.Log("Scrapscallions Won!");
            InventoryManager.Instance.money += m_prizeMoney;
            PostProcessingManager.Instance.ShowVignette();
            playerAgent.kinematic.DisableMovement();

            SlowTime();

            CinematicManager.instance.SetSingleTarget(playerRobot.bodyController.transform);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.SingleTarget);

            Invoke(nameof(ShowPlayerWon), m_timeUntilWinner);
        }

        private void Update()
        {
            //Go back to menu if softlocked
            if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.L))
            { 
                LoadMenu();
            }
        }

        private void ShowPlayerWon()
        {
            AnnounceWinner?.Invoke("The Scrapscallions");
            Invoke(nameof(LoadMenu), 3f);
            m_prizeParent.gameObject.SetActive(true);
            int randomIndex = UnityEngine.Random.Range(0, m_prizeParts.Count);
            RobotPart randomPart = Instantiate(m_prizeParts[randomIndex]);
            CollectionItem itemUI = Instantiate(m_prizePrefab, m_prizeParent);
            itemUI.SetPart(randomPart);
            InventoryManager.Instance.AddToInventory(randomPart);
        }

        private void ShowPlayerLost()
        {
            AnnounceWinner?.Invoke("Chassi");
            Invoke(nameof(LoadMenu), 3f);
            m_prizeParent.gameObject.SetActive(false);
        }
    }
}