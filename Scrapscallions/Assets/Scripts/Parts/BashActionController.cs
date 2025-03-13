using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class BashActionController : ActionController
    {
        [SerializeField] private AttackCollider m_attackCollider;
        [SerializeField] private float m_bashForce = 2f;
        private Robot m_robot;   
        public override bool IsTakingAction { get; set; } = false;
        public override bool IsReady { get { return !IsTakingAction && IsCooledDown; } set { } }
        public override bool IsCooledDown { get; set; } = true;
        [field: SerializeField] public override float ActionLength { get; set; } = 0.5f;
        [field: SerializeField] public override float CooldownTime { get; set; } = 1f;
        public override Action ActionCompleted { get; set; }
        [field: SerializeField] public override string ActionName { get; set; } = "Bash Attack";

        protected override void Awake()
        {
            base.Awake();

            m_robot = GetComponent<PartController>().GetRobot();
        }

        public override void Activate()
        {
            StartCoroutine(Bash());
        }

        IEnumerator Bash()
        {
            Vector3 direction = m_robot.State.Position - m_robot.State.target().transform.position;
            direction.Normalize();
            m_robot.agent.GetComponent<Rigidbody>().AddForce(direction * m_bashForce, ForceMode.Impulse);
            m_attackCollider.canHit = true;

            yield return new WaitForSeconds(ActionLength);

            m_attackCollider.canHit = false;
            StartCooldown();
            ActionCompleted?.Invoke();
        }
    }
}