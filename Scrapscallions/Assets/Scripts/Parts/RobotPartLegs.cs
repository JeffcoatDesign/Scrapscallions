using Scraps.AI.GOAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    [CreateAssetMenu(fileName = "New Robot Legs", menuName = "Parts/Legs")]
    public class RobotPartLegs : RobotPart
    {
        [field: SerializeField] public override string PartName { get; set; }
        [field: SerializeField] public override int MaxHP { get; set; }
        [field: SerializeField] public override int CurrentHP { get; set; }
        [field: SerializeField] public override int Price { get; set; }
        [field: SerializeField] protected override GameObject Prefab { get; set; }
        [field: SerializeField] public override int ItemID { get; set; }
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField] public override bool IsBroken { get; set; } = false;
        [field: SerializeField] public float MaxSpeed { get; set; } = 10;
        [field: SerializeField] public float MaxAngularAcceleration { get; internal set; } = 45;

        /*  LEG PROPERTIES  */
        //[field: SerializeField, Header("Leg Properties")] public float AttackSpeed { get; set; }
        //[field: SerializeField] public float AttackDamage { get; set; }
        public LegsController Spawn(GoapAgent agent)
        {
            return Instantiate(Prefab, agent.transform).GetComponent<LegsController>();
        }

        public LegsController Spawn(Transform parent)
        {
            return Instantiate(Prefab, parent).GetComponent<LegsController>();
        }
    }
}