using Scraps.AI.GOAP;
using Scraps.Cinematic;
using Scraps.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scraps.Gameplay
{
    public abstract class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [HideInInspector] public Robot playerRobot;
        public Robot opponentRobot;
        public float timeUntilStart = 4f;
        [SerializeField] protected GoapAgent m_goapAgentPrefab;
        [SerializeField] protected GameObject m_playerIndicatorPrefab;
        [SerializeField] private float m_slowMoSpeed = 0.5f;
        internal GoapAgent playerAgent, opponentAgent;

        public static event Action<Robot> PlayerRobotSpawned, OpponentRobotSpawned;

        [SerializeField] protected Animator m_countdownAnimator;
        [SerializeField] protected TextMeshProUGUI m_countdownText;
        [SerializeField] protected BattleUI m_battleUI;

        protected abstract void OnPlayerWon();
        protected abstract void OnPlayerLost();

        private void Awake()
        {
            Instance = this;
        }

        protected void SpawnRobot(Robot robot, Transform spawnPoint, Robot target, bool isPlayer)
        {
            GoapAgent agent = Instantiate(m_goapAgentPrefab, spawnPoint.position, spawnPoint.rotation);
            robot.Spawn(agent, target, isPlayer);

            if (isPlayer)
            {
                agent.Died += OnPlayerLost;
                playerAgent = agent;
                OpponentRobotSpawned?.Invoke(robot);
            }
            else
            {
                agent.Died += OnPlayerWon;
                opponentAgent = agent;
                OpponentRobotSpawned?.Invoke(robot);
            }


            CinematicManager.instance.AddTarget(agent.transform);
            CinematicManager.instance.AddTarget(robot.headController.transform.parent);

            if (isPlayer && robot.headController.tagTransform)
            {
                Instantiate(m_playerIndicatorPrefab, robot.headController.tagTransform);
            }
        }

        protected void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("UI");
            if (MusicPlayer.Instance != null)
                MusicPlayer.Instance.MainMenu();
        }

        protected void SlowTime()
        {
            Time.timeScale = m_slowMoSpeed;
        }

        protected IEnumerator StartCountdown()
        {
            m_countdownAnimator.Play("Countdown");
            while (timeUntilStart > 0)
            {
                timeUntilStart -= Time.deltaTime;
                m_countdownText.text = timeUntilStart.ToString("F0");
                yield return new WaitForEndOfFrame();
            }
            m_battleUI.isTimerGoing = true;
            EnableAI();
        }

        protected void EnableAI()
        {
            playerAgent.EnableAI();
            opponentAgent.EnableAI();
        }
    }
}