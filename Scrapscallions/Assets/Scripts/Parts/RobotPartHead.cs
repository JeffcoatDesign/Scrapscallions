using Scraps.AI.Quirks;
using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    [CreateAssetMenu(fileName = "New Robot Head", menuName = "Parts/Head")]
    public class RobotPartHead : RobotPart
    {
        [field: SerializeField] public override string PartName { get; set; }
        [field: SerializeField] public override float MaxHP { get; set; }
        [field: SerializeField] public override float CurrentHP { get; set; }
        [field: SerializeField] public override int Price { get; set; }
        [field: SerializeField] protected override GameObject Prefab { get; set; }
        [field: SerializeField] public override Sprite Sprite { get; set; }

        /*  HEAD PROPERTIES  */
        [field: SerializeField, Header("Head Properties")] public SerializableHashSet<Quirk> Quirks { get; set; }

        internal HeadController Spawn(BodyController bodyController)
        {
            return Instantiate(Prefab, bodyController.HeadAttachPoint).GetComponent<HeadController>();
        }
    }
}