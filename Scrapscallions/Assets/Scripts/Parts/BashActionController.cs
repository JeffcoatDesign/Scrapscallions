using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Parts;
using Unity.VisualScripting;
using UnityEngine;

namespace Scraps.Parts
{
    public class BashActionController : ActionController
    {
        [SerializeField] private AttackCollider m_attackCollider;
        [SerializeField] private float m_bashForce = 2f;
        private DetachOnBreak m_detachOnBreak;
        private PartController m_part;
        public override bool IsTakingAction { get; set; } = false;
        public override bool IsReady { get { return !IsTakingAction && IsCooledDown; } set { } }
        public override bool IsCooledDown { get; set; } = true;
        public override bool IsInitialized { get; set; } = false;
        [field: SerializeField] public override float ActionLength { get; set; } = 0.5f;
        [field: SerializeField] public override float CooldownTime { get; set; } = 1f;
        public override Action ActionCompleted { get; set; }
        [field: SerializeField] public override string ActionName { get; set; } = "Bash Attack";
        private bool m_isBashing = false;

        protected override void Awake()
        {
            base.Awake();
            m_detachOnBreak = GetComponent<DetachOnBreak>();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public override void Activate()
        {
            StartCoroutine(Bash());
        }

        IEnumerator Bash()
        {
            if (!m_isBashing)
            {
                Robot robot = null;
                if (m_part != null)
                    robot = m_part.GetRobot();
                if (robot != null)
                {
                    m_isBashing = true;
                    Vector3 direction = robot.State.target().transform.position - robot.State.Position;
                    robot.State.SetDestination(() => robot.State.Position);
                    direction.Normalize();
                    Rigidbody rb = null;
                    if (m_detachOnBreak.IsDetached)
                        rb = GetComponent<Rigidbody>();
                    else
                        rb = robot.agent.GetComponent<Rigidbody>();
                    rb.AddForce(-direction * m_bashForce / 2, ForceMode.Impulse);
                    yield return new WaitForSeconds(0.3f);
                    robot.State.SetDestination(() => robot.State.target().transform.position);
                    rb.AddForce(direction * m_bashForce, ForceMode.Impulse);
                    m_attackCollider.CanHit = true;

                    yield return new WaitForSeconds(ActionLength);
                    m_isBashing = true;
                    m_attackCollider.CanHit = false;

                    StartCooldown();
                    ActionCompleted?.Invoke();
                }
                else
                    Debug.Log("Robot is null");
            }
        }

        public override void Initialize(PartController part)
        {
            m_part = part;
            IsInitialized = true;
        }
    }
}