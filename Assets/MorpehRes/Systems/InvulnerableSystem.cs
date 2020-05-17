using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InvulnerableSystem))]
public sealed class InvulnerableSystem : UpdateSystem
{
    Filter f;

    public override void OnAwake()
    {
        f = World.Filter
            .With<InvulnerableComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in f)
        {
            ref var invulnarable = ref entity.GetComponent<InvulnerableComponent>();

            if (invulnarable.Timer >= invulnarable.Duration)
            {
                entity.RemoveComponent<InvulnerableComponent>();
                continue;
            }

            if (invulnarable.Timer <= invulnarable.Duration)
                invulnarable.Timer += deltaTime;
        }
    }
}