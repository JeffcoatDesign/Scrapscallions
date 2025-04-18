using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Scraps.Parts
{
    [RequireComponent(typeof(VisualEffect))]
    [RequireComponent(typeof(Collider))]
    public class FlamethrowerAttackController : ActionController
    {
        [SerializeField] private VisualEffect m_visualEffect;
        [SerializeField] private Collider m_attackCollider;
        [SerializeField] private ArmController m_armController;
        [SerializeField] private Sensor m_sensor;
        [SerializeField] private float m_fireTickTime = 1f;
        private AudioSource m_audioSource;
        private CountdownTimer m_fireTickTimer;
        private bool m_fireTickReady;

        public override bool IsTakingAction { get; set; } = false;
        public override bool IsReady { get; set; } = true;
        public override bool IsCooledDown { get; set; } = false;
        public override bool IsInitialized { get; set; } = false;
        [field: SerializeField] public override float ActionLength { get; set; } = 3f;
        [field: SerializeField] public override float CooldownTime { get; set; } = 4f;
        public override Action ActionCompleted { get; set; }
        public override string ActionName { get; set; } = "Flamethrower Attack";

        public override void Activate()
        {
            StartCoroutine(FlamethrowerAttack());
        }

        private void OnEnable()
        {
            m_visualEffect.Stop();
            m_attackCollider.enabled = false;
            m_audioSource = GetComponent<AudioSource>();

            m_fireTickTimer = new(m_fireTickTime);

            m_fireTickTimer.OnTimerStop += () =>
            {
                m_fireTickReady = true;
            };
        }

        private void Reset()
        {
            m_visualEffect = GetComponent<VisualEffect>();
            m_attackCollider = GetComponent<Collider>();
            transform.parent.TryGetComponent(out m_armController);
        }

        private IEnumerator FlamethrowerAttack()
        {
            IsTakingAction = true;

            m_audioSource.Play();
            m_visualEffect.Play();
            m_attackCollider.enabled = true;
            m_fireTickReady = true;
            float startTime = Time.time;
            Vector3 rotation = Vector3.zero;
            //Quaternion startRotation = m_armController.transform.localRotation;
            Vector3 directionToTarget = m_armController.transform.forward;

            while (Time.time - startTime < ActionLength)
            {
                //float t = (Time.time - startTime) / ActionLength;

                if (m_sensor.TargetPosition != Vector3.zero)
                {
                    directionToTarget = (m_sensor.TargetPosition - m_armController.transform.position).normalized;
                } else
                {
                    directionToTarget = m_armController.transform.parent.forward;
                }
                Quaternion endRotation = Quaternion.LookRotation(directionToTarget);

                Debug.DrawLine(m_armController.transform.position, m_armController.transform.position + directionToTarget, Color.yellow);

                m_armController.transform.localRotation = Quaternion.Lerp(m_armController.transform.localRotation, endRotation, 0.1f);

                //m_rangedArmController.transform.localRotation = Quaternion.Euler(rotation);
                yield return new WaitForEndOfFrame();
            }

            m_armController.transform.localRotation = Quaternion.identity;
            //Action Finished
            ActionCompleted?.Invoke();
            m_audioSource.Stop();
            m_attackCollider.enabled = false;
            m_visualEffect.Stop();
            IsTakingAction = false;
            IsReady = false;

            StartCooldown();
            yield return null;
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.TryGetComponent(out PartController part))
            {
                if (part.GetRobot() == m_armController.GetRobot()) return;

                if(m_fireTickReady)
                {
                    m_fireTickReady = false;

                    part.Hit(m_armController.arm.AttackDamage);

                    m_fireTickTimer.Start();
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            m_fireTickTimer.Tick(Time.deltaTime);
        }
    }
}