using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Utilities
{
    public static class RobotExtensions
    {
        static internal Robot Copy(this Robot target)
        {
            Robot copy = Object.Instantiate(target);
            copy.leftArm = Object.Instantiate(target.leftArm);
            copy.rightArm = Object.Instantiate(target.rightArm);
            copy.legs = Object.Instantiate(target.legs);
            copy.body = Object.Instantiate(target.body);
            copy.head = Object.Instantiate(target.head);
            return copy;
        }
    }
}