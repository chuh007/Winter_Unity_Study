using Code.Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Code.Enemies.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "FacingToTarget", story: "[self] facing to [target] with [renderer]", category: "Action", id: "97cbcf78a556b702b165e227c43bb621")]
    public partial class FacingToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;

        protected override Status OnStart()
        {
            Vector2 direction = Target.Value.position - Self.Value.position;

            Renderer.Value.FlipController(Mathf.Sign(direction.x));

            return Status.Success;
        }
    }
}


