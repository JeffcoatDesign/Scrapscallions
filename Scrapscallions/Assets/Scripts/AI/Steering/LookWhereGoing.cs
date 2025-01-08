using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Look Where Going", menuName = "Steering/Look Where Going")]
    public class LookWhereGoing : Align
    {
        // override Align's getTargetAngle to look where we're going instead of matching our target's orientation
        public override float GetTargetAngle(GameObject target)
        {
            // check for a zero velocity and make no change if so
            Vector3 velocity = robotState.character.linearVelocity;
            if (velocity.magnitude == 0)
            {
                // return our current orientation
                return robotState.character.transform.eulerAngles.y;
            }

            // otherwise set the target angle based on our velocity
            float targetAngle = Mathf.Atan2(velocity.x, velocity.z);
            targetAngle *= Mathf.Rad2Deg;

            //Debug.Log(targetAngle);
            return targetAngle;
        }
    }
}