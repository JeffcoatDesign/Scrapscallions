using Scraps.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    public interface IGoapPlanner
    {
        ActionPlan Plan(GoapAgent agent, SerializableHashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null);
    }

    public class GoapPlanner : IGoapPlanner
    {
        public ActionPlan Plan(GoapAgent agent, SerializableHashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null)
        {
            //order goals by priority, descending
            List<AgentGoal> orderedGoals = goals
                .Where(g => g.DesiredEffects.Any(b => !b.Evaluate()))
                .OrderByDescending(g => g == mostRecentGoal ? g.Priority - 0.01 : g.Priority) //Most recent goal's priority is shrunk a little
                .ToList();

            //Try to solve for each goal in order
            foreach (var goal in orderedGoals)
            {
                Node goalNode = new(null, null, goal.DesiredEffects, 0);

                //If we can find a path to the goal, return the plan
                if (FindPath(goalNode, agent.actions))
                {
                    //If the goal node has no leaves and no action to perform try a different goal
                    if (goalNode.IsLeafDead) continue;

                    Stack<AgentAction> actionStack = new Stack<AgentAction>();
                    while (goalNode.Leaves.Count > 0)
                    {
                        var cheapestLeaf = goalNode.Leaves.OrderBy(leaf => leaf.Cost).First();
                        goalNode = cheapestLeaf;
                        actionStack.Push(cheapestLeaf.Action);
                    }

                    return new ActionPlan(goal, actionStack, goalNode.Cost);
                }
            }

            Debug.LogWarning("No Action Plan Found");
            return null;
        }

        //TODO use A*
        private bool FindPath(Node parent, SerializableHashSet<AgentAction> actions)
        {
            //Order actions by cost
            var orderedActions = actions.OrderBy(a => a.Cost);
            foreach (var action in orderedActions)
            {
                var requiredEffects = parent.RequiredEffects;

                //Remove any effects that evaluate to true, there is no action to take.
                requiredEffects.RemoveWhere(b => b.Evaluate());

                // If there are no required effect to fulfill, we have a plan
                if (requiredEffects.Count == 0)
                {
                    return true;
                }

                if (action.Effects.Any(requiredEffects.Contains))
                {
                    SerializableHashSet<AgentBelief> newRequiredEffects = new(requiredEffects);
                    newRequiredEffects.ExceptWith(action.Effects);
                    newRequiredEffects.UnionWith(action.Preconditions);

                    var newAvailableActions = new SerializableHashSet<AgentAction>(actions);
                    newAvailableActions.Remove(action);

                    var newNode = new Node(parent, action, newRequiredEffects, parent.Cost + action.Cost);

                    if (FindPath(newNode, newAvailableActions))
                    {
                        parent.Leaves.Add(newNode);
                        newRequiredEffects.ExceptWith(newNode.Action.Preconditions);
                    }

                    //If all effects at this depth have been satisfied, return true
                    if (newRequiredEffects.Count == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class Node
    {
        public Node Parent { get; }
        public AgentAction Action { get; }
        public SerializableHashSet<AgentBelief> RequiredEffects { get; }
        public List<Node> Leaves { get; }
        public float Cost { get; }

        public bool IsLeafDead => Leaves.Count == 0 && Action == null;

        public Node(Node parent, AgentAction action, SerializableHashSet<AgentBelief> effects, float cost)
        {
            Parent = parent;
            Action = action;
            RequiredEffects = new(effects);
            Leaves = new List<Node>();
            Cost = cost;
        }
    }

    public class ActionPlan
    {
        public AgentGoal AgentGoal { get; }
        public Stack<AgentAction> Actions { get; }
        public float TotalCost { get; set; }

        public ActionPlan(AgentGoal agentGoal, Stack<AgentAction> actions, float totalCost)
        {
            AgentGoal = agentGoal;
            Actions = actions;
            TotalCost = totalCost;
        }
    }
}