using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scraps.Parts
{
    public class BashActionController : ActionController
    {
        [SerializeField] private AttackCollider m_attackCollider;
        [SerializeField] private float m_bashForce = 2f;
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
            PartController part = GetComponent<PartController>();
            Robot robot = null;
            if (part != null)
                robot = part.GetRobot();
            if (robot != null)
            {
                Vector3 direction = robot.State.target().transform.position - robot.State.Position;
                direction.Normalize();
                robot.agent.GetComponent<Rigidbody>().AddForce(-direction * m_bashForce / 2, ForceMode.Impulse);
                yield return new WaitForSeconds(0.3f);
                robot.agent.GetComponent<Rigidbody>().AddForce(direction * m_bashForce, ForceMode.Impulse);
                m_attackCollider.canHit = true;

                yield return new WaitForSeconds(ActionLength);

                m_attackCollider.canHit = false;
                StartCooldown();
                ActionCompleted?.Invoke();
            }
            else
                Debug.Log("Robot is null");
        }
    }
}