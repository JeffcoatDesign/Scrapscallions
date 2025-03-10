using Scraps.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Testing
{
    public class ThrowRockButton : MonoBehaviour
    {
        [SerializeField] GameObject m_rockPrefab;
        [SerializeField] private float throwForce = 20f;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Vector3 startPos = Camera.main.transform.position;
                Vector3 targetPos = Vector3.zero;
                if (ScrapyardManager.Instance != null)
                {
                    targetPos = ScrapyardManager.Instance.playerRobot.agent.transform.position.With(y:1);
                } else if (GameManager.Instance != null)
                {
                    targetPos = GameManager.Instance.playerRobot.agent.transform.position;
                }
                Vector3 look = (targetPos - startPos).normalized;

                Instantiate(m_rockPrefab, startPos, Quaternion.LookRotation(look)).GetComponent<Rigidbody>().AddForce(look * throwForce, ForceMode.Impulse);
            }
        }
    }
}