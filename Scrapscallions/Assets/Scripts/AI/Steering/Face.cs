using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Face", menuName = "Steering/Face")]
    public class Face : Align
    {
        public override float GetTargetAngle(GameObject target)
        {
            Vector3 direction = target.transform.position - robotState.character.transform.position;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            return targetAngle;
        }
    }
}