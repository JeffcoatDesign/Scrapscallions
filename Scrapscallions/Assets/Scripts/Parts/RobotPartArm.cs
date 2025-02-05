using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    [CreateAssetMenu(fileName = "New Robot Arm", menuName = "Parts/Arm")]
    public class RobotPartArm : RobotPart
    {
        [field: SerializeField] public override string PartName { get; set; }
        [field: SerializeField] public override float MaxHP { get; set; }
        [field: SerializeField] public override float CurrentHP { get; set; }
        [field: SerializeField] public override int Price { get; set; }
        [field: SerializeField] protected override GameObject Prefab { get; set; }
        [field: SerializeField] public override Sprite Sprite { get; set; }

        /*  ARM PROPERTIES  */
        [field: SerializeField, Header("Arm Properties")] public float AttackSpeed { get; set; }
        [field: SerializeField] public float AttackDamage { get; set; }
        public ArmController Spawn(BodyController bodyController, bool isRightArm)
        {
            Transform attachPoint = null;

            if (isRightArm)
                attachPoint = bodyController.RightArmAttachPoint;
            else
                attachPoint = bodyController.LeftArmAttachPoint;

            ArmController armController = Instantiate(Prefab, attachPoint).GetComponent<ArmController>();
            armController.side = isRightArm ? ArmController.Side.Right : ArmController.Side.Left;

            if (isRightArm) armController.transform.localScale = armController.transform.localScale.With(x:-armController.transform.localScale.x);

            return armController;
        }
    }
}