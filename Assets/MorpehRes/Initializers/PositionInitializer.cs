using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(PositionInitializer))]
public sealed class PositionInitializer : Initializer
{
    Filter fAll;

    public override void OnAwake() {
        fAll = World.Filter
            .With<TransformComponent>()
            .With<PositionComponent>();

        InitAll();
    }

    void InitAll()
    {
        var transforms = fAll.Select<TransformComponent>();
        var positions = fAll.Select<PositionComponent>();

        for (int i = 0; i < fAll.Length; i++)
        {
            ref var transform = ref transforms.GetComponent(i);
            ref var position = ref positions.GetComponent(i);

            position.Position = transform.Transform.position;
            position.IsInitialized = true;
        }
    }

    public override void Dispose() {
    }
}