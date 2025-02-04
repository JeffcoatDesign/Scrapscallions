using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    public class OldGoapAgent : MonoBehaviour
    {
        [Header("Sensors")]
        [SerializeField] Sensor chaseSensor;
        [SerializeField] Sensor attackSensor;

        [Header("Known Locations")]
        [SerializeField] Transform restingPosition;
        [SerializeField] Transform foodShack;
        [SerializeField] Transform doorOnePosition;
        [SerializeField] Transform doorTwoPosition;

        Robot robot;
        Rigidbody rb;

        [Header("Stats")]
        public float health = 100;
        public float stamina = 100;

        CountdownTimer statsTimer;

        GameObject target;
        Vector3 destination;

        AgentGoal lastGoal;
        public AgentGoal currentGoal;
        public ActionPlan actionPlan;
        public AgentAction currentAction;

        public Dictionary<string, AgentBelief> beliefs;
        public SerializableHashSet<AgentAction> actions;
        public SerializableHashSet<AgentGoal> goals;

        IGoapPlanner gPlanner;

        private void Awake()
        {
            robot = GetComponent<Robot>();
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            gPlanner = new GoapPlanner();
        }

        private void Start()
        {
            SetUpTimers();
            SetupBeliefs();
            SetupActions();
            SetupGoals();
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
            stamina += InRangeOf(restingPosition.position, 3f) ? 20 : -10;
            health += InRangeOf(foodShack.position, 3f) ? 20 : -5;
            stamina = Mathf.Clamp(stamina, 0, 100);
            health = Mathf.Clamp(health, 0, 100);
        }

        bool InRangeOf(Vector3 position, float range) => Vector3.Distance(transform.position, position) < range;

        private void SetupBeliefs()
        {
            //beliefs = new Dictionary<string, AgentBelief>();
            //BeliefFactory factory = new BeliefFactory(this, beliefs);

            //factory.AddBelief("Nothing", () => false);
            //factory.AddBelief("AgentIdle", () => !robot.hasPath);
            //factory.AddBelief("AgentMoving", () => robot.hasPath);
            //factory.AddBelief("AgentHealthLow", () => health < 30);
            //factory.AddBelief("AgentIsHealthy", () => health >= 50);
            //factory.AddBelief("AgentStaminLow", () => stamina < 10);
            //factory.AddBelief("AgentIsRested", () => stamina >= 50);

            //factory.AddLocationBelief("AgentAtDoorOne", 3f, doorOnePosition);
            //factory.AddLocationBelief("AgentAtDoorTwo", 3f, doorTwoPosition);
            //factory.AddLocationBelief("AgentAtRestingPosition", 3f, restingPosition);
            //factory.AddLocationBelief("AgentAtFoodShack", 3f, foodShack);

            //factory.AddSensorBelief("PlayerInChaseRange", chaseSensor);
            //factory.AddSensorBelief("PlayerInAttackRange", attackSensor);
            //factory.AddBelief("AttackingPlayer", () => false); //Player can always be attacked, this will never become true
        }

        private void SetupActions()
        {
            actions = new SerializableHashSet<AgentAction> {
            new AgentAction.Builder("Relax")
                .WithStrategy(new IdleStrategy().Initialize(5))
                .AddEffect(beliefs["Nothing"])
                .Build(),
            new AgentAction.Builder("Wander Around")
                .WithStrategy(new WanderStrategy().Initialize(robot, 10))
                .AddEffect(beliefs["AgentMoving"])
                .Build(),
            new AgentAction.Builder("MoveToEatingPostion")
                .WithStrategy(new MoveToStrategy().Initialize(robot, () => foodShack.position))
                .AddEffect(beliefs["AgentAtFoodShack"])
                .Build(),
            new AgentAction.Builder("Eat")
                .WithStrategy(new IdleStrategy().Initialize(5)) //Replace with a command later
                .AddEffect(beliefs["AgentIsHealthy"])
                .WithPrecondition(beliefs["AgentAtFoodShack"])
                .Build(),
            new AgentAction.Builder("MoveToDoorOne")
                .WithStrategy(new MoveToStrategy().Initialize(robot, () => doorOnePosition.position))
                .AddEffect(beliefs["AgentAtDoorOne"])
                .Build(),
            new AgentAction.Builder("MoveToDoorTwo")
                .WithStrategy(new MoveToStrategy().Initialize(robot, () => doorTwoPosition.position))
                .AddEffect(beliefs["AgentAtDoorTwo"])
                .Build(),
            new AgentAction.Builder("MoveFromDoorOneToRestArea")
                .WithStrategy(new MoveToStrategy().Initialize(robot, () => restingPosition.position))
                .WithPrecondition(beliefs["AgentAtDoorOne"])
                .AddEffect(beliefs["AgentAtRestingPosition"])
                .Build(),
            new AgentAction.Builder("MoveFromDoorTwoToRestArea")
                .WithStrategy(new MoveToStrategy().Initialize(robot, () => restingPosition.position))
                .WithCost(2)
                .WithPrecondition(beliefs["AgentAtDoorTwo"])
                .AddEffect(beliefs["AgentAtRestingPosition"])
                .Build(),
            new AgentAction.Builder("Rest")
                .WithStrategy(new IdleStrategy().Initialize(5))
                .WithPrecondition(beliefs["AgentAtRestingPosition"])
                .AddEffect(beliefs["AgentIsRested"])
                .Build(),
            new AgentAction.Builder("ChasePlayer")
                .WithStrategy(new MoveToStrategy().Initialize(robot, () => beliefs["PlayerInChaseRange"].Location))
                .WithPrecondition(beliefs["PlayerInChaseRange"])
                .AddEffect(beliefs["PlayerInAttackRange"])
                .Build(),
            new AgentAction.Builder("AttackPlayer")
                .WithStrategy(new AttackStrategy().Initialize(1))
                .WithPrecondition(beliefs["PlayerInAttackRange"])
                .AddEffect(beliefs["AttackingPlayer"])
                .Build()
        };
        }

        private void SetupGoals()
        {
            goals = new SerializableHashSet<AgentGoal>
        {
            new AgentGoal.Builder("Chill Out").WithPriority(1).WithDesiredEffect(beliefs["Nothing"]).Build(),
            new AgentGoal.Builder("Wander Around").WithPriority(1).WithDesiredEffect(beliefs["AgentMoving"]).Build(),
            new AgentGoal.Builder("KeepHealthUp").WithPriority(2).WithDesiredEffect(beliefs["AgentIsHealthy"]).Build(),
            new AgentGoal.Builder("KeepStaminaUp").WithPriority(2).WithDesiredEffect(beliefs["AgentIsRested"]).Build(),
            new AgentGoal.Builder("SeekAndDestroy").WithPriority(3).WithDesiredEffect(beliefs["AttackingPlayer"]).Build()
        };
        }

        private void OnEnable() => chaseSensor.OnTargetChanged += HandleTargetChanged;
        private void OnDisable() => chaseSensor.OnTargetChanged -= HandleTargetChanged;

        private void HandleTargetChanged()
        {
            Debug.Log("Target chaged, clearing current action and goal");
            currentAction = null;
            currentGoal = null;
        }

        private void Update()
        {
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

            //var potentialPlan = gPlanner.Plan(this, goalsToCheck, lastGoal);
            //if (potentialPlan != null)
            //{
            //    actionPlan = potentialPlan;
            //}
        }
    }
}