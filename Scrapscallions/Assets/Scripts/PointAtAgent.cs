using Scraps.AI.GOAP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Gameplay
{
    public class PointAtAgent : MonoBehaviour
    {
        [SerializeField] private float m_rotateSpeed = 0.05f;
        GoapAgent m_playerAgent;
        GoapAgent m_opponentAgent;
        private void Awake()
        {
            GameManager.OpponentSpawned += OnOpponentSpawned;
            GameManager.PlayerSpawned += OnPlayerSpawned;
        }

        private void OnDisable()
        {
            GameManager.OpponentSpawned -= OnOpponentSpawned;
            GameManager.PlayerSpawned -= OnPlayerSpawned;
        }

        private void OnPlayerSpawned(GoapAgent agent)
        {
            m_playerAgent = agent;
        }

        private void OnOpponentSpawned(GoapAgent agent)
        {
            m_opponentAgent = agent;
        }

        private void Update()
        {
            if (m_playerAgent == null || m_opponentAgent == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, m_playerAgent.transform.position);
            float distanceToOpponent = Vector3.Distance(transform.position, m_opponentAgent.transform.position);
            Vector3 targetPosition;
            if (distanceToPlayer < distanceToOpponent)
                targetPosition = m_playerAgent.transform.position;
            else
                targetPosition = m_opponentAgent.transform.position;

            Vector3 forward = (targetPosition - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + forward * 100, Color.magenta);

            Quaternion targetRotation = Quaternion.LookRotation(forward); 

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, m_rotateSpeed * Time.deltaTime);
        }
    }
}