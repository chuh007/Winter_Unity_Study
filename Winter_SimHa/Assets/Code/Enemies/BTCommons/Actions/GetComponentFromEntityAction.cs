using Code.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Code.Entities;

namespace Code.Enemies.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "GetComponentFromEntity", story: "Get components form [btEnemy]", category: "Action", id: "8b26e18d3bdce6ea55d1db3f059754ee")]
    public partial class GetComponentFromEntityAction : Action
    {
        [SerializeReference] public BlackboardVariable<BTEnemy> BtEnemy;

        protected override Status OnStart()
        {
            BTEnemy enemy = BtEnemy.Value;

            SetVariableToBT(enemy, "Renderer", enemy.GetCompo<EntityRenderer>());
            SetVariableToBT(enemy, "MainAnimator", enemy.GetCompo<EntityRenderer>().GetComponent<Animator>());
            SetVariableToBT(enemy, "Mover", enemy.GetCompo<EntityMover>());
            SetVariableToBT(enemy, "AnimationTrigger", enemy.GetCompo<EntityAnimationTrigger>());

            return Status.Success;
        }

        private void SetVariableToBT<T>(BTEnemy enemy, string variableName, T component)
        {
            Debug.Assert(component != null, $"Check {variableName} trigger component exist on {enemy.gameObject.name}");
            BlackboardVariable<T> variable = enemy.GetBlackboardVariable<T>(variableName);
            variable.Value = component;
        }
    }
}


