using Scraps;
using Scraps.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scraps.AI.GOAP;
using System;

namespace Scraps.UI
{
    public class BattleUI : MonoBehaviour
    {
        public static BattleUI Instance;
        public TextMeshProUGUI timerText;
        public float timePassed = 60;
        public bool isTimerGoing;
        public bool isBattleOpen = true;

        [SerializeField]
        private Slider playerHP, enemyHP;
        public GoapAgent playerGOAP;
        public GoapAgent enemyGOAP;


        [SerializeField] TextMeshProUGUI m_winnerText;
        [SerializeField] GameObject m_winnerBG;

        private void OnEnable()
        {
            ArenaManager.AnnounceWinner += OnAnnounceWinner;

            GameManager.PlayerRobotSpawned += OnPlayerRobotSpawned;
            GameManager.OpponentRobotSpawned += OnOpponentRobotSpawned;
        }
        private void OnDisable()
        {
            ArenaManager.AnnounceWinner -= OnAnnounceWinner;

            GameManager.PlayerRobotSpawned -= OnPlayerRobotSpawned;
            GameManager.OpponentRobotSpawned -= OnOpponentRobotSpawned;
        }

        private void OnOpponentRobotSpawned(Robot robot)
        {
            enemyHP.maxValue = robot.TotalMaxHP;
            enemyHP.value = robot.TotalCurrentHP;
        }

        private void OnPlayerRobotSpawned(Robot robot)
        {
            playerHP.maxValue = robot.TotalMaxHP;
            playerHP.value = robot.TotalCurrentHP;
        }

        private void Awake()
        {
            Instance = this;
            isTimerGoing = false;
        }

        private void Start()
        {
            playerHP.maxValue = InventoryManager.Instance.myRobot.TotalMaxHP;
            playerHP.value = InventoryManager.Instance.myRobot.TotalCurrentHP;

            enemyHP.maxValue = GameManager.Instance.opponentRobot.TotalMaxHP;
            enemyHP.value = GameManager.Instance.opponentRobot.TotalCurrentHP;
        }

        void FixedUpdate()
        {
            if (isTimerGoing)
            {
                timePassed = timePassed - Time.deltaTime;
                timerText.text = timePassed.ToString("F0");
                if (GameManager.Instance.playerRobot.State.isAlive)
                    playerHP.value = GameManager.Instance.playerRobot.TotalCurrentHP;
                else
                    playerHP.value = 0;
                if (GameManager.Instance.opponentRobot.State.isAlive)
                    enemyHP.value = GameManager.Instance.opponentRobot.TotalCurrentHP;
                else
                    enemyHP.value = 0;
            }
            if (timePassed <= 0)
            {
                isTimerGoing = false;
                OnAnnounceTie();
            }
        }

        private void OnAnnounceWinner(string winner)
        {
            isTimerGoing = false;
            m_winnerText.text = $"{winner} Won!";
            m_winnerBG.SetActive(true);
        }

        private void OnAnnounceTie()
        {
            m_winnerText.text = $"Time Up!";
            m_winnerBG.SetActive(true);

            GameManager.Instance.playerAgent.kinematic.DisableMovement();
            GameManager.Instance.opponentAgent.kinematic.DisableMovement();

            GameManager.Instance.Invoke("LoadMenu", 3f);
        }
    }
}