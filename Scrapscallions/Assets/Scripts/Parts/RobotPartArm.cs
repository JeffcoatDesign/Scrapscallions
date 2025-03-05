using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    [CreateAssetMenu(fileName = "New Robot Arm", menuName = "Parts/Arm")]
    public class RobotPartArm : RobotPart
    {
        [field: SerializeField] public override string PartName { get; set; } = "Robot Arm";
        [field: SerializeField] public override int MaxHP { get; set; } = 40;
        [field: SerializeField] public override int CurrentHP { get; set; } = 40;
        [field: SerializeField] public override int Price { get; set; } = 100;
        [field: SerializeField] protected override GameObject Prefab { get; set; }
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField] public override bool IsBroken { get; set; } = false;

        /*  ARM PROPERTIES  */
        [field: SerializeField, Header("Arm Properties")] public float AttackSpeed { get; set; } = 10;
        [field: SerializeField] public int AttackDamage { get; set; } = 20;

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