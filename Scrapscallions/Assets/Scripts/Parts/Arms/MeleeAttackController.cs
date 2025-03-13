using Scraps.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class MeleeAttackController : ActionController
    {
        public override bool IsTakingAction { get; set; } = false;
        public override bool IsReady { get => !IsTakingAction && IsCooledDown; set { } }
        public override bool IsCooledDown { get; set; } = true;
        [field: SerializeField] public override float ActionLength { get; set; } = 2f;
        public override float CooldownTime { get; set; } = 3f;
        public override Action ActionCompleted { get; set; }
        [field: SerializeField] public override string ActionName { get; set; } = "Melee Attack";

        [SerializeField] AttackCollider m_attackCollider;
        [SerializeField] Sensor m_meleeSensor;
        [SerializeField] Vector3 m_attackRotation;
        [SerializeField] float m_peakSwing = 0.5f;
        private ScrapsSFX m_sfx;

        private void OnEnable()
        {
            m_sfx = GetComponent<ScrapsSFX>();
        }

        public override void Activate()
        {
            StartCoroutine(MeleeAttack());
        }

        private IEnumerator MeleeAttack()
        {
            IsTakingAction = true;

            if (m_sfx != null)
                m_sfx.Play();
            m_attackCollider.canHit = true;
            float startTime = Time.time;
            Vector3 rotation = Vector3.zero;
            Vector3 targetForward = m_meleeSensor.TargetPosition - transform.position;
            targetForward *= 0.2f;
            while (Time.time - startTime < ActionLength)
            {
                float t = (Time.time - startTime) / ActionLength;
                if (t <= m_peakSwing)
                {
                    rotation = new(
                        Mathf.Lerp(rotation.x, m_attackRotation.x + targetForward.x, t / m_peakSwing),
                        Mathf.Lerp(rotation.y, m_attackRotation.y + targetForward.y, t / m_peakSwing),
                        Mathf.Lerp(rotation.z, m_attackRotation.z + targetForward.z, t / m_peakSwing)
                        );
                }else
                {
                    rotation = new(
                        Mathf.Lerp(rotation.x, 0, (t - m_peakSwing) / (1 - m_peakSwing)),
                        Mathf.Lerp(rotation.y, 0, (t - m_peakSwing) / (1 - m_peakSwing)),
                        Mathf.Lerp(rotation.z, 0, (t - m_peakSwing) / (1 - m_peakSwing))
                        );
                }


                transform.localRotation = Quaternion.Euler(rotation);
                yield return new WaitForEndOfFrame();
            }
            //Debug.Log("Melee Attack Finished");

            transform.localRotation = Quaternion.identity;
            //Action Finished
            ActionCompleted?.Invoke();

            m_attackCollider.canHit = false;
            IsTakingAction = false;

            StartCooldown();
            yield return null;
        }
    }
}