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
            GameManager.Instance.AnnounceWinner += OnAnnounceWinner;
            playerHP.maxValue = InventoryManager.Instance.myRobot.head.MaxHP +
                                InventoryManager.Instance.myRobot.body.MaxHP +
                                InventoryManager.Instance.myRobot.leftArm.MaxHP +
                                InventoryManager.Instance.myRobot.rightArm.MaxHP +
                                InventoryManager.Instance.myRobot.legs.MaxHP;

            enemyHP.maxValue = GameManager.Instance.opponentRobot.head.MaxHP +
                               GameManager.Instance.opponentRobot.body.MaxHP +
                               GameManager.Instance.opponentRobot.leftArm.MaxHP +
                               GameManager.Instance.opponentRobot.rightArm.MaxHP +
                               GameManager.Instance.opponentRobot.legs.MaxHP;
        }

        void FixedUpdate()
        {
            if (isTimerGoing && isBattleOpen)
            {
                timePassed = timePassed - Time.deltaTime;
                timerText.text = timePassed.ToString("F0");
            }
            if (timePassed <= 0)
            {
                Debug.Log("Time Up!");
                isTimerGoing = false;
                OnAnnounceTie();
            }
        }

        public void DamagePlayer(int dmg)
        {
            Debug.Log("Damage Player");
            if (playerHP.value > 0)
                playerHP.value -= dmg;
            if (playerHP.value < 0)
                playerHP.value = 0;
        }

        public void DamageEnemy(int dmg)
        {
            Debug.Log("Damage Enemy");
            if (enemyHP.value > 0)
                enemyHP.value -= dmg;
            if (enemyHP.value < 0)
                enemyHP.value = 0;
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

            GameManager.Instance.m_playerAgent.kinematic.DisableMovement();
            GameManager.Instance.m_opponentAgent.kinematic.DisableMovement();

            GameManager.Instance.Invoke("LoadMenu", 3f);
        }
    }
}