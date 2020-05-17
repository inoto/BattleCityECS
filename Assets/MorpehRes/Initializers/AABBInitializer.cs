﻿using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(AABBInitializer))]
public sealed class AABBInitializer : Initializer
{
    Filter fAll;

    public override void OnAwake()
    {
        fAll = World.Filter
            .With<CollidableComponent>()
            .With<PositionComponent>();

        InitAll();
    }

    void InitAll()
    {
        var collidables = fAll.Select<CollidableComponent>();
        var positions = fAll.Select<PositionComponent>();

        for (int i = 0; i < fAll.Length; i++)
        {

            ref var collidable = ref collidables.GetComponent(i);
            ref var position = ref positions.GetComponent(i);

            collidable.Collider.Bounds.center = position.Position;
        }
    }

    public override void Dispose() {
    }
}