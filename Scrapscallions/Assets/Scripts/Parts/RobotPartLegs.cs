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
        [field: SerializeField] public override float MaxHP { get; set; }
        [field: SerializeField] public override float CurrentHP { get; set; }
        [field: SerializeField] public override int Price { get; set; }
        [field: SerializeField] protected override GameObject Prefab { get; set; }
        [field: SerializeField] public override Sprite Sprite { get; set; }

        /*  LEG PROPERTIES  */
        //[field: SerializeField, Header("Leg Properties")] public float AttackSpeed { get; set; }
        //[field: SerializeField] public float AttackDamage { get; set; }
        public LegsController Spawn(GoapAgent agent)
        {
            return Instantiate(Prefab, agent.transform).GetComponent<LegsController>();
        }
    }
}