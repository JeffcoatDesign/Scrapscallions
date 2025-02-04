using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    public class GoapAgent: MonoBehaviour
    {
        Robot robot;
        Rigidbody rb;

        [SerializeField] Sensor chaseSensor;
        [SerializeField] Sensor attackSensor;

        [Header("Stats")]
        public float health = 100;
        public float stamina = 100;

        CountdownTimer statsTimer;

        public GameObject target;
        Vector3 destination;

        AgentGoal lastGoal;
        public AgentGoal currentGoal;
        public ActionPlan actionPlan;
        public AgentAction currentAction;

        public Dictionary<string, AgentBelief> beliefs;
        public SerializableHashSet<AgentAction> actions;
        public SerializableHashSet<AgentGoal> goals;

        IGoapPlanner gPlanner;

        bool m_isInitialized = false;

        internal void Initialize(Robot robot)
        {
            this.robot = robot;
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            gPlanner = new GoapPlanner();

            SetUpTimers();
            GetBeliefs();
            GetActions();
            GetGoals();

            m_isInitialized = true;
        }

        private void SetUpTimers()
        {
            statsTimer = new CountdownTimer(2f);
            statsTimer.OnTimerStop += () =>
            {
                UpdateStats();
                statsTimer.Start();
            };
            statsTimer.Start();
        }

        //TODO Move to stats system
        private void UpdateStats()
        {
            /*stamina += InRangeOf(restingPosition.position, 3f) ? 20 : -10;
            health += InRangeOf(foodShack.position, 3f) ? 20 : -5;
            stamina = Mathf.Clamp(stamina, 0, 100);
            health = Mathf.Clamp(health, 0, 100);*/
        }

        bool InRangeOf(Vector3 position, float range) => Vector3.Distance(transform.position, position) < range;

        private void GetBeliefs()
        {
            beliefs = new Dictionary<string, AgentBelief>();

            /*      BASIC BELIEFS       */
            BeliefFactory factory = new(this, beliefs);
            factory.AddBelief("Nothing", () => false);
            factory.AddBelief("AttackingOpponent", () => false);

            /*      PART BELIEFS        */
            robot.State.LeftArmController.GetBeliefs(this, beliefs);
            robot.State.RightArmController.GetBeliefs(this, beliefs);

            foreach (KeyValuePair<string, AgentBelief> pair in beliefs)
            {
                Debug.Log(pair.Key);
            }
        }

        private void GetActions()
        {
            actions = new SerializableHashSet<AgentAction>()
            {
                new AgentAction.Builder("Do Nothing").AddEffect(beliefs["Nothing"]).WithStrategy(ScriptableObject.CreateInstance<IdleStrategy>().Initialize(5)).Build()
            };
            actions.CopyFrom(robot.State.LeftArmController.GetActions(this, beliefs));
            actions.CopyFrom(robot.State.RightArmController.GetActions(this, beliefs));

            foreach (var value in actions)
            {
                Debug.Log(value.Name);
            }
        }

        private void GetGoals()
        {
            goals = new SerializableHashSet<AgentGoal>()
            {
                // BASIC GOALS
                new AgentGoal.Builder("SeekAndDestroy").WithPriority(3).WithDesiredEffect(beliefs["AttackingOpponent"]).Build(),
                new AgentGoal.Builder("Rest").WithPriority(0).WithDesiredEffect(beliefs["Nothing"]).Build()
            };

            /*      PART GOALS      */
            goals.CopyFrom(robot.State.LeftArmController.GetGoals(this, beliefs));
            goals.CopyFrom(robot.State.RightArmController.GetGoals(this, beliefs));

            foreach (AgentGoal goal in goals)
            {
                Debug.Log(goal.Name);
            }
        }

        private void HandleTargetChanged()
        {
            Debug.Log("Target changed, clearing current action and goal");
            currentAction = null;
            currentGoal = null;
        }

        private void Update()
        {
            if (!m_isInitialized) return;

            statsTimer.Tick(Time.deltaTime);

            if (currentAction == null)
            {
                Debug.Log("Calculating any potential new plan");
                CalculatePlan();

                if (actionPlan != null && actionPlan.Actions.Count > 0)
                {
                    robot.ResetPath();

                    currentGoal = actionPlan.AgentGoal;
                    Debug.Log($"Goal: {currentGoal.Name} with {actionPlan.Actions.Count} actions in plan.");
                    currentAction = actionPlan.Actions.Pop();
                    Debug.Log($"Popped action: {currentAction.Name}");
                    //Verify all precondition effects are true
                    if (currentAction.Preconditions.All(b => b.Evaluate()))
                        currentAction.Start();
                    else
                    {
                        Debug.Log("Preconditions not met, clearing current action and goal");
                        currentAction = null;
                        currentGoal = null;
                    }
                }
            }

            if (actionPlan != null && currentAction != null)
            {
                currentAction.Tick(Time.deltaTime);

                if (currentAction.IsComplete)
                {
                    Debug.Log($"{currentAction.Name} complete");
                    currentAction.Stop();
                    currentAction = null;

                    if (actionPlan.Actions.Count == 0)
                    {
                        Debug.Log("Plan complete");
                        lastGoal = currentGoal;
                        currentGoal = null;
                    }
                }
            }
        }

        void CalculatePlan()
        {
            var priorityLevel = currentGoal?.Priority ?? 0;

            SerializableHashSet<AgentGoal> goalsToCheck = goals;

            if (currentGoal != null)
            {
                Debug.Log("Current goal exists, checking goals with higher priority");
                goalsToCheck = new SerializableHashSet<AgentGoal>(goals.Where(g => g.Priority > priorityLevel).ToHashSet());
            }

            var potentialPlan = gPlanner.Plan(this, goalsToCheck, lastGoal);
            if (potentialPlan != null)
            {
                actionPlan = potentialPlan;
            }
        }
    }
}