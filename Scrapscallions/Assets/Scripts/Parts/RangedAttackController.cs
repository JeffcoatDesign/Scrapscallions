using Scraps.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Scraps.Parts
{
    public class RangedAttackController : ActionController
    {
        [SerializeField] private RangedArmController m_rangedArmController;
        [SerializeField] private Sensor m_sensor;
        [SerializeField] private ParticleSystem m_particleSystem;
        [SerializeField] private float m_maxRange;
        [SerializeField] private LayerMask m_layerMask;
        private bool m_wasFiring;
        private ScrapsSFX m_sfx;

        public override bool IsTakingAction { get; set; } = false;
        public override bool IsReady { get; set; } = true;
        public override bool IsCooledDown { get; set; } = true;
        [field: SerializeField] public override float ActionLength { get; set; } = 1f;
        [field: SerializeField] public override float CooldownTime { get; set; } = 3f;
        public override Action ActionCompleted { get; set; }
        [field: SerializeField] public override string ActionName { get; set; } = "Ranged Attack";

        private void OnEnable()
        {
            m_sfx = GetComponent<ScrapsSFX>();
        }

        protected override void Update()
        {
            base.Update();

            if (m_wasFiring)
            {
                m_wasFiring = false;
            } else
               m_particleSystem.Stop();
        }

        public override void Activate()
        {
            if (!IsTakingAction)
                StartCoroutine(BlastAttack());
        }

        public void Fire()
        {
            Ray ray = new(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, m_maxRange, m_layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                Debug.Log("Hit object with laser: " + hit.collider.name);
                if (hit.collider.CompareTag("Robot"))
                {
                    Debug.Log("Hit robot with laser");
                    PartController otherPart = hit.collider.GetComponent<PartController>();
                    otherPart.Hit(m_rangedArmController.arm.AttackDamage);
                }
            }
            else
            {
                Vector3 missPoint = transform.position + transform.forward * m_maxRange;
                Debug.DrawLine(transform.position, missPoint, Color.white);
            }
            m_wasFiring = true;
        }

        private IEnumerator BlastAttack()
        {
            IsTakingAction = true;
            bool hasFired = false;
            m_particleSystem.Play();
            m_sfx.Play();

            float startTime = Time.time;
            Vector3 rotation = Vector3.zero;
            Quaternion startRotation = m_rangedArmController.transform.rotation;
            Vector3 directionToTarget = m_rangedArmController.transform.forward;
            if (m_sensor.TargetPosition != Vector3.zero)
                directionToTarget = (m_sensor.TargetPosition - m_rangedArmController.transform.position).normalized;
            Vector3 targetAngle = Quaternion.LookRotation(directionToTarget).eulerAngles;
            Quaternion endRotation = Quaternion.LookRotation(directionToTarget);
            while (Time.time - startTime < ActionLength)
            {
                float t = ((Time.time - startTime) * 2) / ActionLength;
                //rotation = new(
                //    Mathf.Lerp(rotation.x, targetAngle.x, t),
                //    Mathf.Lerp(rotation.y, targetAngle.y, t),
                //    Mathf.Lerp(rotation.z, targetAngle.z, t)
                //);

                Debug.DrawLine(m_rangedArmController.transform.position, m_rangedArmController.transform.position + directionToTarget, Color.yellow);

                m_rangedArmController.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
                if (t > 0.5f && !hasFired)
                {
                    hasFired = true;
                    Fire();
                }

                //m_rangedArmController.transform.localRotation = Quaternion.Euler(rotation);
                yield return new WaitForEndOfFrame();
            }

            m_rangedArmController.transform.localRotation = Quaternion.identity;
            //Action Finished
            ActionCompleted?.Invoke();

            IsTakingAction = false;
            IsReady = false;

            StartCooldown();
            yield return null;
        }
    }
}