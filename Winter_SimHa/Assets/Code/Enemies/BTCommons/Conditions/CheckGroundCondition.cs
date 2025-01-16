using Code.Entities;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckGround", story: "[mover] ground is [status]", category: "Conditions", id: "47b62c9d8ab2179911ad9e5ecfa5930d")]
public partial class CheckGroundCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EntityMover> Mover;
    [SerializeReference] public BlackboardVariable<bool> Status;

    public override bool IsTrue()
    {
        return Mover.Value.IsGroundDetected() == Status.Value;
    }
}
