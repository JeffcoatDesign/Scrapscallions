using Scraps.AI.GOAP;
using Scraps.Cinematic;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scraps.Gameplay
{
    [RequireComponent(typeof(ScrapyardCollection))]
    public class ScrapyardManager : GameManager
    {
        public GoapAgent goapAgent;
        [SerializeField] private List<Transform> m_spawnPoints = new();

        public ScrapyardCollection collection;
        [SerializeField] private LootTable m_lootTable;
        [SerializeField] private float m_timeUntilScreen = 3f;
        [Header("UI")]
        [SerializeField] private GameObject m_playerIndicator;
        [SerializeField] private GameObject m_winScreen;
        [SerializeField] private GameObject m_loseScreen;
        private bool m_gameOver = false;
        private MusicPlayer m_musicPlayer;
        private int m_robotsDefeated = 0;
        public void KeepGoing()
        {
            m_gameOver = false;
            m_winScreen.SetActive(false);
            PostProcessingManager.Instance.HideVignette();

            opponentRobot = m_lootTable.GetRandomRobot(m_robotsDefeated, true);

            int index = UnityEngine.Random.Range(0, m_spawnPoints.Count);
            SpawnRobot(opponentRobot, m_spawnPoints[index], playerRobot, false);
            playerAgent.kinematic.EnableMovement();
            opponentAgent.kinematic.EnableMovement();
            EnableAI();
            Time.timeScale = 1f;
            m_battleUI.isTimerGoing = true;
            m_battleUI.timePassed = 60;

            playerRobot.State.target = opponentRobot.AgentObject;
        }
        private void OnEnable()
        {
            Instance = this;
            m_musicPlayer = MusicPlayer.Instance;

            if (InventoryManager.Instance == null)
            {
                SceneManager.LoadScene("UI");
                if (m_musicPlayer != null)
                    m_musicPlayer.MainMenu();
            }
        }

        private void Reset()
        {
            collection = GetComponent<ScrapyardCollection>();
        }
        private void Start()
        {
            playerRobot = InventoryManager.Instance.myRobot.Copy();
            opponentRobot = m_lootTable.GetRandomRobot(m_robotsDefeated, true);

            List<Transform> spawnPoints = m_spawnPoints;
            
            int index = UnityEngine.Random.Range(0, spawnPoints.Count);
            SpawnRobot(playerRobot, spawnPoints[index], opponentRobot, true);
            spawnPoints.RemoveAt(index);

            index = UnityEngine.Random.Range(0, spawnPoints.Count);
            SpawnRobot(opponentRobot, spawnPoints[index], playerRobot, false);

            CinematicManager.instance.SetCamera(CinematicManager.CameraType.Group);

            StartCoroutine(StartCountdown());
        }

        protected override void OnPlayerLost()
        {
            if (m_gameOver) return;

            m_gameOver = true;
            Debug.Log("The Heap Won!");

            Time.timeScale = 0.5f;
            Invoke(nameof(ShowLoseScreen), m_timeUntilScreen);

            PostProcessingManager.Instance.ShowVignette();

            opponentAgent.kinematic.DisableMovement();

            CinematicManager.instance.SetSingleTarget(opponentRobot.bodyController.transform);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.SingleTarget);
        }

        protected override void OnPlayerWon()
        {
            if (m_gameOver) return;
            m_gameOver = true;

            playerAgent.kinematic.DisableMovement();
            opponentRobot.agent.Died -= OnPlayerWon;
            collection.GetPartFromRobot(opponentRobot);

            m_robotsDefeated++;

            PostProcessingManager.Instance.ShowVignette();

            Time.timeScale = 0.5f;
            Invoke(nameof(ShowWinScreen), m_timeUntilScreen);

            m_battleUI.isTimerGoing = false;

            CinematicManager.instance.RemoveTarget(opponentRobot.AgentObject().transform);
            if (opponentRobot != null && opponentRobot.headController != null)
                CinematicManager.instance.RemoveTarget(opponentRobot.headController.transform.parent);
        }

        public void LeaveWithLoot()
        {
            Time.timeScale = 1f;
            collection.AddLoot();
            LoadMenu();
        }

        private void Update()
        {
            //Go back to menu if softlocked
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.L))
            {
                LoadMenu();
            }
        }

        private void ShowWinScreen()
        {
            m_winScreen.SetActive(true);
        }

        private void ShowLoseScreen()
        {
            m_loseScreen.SetActive(true);
        }
    }
}