using Code.Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Code.Enemies.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Flip", story: "Flip [renderer]", category: "Action", id: "7e47e17c2391f75adfa22ae32defc18c")]
    public partial class FlipAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;

        protected override Status OnStart()
        {
            Renderer.Value.Flip();
            return Status.Success;
        }
    }
}


