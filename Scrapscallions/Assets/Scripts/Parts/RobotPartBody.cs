using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    [CreateAssetMenu(fileName = "New Robot Body", menuName = "Parts/Body")]
    public class RobotPartBody : RobotPart
    {
        [field: SerializeField] public override string PartName { get; set; }
        [field: SerializeField] public override int MaxHP { get; set; }
        [field: SerializeField] public override int CurrentHP { get; set; }
        [field: SerializeField] public override int Price { get; set; }
        [field: SerializeField] protected override GameObject Prefab { get; set; }
        [field: SerializeField] public override int ItemID { get; set; }
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField] public override bool IsBroken { get; set; } = false;

        /*  BODY PROPERTIES  */
        //[field: SerializeField, Header("Body Properties")] public float AttackSpeed { get; set; }
        //[field: SerializeField] public float AttackDamage { get; set; }

        public BodyController Spawn(LegsController legsController)
        {
            return Instantiate(Prefab,legsController.BodyAttachPoint).GetComponent<BodyController>();
        }
    }
}