using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(LastPositionSystem))]
public sealed class LastPositionSystem : UpdateSystem
{
    Filter fMovable;

    public override void OnAwake() {
        fMovable = World.Filter
            .With<PositionComponent>()
            .With<MoveComponent>()
            .With<CollidableComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        var positions = fMovable.Select<PositionComponent>();
        var collidables = fMovable.Select<CollidableComponent>();

        for (int i = 0; i < fMovable.Length; i++)
        {
            ref var collidable = ref collidables.GetComponent(i);
            if (collidable.Others.Count == 0)
                continue;

            ref var position = ref positions.GetComponent(i);

            if (collidable.Others.Count > 0 && position.IsInitialized)
            {
                position.Position = position.LastPosition;
            }
        }
    }
}