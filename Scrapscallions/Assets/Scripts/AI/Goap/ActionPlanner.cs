using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Scraps.AI
{
    public class ActionPlanner : MonoBehaviour
    {
        public TextMeshProUGUI m_currentActionText;

        //[SerializeField] protected Agent m_agent; 
        [SerializeField] protected List<Goal> myGoals;
        protected List<Action> myActions = new();
        protected Action m_currentAction;
        protected bool m_startedAction = false;
        public void Add(Action action)
        {
            myActions.Add(action);
        }

        private void Awake()
        {
            //m_sgent = GetComponent<Agent>();

            myGoals = new List<Goal>
            {
                new Goal("Hunger", 5f, 2f),
                new Goal("Thirst", 4f, 2f),
                new Goal("Sleep", 4f, 2f),
                new Goal("Bathroom", 4f, 1f)
            };

            InvokeRepeating(nameof(Decay), 0f, 5f);
        }

        private void Update()
        {
            if (m_currentAction == null)
            {
                m_currentAction = ChooseAction(myActions, myGoals);
                //m_agent.SetDestination(m_currentAction.actionPoint.position);
                // TODO Get in range
                transform.position = m_currentAction.actionPoint.position;
                m_currentActionText.text = m_currentAction.name;
            }
            if (m_currentAction.GetDistance(transform) < 1.1f && !m_startedAction)
            {
                TakeAction();
                m_startedAction = true;
            }
        }

        private void ClearAction()
        {
            m_currentAction = null;
            m_startedAction = false;
            m_currentActionText.text = "";
        }

        private void Decay()
        {
            string goalsStatus = "";
            foreach (Goal goal in myGoals)
            {
                goalsStatus += goal.name + " " + goal.value + "\n";
                goal.value += goal.rateOfChange;
            }
            Debug.Log(goalsStatus);
        }
        private void TakeAction()
        {
            string goalsStatus = "";
            foreach (Goal goal in myGoals)
            {
                goal.value += m_currentAction.GetGoalChange(goal);
                goal.value = Mathf.Clamp(goal.value, 0f, goal.value);
                goalsStatus += goal.name + " " + goal.value + "\n";
            }
            Debug.Log("I will: " + m_currentAction.name + "\n" + goalsStatus);
            Invoke("ClearAction", m_currentAction.duration);
        }

        Action ChooseAction(List<Action> actions, List<Goal> goals)
        {
            Action bestAction = actions[0];
            float bestValue = float.MaxValue;

            foreach (var action in actions)
            {
                float thisValue = GetDiscontentment(action, goals);
                if (thisValue < bestValue)
                {
                    bestValue = thisValue;
                    bestAction = action;
                }
            }

            return bestAction;
        }

        private float GetDiscontentment(Action action, List<Goal> goals)
        {
            float discontentment = 0f;

            foreach (var goal in goals)
            {
                float newValue = goal.value + action.GetGoalChange(goal);

                newValue += action.GetDistance(transform) / 10f;

                // This won't stop making them only drink or go to the bathroom
                newValue += action.duration * goal.rateOfChange;

                discontentment += goal.GetDiscontentment(newValue);
            }

            return discontentment;
        }
    }
}