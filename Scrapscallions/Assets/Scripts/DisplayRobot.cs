using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps
{
    public class DisplayRobot : MonoBehaviour
    {
        GameObject m_legs;
        public void Display(Robot robot)
        {
            if (m_legs != null) Destroy(m_legs);

            var legsController = robot.legs.Spawn(transform);
            var bodyController = robot.body.Spawn(legsController);
            var headController = robot.head.Spawn(bodyController);
            var leftArmController = robot.leftArm.Spawn(bodyController, false);
            var rightArmController = robot.rightArm.Spawn(bodyController, true);

            m_legs = legsController.gameObject;
        }

        private void OnDisable()
        {
            Destroy(m_legs);
        }
    }
}