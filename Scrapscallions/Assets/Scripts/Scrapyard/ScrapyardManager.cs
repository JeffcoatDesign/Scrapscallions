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
    public class ScrapyardManager : MonoBehaviour
    {
        public static ScrapyardManager Instance;
        public Robot playerRobot;
        [HideInInspector] public Robot opponentRobot;
        public GoapAgent goapAgent;
        public Transform playerSpawnPoint;
        public Transform opponentSpawnPoint;
        public ScrapyardCollection collection;
        private GoapAgent m_playerAgent;
        private GoapAgent m_opponentAgent;
        [SerializeField] private LootTable m_lootTable;
        [SerializeField] private GameObject m_playerIndicator;
        [SerializeField] private GameObject m_winScreen;
        [SerializeField] private GameObject m_loseScreen;
        private bool m_gameOver = false;
        private MusicPlayer musicPlayer;
        public void KeepGoing()
        {
            m_winScreen.SetActive(false);

            opponentRobot = m_lootTable.GetRandomRobot();
            SpawnRobot(opponentRobot, opponentSpawnPoint, playerRobot, false);
            m_playerAgent.kinematic.EnableMovement();

            playerRobot.State.target = opponentRobot.AgentObject;
        }
        private void OnEnable()
        {
            Instance = this;
            musicPlayer = MusicPlayer.Instance;

            if (InventoryManager.Instance == null)
            {
                SceneManager.LoadScene("UI");
                musicPlayer.MainMenu();
            }
        }

        private void Reset()
        {
            collection = GetComponent<ScrapyardCollection>();
        }
        private void Start()
        {
            playerRobot = InventoryManager.Instance.myRobot.Copy();
            opponentRobot = m_lootTable.GetRandomRobot(true);

            SpawnRobot(playerRobot, playerSpawnPoint, opponentRobot, true);
            SpawnRobot(opponentRobot, opponentSpawnPoint, playerRobot, false);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.Group);
        }
        private void SpawnRobot(Robot robot, Transform spawnPoint, Robot target, bool isPlayer)
        {
            GoapAgent agent = Instantiate(goapAgent, spawnPoint.position, spawnPoint.rotation);

            if (isPlayer)
            {
                agent.Died += OnPlayerLost;
                m_playerAgent = agent;
            }
            else
            {
                agent.Died += OnOpponentLost;
                m_opponentAgent = agent;
            }

            robot.Spawn(agent, target, isPlayer);

            CinematicManager.instance.AddTarget(agent.transform);
            CinematicManager.instance.AddTarget(robot.headController.transform.parent);

            if (isPlayer && robot.headController.tagTransform)
            {
                Instantiate(m_playerIndicator, robot.headController.tagTransform);
            }
        }

        private void OnPlayerLost()
        {
            if (m_gameOver) return;

            m_gameOver = true;
            Debug.Log("The Heap Won!");
            m_loseScreen.SetActive(true);

            m_opponentAgent.kinematic.DisableMovement();

            CinematicManager.instance.SetSingleTarget(opponentRobot.bodyController.transform);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.SingleTarget);
        }

        private void OnOpponentLost()
        {
            if (m_gameOver) return;

            m_playerAgent.kinematic.DisableMovement();
            opponentRobot.agent.Died -= OnOpponentLost;
            collection.GetPartFromRobot(opponentRobot);

            m_winScreen.SetActive(true);

            CinematicManager.instance.RemoveTarget(opponentRobot.AgentObject().transform);
            CinematicManager.instance.RemoveTarget(opponentRobot.headController.transform.parent);
        }

        public void LeaveWithLoot()
        {
            collection.AddLoot();
            LoadMenu();
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("UI");
            musicPlayer.MainMenu();
        }

        private void Update()
        {
            //Go back to menu if softlocked
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.L))
            {
                LoadMenu();
            }
        }
    }
}