using Code.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayExclamation", story: "[Enemy] play exclamation fx", category: "Action", id: "050e8bc8162305312b3ee92ab655bdc8")]
public partial class PlayExclamationAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<float> Offset;
    [SerializeReference] public BlackboardVariable<Vector3> Scale;

    protected override Status OnStart()
    {
        Enemy.Value.ShowExclamationFX(Offset.Value, Scale.Value);
        return Status.Success;
    }
}

//다음시간에 사라지게 만드는거까지 합니다.