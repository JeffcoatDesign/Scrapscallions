using Scraps;
using Scraps.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scraps.AI.GOAP;

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

        private void Awake()
        {
            Instance = this;
            isTimerGoing = false;
        }

        private void Start()
        {
            ArenaManager.AnnounceWinner += OnAnnounceWinner;
            playerHP.maxValue = InventoryManager.Instance.myRobot.TotalMaxHP;

            enemyHP.maxValue = GameManager.Instance.opponentRobot.TotalMaxHP;
        }

        private void OnDestroy()
        {
            ArenaManager.AnnounceWinner -= OnAnnounceWinner;
        }
        void FixedUpdate()
        {
            if (isTimerGoing)
            {
                timePassed = timePassed - Time.deltaTime;
                timerText.text = timePassed.ToString("F0");
                playerHP.value = GameManager.Instance.playerRobot.TotalCurrentHP;
                enemyHP.value = GameManager.Instance.opponentRobot.TotalCurrentHP;
            }
            if (timePassed <= 0)
            {
                Debug.Log("Time Up!");
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