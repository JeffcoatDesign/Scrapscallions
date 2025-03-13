using Scraps.AI.GOAP;
using Scraps.Cinematic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scraps.Gameplay
{
    public abstract class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [HideInInspector] public Robot playerRobot;
        public Robot opponentRobot;
        public float timeUntilStart = 3f;
        [SerializeField] protected GoapAgent m_goapAgentPrefab;
        [SerializeField] protected GameObject m_playerIndicatorPrefab;
        [SerializeField] private float m_slowMoSpeed = 0.5f;
        protected private GoapAgent m_playerAgent, m_opponentAgent;

        public static event Action<GoapAgent> PlayerSpawned, OpponentSpawned;

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
                m_playerAgent = agent;
                PlayerSpawned?.Invoke(m_playerAgent);
            }
            else
            {
                agent.Died += OnPlayerWon;
                m_opponentAgent = agent;
                OpponentSpawned?.Invoke(m_opponentAgent);
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
            while (timeUntilStart > 0)
            {
                timeUntilStart -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            EnableAI();
        }

        private void EnableAI()
        {
            m_playerAgent.EnableAI();
            m_opponentAgent.EnableAI();
        }
    }
}