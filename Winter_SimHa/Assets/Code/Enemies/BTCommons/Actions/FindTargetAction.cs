using Code.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Code.Enemies.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "FindTarget", story: "[self] set [target] form finder", category: "Action", id: "2f489222f77bba23383cb92cfe6a30ed")]
    public partial class FindTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<BTEnemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            Target.Value = Self.Value.PlayerFinder.target.transform;
            Debug.Assert(Target.Value != null,$"Target is null : {Self.Value.gameObject.name}");

            return Status.Success;
        }
    }
}


