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
        public Robot robot;
        Rigidbody rb;

        public GameObject target;

        [HideInInspector] public CustomKinematic kinematic;

        private AgentGoal lastGoal;
        public AgentGoal currentGoal;
        public ActionPlan actionPlan;
        public AgentAction currentAction;

        public Dictionary<string, AgentBelief> beliefs;
        public SerializableHashSet<AgentAction> actions;
        public SerializableHashSet<AgentGoal> goals;

        IGoapPlanner gPlanner;

        private bool m_isInitialized = false;
        private bool m_isAIEnabled = false;

        public event Action Died;

        internal void Initialize(Robot robot)
        {
            this.robot = robot;

            kinematic = GetComponent<CustomKinematic>();

            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            gPlanner = new GoapPlanner();

            GetBeliefs();
            GetActions();
            GetGoals();

            robot.body.Break += OnDie;

            m_isInitialized = true;
        }

        internal void EnableAI() => m_isAIEnabled = true;
        internal void DisableAI() => m_isAIEnabled = false;

        private void OnDie()
        {
            robot.State.isAlive = false;
            Died?.Invoke();
            kinematic.DisableMovement();
            SFXPlayer.Instance.Death();
        }

        private void OnDestroy()
        {
            robot.body.Break -= OnDie;
        }

        //TODO MOVE THIS TO A STATIC UTIL
        bool InRangeOf(Vector3 position, float range) => Vector3.Distance(transform.position, position) < range;

        private void GetBeliefs()
        {
            beliefs = new Dictionary<string, AgentBelief>();

            /*      BASIC BELIEFS       */
            BeliefFactory factory = new(this, beliefs);
            factory.AddBelief("Nothing", () => false);
            factory.AddBelief("AttackingOpponent", () => false);
            factory.AddBelief("Alive", () => robot.State.isAlive);
            factory.AddBelief("IsPursuing", () => robot.State.isPursuing);
            factory.AddBelief("CanMove", () => robot.State.CanMove);

            /*      PART BELIEFS        */
            robot.State.LeftArmController.GetBeliefs(this, beliefs);
            robot.State.RightArmController.GetBeliefs(this, beliefs);
            robot.State.HeadController.GetBeliefs(this, beliefs);
        }

        private void GetActions()
        {
            actions = new SerializableHashSet<AgentAction>()
            {
                new AgentAction.Builder("Do Nothing")
                .AddEffect(beliefs["Nothing"])
                .WithStrategy(ScriptableObject
                .CreateInstance<IdleStrategy>()
                .Initialize(5))
                .WithCost(10)
                .Build()
            };
            robot.State.LeftArmController.GetActions(this, actions, beliefs);
            robot.State.RightArmController.GetActions(this, actions, beliefs);
            robot.State.HeadController.GetActions(this, actions, beliefs);
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
            robot.State.LeftArmController.GetGoals(this, goals, beliefs);
            robot.State.RightArmController.GetGoals(this, goals, beliefs);
            robot.State.HeadController.GetGoals(this, goals, beliefs);
        }

        //TODO Reapply this
        private void HandleTargetChanged()
        {
            //Debug.Log("Target changed, clearing current action and goal");
            currentAction = null;
            currentGoal = null;
        }

        private void Update()
        {
            if (!m_isInitialized || !m_isAIEnabled) return;

            if (currentAction == null)
            {
                //Debug.Log("Calculating any potential new plan");
                CalculatePlan();

                if (actionPlan != null && actionPlan.Actions.Count > 0)
                {
                    robot.State.ResetPath();

                    currentGoal = actionPlan.AgentGoal;
                    //Debug.Log($"Goal: {currentGoal.Name} with {actionPlan.Actions.Count} actions in plan.");
                    currentAction = actionPlan.Actions.Pop();
                    //Debug.Log($"Popped action: {currentAction.Name}\n");
                    //Verify all precondition effects are true
                    //string debug = "";
                    //foreach(var pre in currentAction.Preconditions)
                    //{
                    //    debug += $"{pre.name} is {pre.Evaluate()}";
                    //}
                    //Debug.Log(debug);
                    if (currentAction.Preconditions.All(b => b.Evaluate()))
                        currentAction.Start();
                    else
                    {
                        foreach (var precon in currentAction.Preconditions)
                        {
                            //if (!precon.Evaluate()) Debug.Log("Precon not met: " + precon.Name);
                        }
                        //Debug.Log("Preconditions not met, clearing current action and goal");
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
                    //Debug.Log($"{currentAction.Name} complete");
                    currentAction.Stop();
                    currentAction = null;

                    if (actionPlan.Actions.Count == 0)
                    {
                        //Debug.Log("Plan complete");
                        lastGoal = currentGoal;
                        currentGoal = null;
                    }
                }
            }
        }

        void CalculatePlan()
        {
            float priorityLevel = currentGoal != null ? currentGoal.Priority : 0;

            SerializableHashSet<AgentGoal> goalsToCheck = goals;

            if (currentGoal != null)
            {
                //Debug.Log("Current goal exists, checking goals with higher priority");
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