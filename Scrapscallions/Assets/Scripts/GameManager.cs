using Scraps.AI.GOAP;
using Scraps.Utilities;
using Scraps.Cinematic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scraps.Gameplay;
using UnityEngine.SceneManagement;
using Scraps.UI;
using Unity.VisualScripting;
using TMPro;

namespace Scraps.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [HideInInspector] public Robot playerRobot;
        public Robot opponentRobot;
        public GoapAgent goapAgent;
        public Transform playerSpawnPoint;
        public Transform opponentSpawnPoint;
        public static event Action<GoapAgent> PlayerSpawned, OpponentSpawned;
        public event Action<string> AnnounceWinner;
        public GoapAgent m_playerAgent;
        public GoapAgent m_opponentAgent;
        [SerializeField] private LootTable m_lootTable;
        [SerializeField] private GameObject m_playerIndicator;
        [SerializeField] private int m_prizeMoney = 15;
        private MusicPlayer m_musicPlayer;

        public Animator countdownAnim;
        protected int countdownAmt = 3;
        public TextMeshProUGUI countdownText;

        private void OnEnable()
        {
            Instance = this;
            m_musicPlayer = MusicPlayer.Instance;
        }
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

            Countdown();
        }
        private void SpawnRobot(Robot robot, Transform spawnPoint, Robot target, bool isPlayer)
        {
            GoapAgent agent = Instantiate(goapAgent, spawnPoint.position, spawnPoint.rotation);
            robot.Spawn(agent, target, isPlayer);

            if (isPlayer)
            {
                agent.Died += OnPlayerLost;
                m_playerAgent = agent;
                PlayerSpawned?.Invoke(m_playerAgent);
                BattleUI.Instance.playerGOAP = m_playerAgent;
            }
            else
            {
                agent.Died += OnPlayerWon;
                m_opponentAgent = agent;
                OpponentSpawned?.Invoke(m_opponentAgent);
                BattleUI.Instance.enemyGOAP = m_opponentAgent;
            }


            CinematicManager.instance.AddTarget(agent.transform);
            CinematicManager.instance.AddTarget(robot.headController.transform.parent);

            if(isPlayer && robot.headController.tagTransform)
            {
                Instantiate(m_playerIndicator, robot.headController.tagTransform);
            }
        }

        void Countdown()
        {
            countdownAnim.Play("Countdown");
            CountdownTimer();
            Invoke("StartFight", 3.2f);
        }

        void CountdownTimer()
        {
            if (countdownAmt > 0)
                countdownText.text = countdownAmt.ToString();
            else
                countdownText.text = "GO!";
            countdownAmt--;
            if (countdownAmt >= 0)
                Invoke("CountdownTimer", 0.95f);
        }

        private void StartFight()
        {
            m_playerAgent.EnableAI();
            m_opponentAgent.EnableAI();
            BattleUI.Instance.isTimerGoing = true;
        }

        private void OnPlayerLost()
        {
            Debug.Log("Chassi Won!");
            AnnounceWinner?.Invoke("Chassi");

            Time.timeScale = 0.5f;

            m_opponentAgent.kinematic.DisableMovement();

            CinematicManager.instance.SetSingleTarget(opponentRobot.bodyController.transform);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.SingleTarget);

            Invoke("LoadMenu", 3f);
        }

        private void OnPlayerWon()
        {
            Debug.Log("Scrapscallions Won!");
            AnnounceWinner?.Invoke("The Scrapscallions");
            InventoryManager.Instance.money += m_prizeMoney;
            m_playerAgent.kinematic.DisableMovement();

            Time.timeScale = 0.5f;

            CinematicManager.instance.SetSingleTarget(playerRobot.bodyController.transform);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.SingleTarget);

            Invoke("LoadMenu", 3f);
        }

        private void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("UI");
            if(m_musicPlayer != null)
                m_musicPlayer.MainMenu();
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