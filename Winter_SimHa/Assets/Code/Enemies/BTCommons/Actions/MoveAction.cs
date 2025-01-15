using Code.Enemies;
using Code.Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Code.Enemies.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Move", story: "[Self] move with [mover]", category: "Action", id: "e0f4d5406e32939f649b94009d59afc6")]
    public partial class MoveAction : Action
    {
        [SerializeReference] public BlackboardVariable<BTEnemy> Self;
        [SerializeReference] public BlackboardVariable<EntityMover> Mover;

        protected override Status OnStart()
        {
            Mover.Value.SetMovementX(1);
            return Status.Success;
        }

    }
}



