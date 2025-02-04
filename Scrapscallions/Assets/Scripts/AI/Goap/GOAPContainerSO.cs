using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    public class GOAPContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public SerializableDictionary<GOAPGroupSO, List<GoapScriptableObject>> GoapGroups { get; set; }
        [field: SerializeField] public List<GoapScriptableObject> UngroupedScriptableObjects { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            GoapGroups = new();
            UngroupedScriptableObjects = new();
        }
        public List<string> GetDialogueGroupNames()
        {
            List<string> dialogueGroupNames = new();

            foreach (var goapGroup in GoapGroups.Keys)
            {
                dialogueGroupNames.Add(goapGroup.Name);
            }

            return dialogueGroupNames;
        }

        public List<string> GetGroupedDialogueNames(GOAPGroupSO goapGroup, bool endsOnly)
        {
            List<GoapScriptableObject> groupedGoapObjects = GoapGroups[goapGroup];

            List<string> groupedDialogueNames = new();

            foreach (var groupedGoapObject in groupedGoapObjects)
            {
                if (endsOnly && (groupedGoapObject is AgentBelief))
                {
                    continue;
                }

                groupedDialogueNames.Add(groupedGoapObject.name);
            }

            return groupedDialogueNames;
        }

        public List<string> GetUngroupedGoapObjectNames(bool endsOnly)
        {
            List<string> ungroupedObjectNames = new();

            foreach (var ungroupedObject in UngroupedScriptableObjects)
            {
                if (endsOnly && (ungroupedObject is AgentBelief))
                {
                    continue;
                }

                ungroupedObjectNames.Add(ungroupedObject.name);
            }

            return ungroupedObjectNames;
        }
    }

    public class GOAPGroupSO : ScriptableObject { 
        [field: SerializeField] public string Name {  get; set; }

        public void Initialize(string groupName)
        {
            Name = name;
        }
    }

    public abstract class GoapScriptableObject : ScriptableObject { }
}