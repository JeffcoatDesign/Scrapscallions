using Scraps.Parts;
using Scraps.Utilities;
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
        [SerializeField] private float m_skewQuotient = 2;
        [Header("Difficulty Curves")]
        [SerializeField] private AnimationCurve m_healthCurve;
        internal RobotPart GetRandomPart(PartType type, Rarity rarity = Rarity.Any, bool randomizeHealth = false, float skew = 0)
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
                float randomValue = MathUtility.NormalizedRandom(0, 1) + skew;
                bool isCommon = randomValue < commonChance;
                bool isRare = !isCommon && randomValue < 1 - m_epicChance;

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
                float randomValue = m_healthCurve.Evaluate(MathUtility.NormalizedRandom(0.1f) + skew);
                part.CurrentHP = Mathf.Clamp(Mathf.RoundToInt(randomValue * part.MaxHP), 0, int.MaxValue);
            } 
            else
                part.CurrentHP = part.MaxHP;

            return part;
        }

        public Robot GetRandomRobot(int numDefeated, bool randomizeHealth = false)
        {
            float skew = numDefeated / m_skewQuotient;
            Robot robot = CreateInstance<Robot>();
            robot.head = (RobotPartHead)GetRandomPart(PartType.Head, Rarity.Any, randomizeHealth, skew);
            robot.leftArm = (RobotPartArm)GetRandomPart(PartType.Arm, Rarity.Any, randomizeHealth, skew);
            robot.rightArm = (RobotPartArm)GetRandomPart(PartType.Arm, Rarity.Any, randomizeHealth, skew);
            robot.body = (RobotPartBody)GetRandomPart(PartType.Body, Rarity.Any, randomizeHealth, skew);
            robot.legs = (RobotPartLegs)GetRandomPart(PartType.Legs, Rarity.Any, randomizeHealth, skew);
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