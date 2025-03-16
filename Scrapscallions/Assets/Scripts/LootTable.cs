using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scraps.Gameplay
{
    [CreateAssetMenu(menuName ="Loot Table", fileName = "New Loot Table")]
    public class LootTable : ScriptableObject
    {
        [SerializeField] private List<LootTableEntry> m_entries = new();
        [SerializeField] private float m_rareChance, m_epicChance;
        [SerializeField] private AnimationCurve m_healthCurve;
        internal RobotPart GetRandomPart(PartType type, Rarity rarity = Rarity.Any, bool randomizeHealth = false)
        {
            RobotPart part = null;
            List<LootTableEntry> parts;
            if (type != PartType.Any)
                parts = m_entries.Where(p => p.partType == type).ToList();
            else
                parts = m_entries;

            if (rarity == Rarity.Any)
            {
                float commonChance = 1f - m_rareChance - m_epicChance;
                float randomValue = Random.Range(0f, 1f);
                bool isCommon = randomValue < commonChance;
                bool isRare = !isCommon && randomValue < m_epicChance;
                if (isCommon) rarity = Rarity.Common;
                else if (isRare) rarity = Rarity.Rare;
                else rarity = Rarity.Epic;
            }
            if (parts.Where(p => p.rarity == rarity).Count() > 0)
                parts.RemoveAll(p => p.rarity != rarity);
            if (parts.Count == 0) Debug.Log("No part found");
            //Look for a less rare part if none are found
            if (parts.Count < 1 && rarity != Rarity.Common) GetRandomPart(type, rarity - 1);
            else if (parts.Count < 1 && rarity != Rarity.Epic) GetRandomPart(type, rarity + 1);

            int randomIndex = Random.Range(0, parts.Count);
            part = parts[randomIndex].part;

            part = Instantiate(part);

            if (randomizeHealth)
            {
                float randomValue = m_healthCurve.Evaluate(Random.Range(0f, 1f));
                part.CurrentHP = Mathf.RoundToInt(randomValue * part.MaxHP);
            } 
            else
                part.CurrentHP = part.MaxHP;

            return part;
        }

        public Robot GetRandomRobot(bool randomizeHealth = false)
        {
            Robot robot = CreateInstance<Robot>();
            robot.head = (RobotPartHead)GetRandomPart(PartType.Head, Rarity.Any, randomizeHealth);
            robot.leftArm = (RobotPartArm)GetRandomPart(PartType.Arm, Rarity.Any, randomizeHealth);
            robot.rightArm = (RobotPartArm)GetRandomPart(PartType.Arm, Rarity.Any, randomizeHealth);
            robot.body = (RobotPartBody)GetRandomPart(PartType.Body, Rarity.Any, randomizeHealth);
            robot.legs = (RobotPartLegs)GetRandomPart(PartType.Legs, Rarity.Any, randomizeHealth);
            return robot;
        }

        [System.Serializable]
        private class LootTableEntry
        {
            public Rarity rarity;
            public PartType partType;
            public RobotPart part;
        }
        internal enum Rarity
        {
            Any,
            Common,
            Rare,
            Epic
        }
        internal enum PartType
        {
            Any,
            Head,
            Body,
            Arm,
            Legs
        }
    }
}