using Code.Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Code.Enemies.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitForAnimationTrigger", story: "Wait for [trigger] end", category: "Action", id: "a5236abd8a883314d8fdee91f3852248")]
    public partial class WaitForAnimationTriggerAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityAnimationTrigger> Trigger;

        private bool _animationEndTrigger;

        protected override Status OnStart()
        {
            _animationEndTrigger = false;
            Trigger.Value.OnAnimationEnd += HandleAnimationEnd;
            return Status.Running;
        }


        protected override Status OnUpdate()
        {
            return _animationEndTrigger ? Status.Success : Status.Running;
        }

        protected override void OnEnd()
        {
            Trigger.Value.OnAnimationEnd -= HandleAnimationEnd;
        }

        private void HandleAnimationEnd()
        {
            _animationEndTrigger = true;
        }
    }
}


