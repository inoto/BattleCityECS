using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(AABBSystem))]
public sealed class AABBSystem : UpdateSystem
{
    Filter fAll;

    public override void OnAwake() {
        fAll = World.Filter
            .With<CollidableComponent>()
            .With<PositionComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        var collidables = fAll.Select<CollidableComponent>();
        var positions = fAll.Select<PositionComponent>();

        for (int i = 0; i < fAll.Length; i++)
        {
            ref var collidable = ref collidables.GetComponent(i);
            ref var position = ref positions.GetComponent(i);

            collidable.Collider.Bounds.center = position.Position;
        }
    }
}