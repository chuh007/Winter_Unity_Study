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
    [NodeDescription(name: "Stop", story: "stop with [mover] on [yAxis]", category: "Action", id: "65b1cda6b08a7f04f78f88c804fa943d")]
    public partial class StopAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityMover> Mover;
        [SerializeReference] public BlackboardVariable<bool> YAxis;

        protected override Status OnStart()
        {
            Mover.Value.StopImmediately(YAxis.Value);
            return Status.Success;
        }
    }
}


