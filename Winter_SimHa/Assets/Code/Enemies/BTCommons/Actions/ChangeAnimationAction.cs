using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Code.Enemies.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChangeAnimation", story: "[animator] change [current] to [next]", category: "Action", id: "fb90d75668e833e0f678e3a3a15ad5b2")]
    public partial class ChangeAnimationAction : Action
    {
        [SerializeReference] public BlackboardVariable<Animator> Animator;
        [SerializeReference] public BlackboardVariable<string> Current;
        [SerializeReference] public BlackboardVariable<string> Next;

        protected override Status OnStart()
        {
            Animator.Value.SetBool(Current.Value, false);
            Current.Value = Next.Value;
            Animator.Value.SetBool(Next.Value, true);
            return Status.Success;
        }
    }


}
