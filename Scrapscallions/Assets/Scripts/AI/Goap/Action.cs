using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scraps.AI {
    [System.Serializable]
    public class Action
    {
        public string name;
        public List<Goal> effectedGoals;
        public float duration;
        [HideInInspector] public Transform actionPoint;
        public Action(string name)
        {
            this.name = name;
        }

        public float GetGoalChange(Goal goal)
        {
            Goal effectedGoal = effectedGoals.FirstOrDefault(g => g.name == goal.name);
            if (effectedGoal != null)
            {
                return effectedGoal.value;
            }
            return 0f;
        }

        public float GetDistance(Transform actorTransform)
        {
            return Vector3.Distance(actionPoint.position, actorTransform.position);
        }
    }
}