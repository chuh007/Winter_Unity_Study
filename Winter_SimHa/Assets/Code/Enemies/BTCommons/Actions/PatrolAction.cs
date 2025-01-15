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
    [NodeDescription(name: "Patrol", story: "[Self] patrol with [mover] in [sec]", category: "Action", id: "95f4a5db24fd8364a4e8abe9a456a45d")]
    public partial class PatrolAction : Action
    {
        [SerializeReference] public BlackboardVariable<BTEnemy> Self;
        [SerializeReference] public BlackboardVariable<EntityMover> Mover;
        [SerializeReference] public BlackboardVariable<float> Sec;

        private float _startTime;

        protected override Status OnStart()
        {
            Mover.Value.SetMovementX(Self.Value.FacingDirection);
            _startTime = Time.time;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            bool isOverTime = Sec.Value + _startTime < Time.time;
            bool isGround = Mover.Value.IsGroundDetected();

            if(isOverTime || isGround == false)
            {
                return Status.Success;
            }

            return Status.Running;
        }

        protected override void OnEnd()
        {
        }
    }
}


