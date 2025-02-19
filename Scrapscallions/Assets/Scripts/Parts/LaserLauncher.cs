using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class LaserLauncher : MonoBehaviour
    {
        [SerializeField] private RangedArmController m_armController;
        [SerializeField] private ParticleSystem m_particleSystem;
        [SerializeField] private float m_maxRange;
        [SerializeField] private LayerMask m_layerMask;
        private bool m_wasFiring;

        private void OnEnable()
        {
            m_armController.Fire += OnFire;
        }

        private void OnDisable()
        {
            m_armController.Fire -= OnFire;
        }

        private void Update()
        {
            if (m_wasFiring)
            {
                m_wasFiring = false;
            } else
               m_particleSystem.Stop();
        }
        private void OnFire()
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
                    otherPart.Hit(m_armController.arm.AttackDamage);
                }
            }
            else
            {
                Vector3 missPoint = transform.position + transform.forward * m_maxRange;
                Debug.DrawLine(transform.position, missPoint, Color.white);
            }
            m_wasFiring = true;
            m_particleSystem.Play();
        }
    }
}