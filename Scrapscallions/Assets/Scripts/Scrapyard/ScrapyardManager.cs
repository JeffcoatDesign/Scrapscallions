using Scraps.AI.GOAP;
using Scraps.Cinematic;
using Scraps.UI;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private List<Transform> m_spawnPoints = new();

        public ScrapyardCollection collection;
        private GoapAgent m_playerAgent;
        private GoapAgent m_opponentAgent;
        [SerializeField] private LootTable m_lootTable;
        [SerializeField] private GameObject m_playerIndicator;
        [SerializeField] private GameObject m_winScreen;
        [SerializeField] private GameObject m_loseScreen;
        private bool m_gameOver = false;
        private MusicPlayer m_musicPlayer;

        public Animator countdownAnim;
        protected int countdownAmt = 3;
        public TextMeshProUGUI countdownText;

        public void KeepGoing()
        {
            m_winScreen.SetActive(false);

            opponentRobot = m_lootTable.GetRandomRobot();

            int index = UnityEngine.Random.Range(0, m_spawnPoints.Count);
            SpawnRobot(opponentRobot, m_spawnPoints[index], playerRobot, false);
            m_playerAgent.kinematic.EnableMovement();
            Time.timeScale = 1f;

            playerRobot.State.target = opponentRobot.AgentObject;

            m_playerAgent.DisableAI();
            m_opponentAgent.DisableAI();
            //BattleUI.Instance.isTimerGoing = false;
            //BattleUI.Instance.timePassed = 60;
            countdownAmt = 3;
            Countdown();
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
            opponentRobot = m_lootTable.GetRandomRobot(true);

            List<Transform> spawnPoints = m_spawnPoints;
            
            int index = UnityEngine.Random.Range(0, spawnPoints.Count);
            SpawnRobot(playerRobot, spawnPoints[index], opponentRobot, true);
            spawnPoints.RemoveAt(index);

            index = UnityEngine.Random.Range(0, spawnPoints.Count);
            SpawnRobot(opponentRobot, spawnPoints[index], playerRobot, false);

            CinematicManager.instance.SetCamera(CinematicManager.CameraType.Group);

            Countdown();
        }
        private void SpawnRobot(Robot robot, Transform spawnPoint, Robot target, bool isPlayer)
        {
            GoapAgent agent = Instantiate(goapAgent, spawnPoint.position, spawnPoint.rotation);

            if (isPlayer)
            {
                agent.Died += OnPlayerLost;
                m_playerAgent = agent;
                //BattleUI.Instance.playerGOAP = m_playerAgent;
            }
            else
            {
                agent.Died += OnOpponentLost;
                m_opponentAgent = agent;
                //BattleUI.Instance.enemyGOAP = m_opponentAgent;
            }

            robot.Spawn(agent, target, isPlayer);

            CinematicManager.instance.AddTarget(agent.transform);
            CinematicManager.instance.AddTarget(robot.headController.transform.parent);

            if (isPlayer && robot.headController.tagTransform)
            {
                Instantiate(m_playerIndicator, robot.headController.tagTransform);
            }
        }

        void Countdown()
        {
            countdownAnim.Play("Countdown");
            CountdownTimer();
            Invoke("StartFight", 3.2f);
            Invoke("CountdownIdle", 4f);
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
            //BattleUI.Instance.isTimerGoing = true;
        }

        private void CountdownIdle()
        {
            countdownAnim.Play("Idle");
        }

        private void OnPlayerLost()
        {
            if (m_gameOver) return;

            m_gameOver = true;
            Debug.Log("The Heap Won!");
            m_loseScreen.SetActive(true);
            Time.timeScale = 0.5f;

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

            Time.timeScale = 0.5f;

            m_winScreen.SetActive(true);

            CinematicManager.instance.RemoveTarget(opponentRobot.AgentObject().transform);
            CinematicManager.instance.RemoveTarget(opponentRobot.headController.transform.parent);
        }

        public void LeaveWithLoot()
        {
            Time.timeScale = 1f;
            collection.AddLoot();
            LoadMenu();
        }

        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("UI");
            m_musicPlayer.MainMenu();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.L))
            {
                LoadMenu();
            }
        }
    }
}