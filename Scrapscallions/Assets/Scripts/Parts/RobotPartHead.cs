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
        [field: SerializeField] public override int MaxHP { get; set; }
        [field: SerializeField] public override int CurrentHP { get; set; }
        [field: SerializeField] public override int Price { get; set; }
        [field: SerializeField] protected override GameObject Prefab { get; set; }
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField] public override bool IsBroken { get; set; } = false;

        /*  HEAD PROPERTIES  */
        [field: SerializeField, Header("Head Properties")] public SerializableHashSet<Quirk> Quirks { get; set; }

        internal HeadController Spawn(BodyController bodyController)
        {
            if (bodyController == null) Debug.LogWarning("Body controller not found");
            return Instantiate(Prefab, bodyController.HeadAttachPoint).GetComponent<HeadController>();
        }
    }
}