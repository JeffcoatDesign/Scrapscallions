using Scraps.AI.GOAP;
using Scraps.Utilities;
using Scraps.Cinematic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scraps.Gameplay;

namespace Scraps.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Robot playerRobot;
        public Robot opponentRobot;
        public GoapAgent goapAgent;
        public Transform playerSpawnPoint;
        public Transform opponentSpawnPoint;
        public event Action<GoapAgent> PlayerSpawned, OpponentSpawned;
        public event Action<string> AnnounceWinner;
        private GoapAgent m_playerAgent;
        private GoapAgent m_opponentAgent;
        [SerializeField] private LootTable m_lootTable;
        [SerializeField] private GameObject m_playerIndicator;
        private void OnEnable()
        {
            Instance = this;
        }
        private void Start()
        {
            playerRobot = m_lootTable.GetRandomRobot();
            opponentRobot = m_lootTable.GetRandomRobot();
            SpawnRobot(playerRobot, playerSpawnPoint, opponentRobot, true);
            SpawnRobot(opponentRobot, opponentSpawnPoint, playerRobot, false);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.Group);
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
            }
            else
            {
                agent.Died += OnPlayerWon;
                m_opponentAgent = agent;
                OpponentSpawned?.Invoke(m_opponentAgent);
            }


            CinematicManager.instance.AddTarget(agent.transform);
            CinematicManager.instance.AddTarget(robot.headController.transform.parent);

            if(isPlayer && robot.headController.tagTransform)
            {
                Instantiate(m_playerIndicator, robot.headController.tagTransform);
            }
        }

        private void OnPlayerLost()
        {
            Debug.Log("Chassi Won!");
            AnnounceWinner?.Invoke("Chassi");

            m_opponentAgent.kinematic.DisableMovement();

            CinematicManager.instance.SetSingleTarget(opponentRobot.bodyController.transform);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.SingleTarget);
        }

        private void OnPlayerWon()
        {
            Debug.Log("Scrapscallions Won!");
            AnnounceWinner?.Invoke("The Scrapscallions");

            m_playerAgent.kinematic.DisableMovement();

            CinematicManager.instance.SetSingleTarget(playerRobot.bodyController.transform);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.SingleTarget);
        }
    }
}