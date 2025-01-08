using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    public class ActionObject : MonoBehaviour
    {
        public Action myAction = new Action("Default Action")
        {
            effectedGoals = new List<Goal>() {
            new Goal("Hunger", 0,0) { },
            new Goal("Thirst", 0,0) { },
            new Goal("Sleep", 0,0) { },
            new Goal("Bathroom", 0,0) { },
        },
            duration = 1f
        };

        private void Start()
        {
            myAction.actionPoint = transform;
            FindAnyObjectByType<ActionPlanner>().Add(myAction);
        }
    }
}