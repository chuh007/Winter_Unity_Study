using Code.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayExclamation", story: "[Enemy] play exclamation fx", category: "Action", id: "ac4f5610ba233f78cfa428aed4abfe83")]
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

