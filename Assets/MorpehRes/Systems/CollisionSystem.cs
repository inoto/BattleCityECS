using System;
using System.Collections.Generic;
using Morpeh;
using SimpleBattleCity;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CollisionSystem))]
public sealed class CollisionSystem : UpdateSystem
{
    Filter fAll, fPlayerTank, fHealth, fBlocker, fProjectiles, fSpawners;
    Filter fBlockerProjectileAndOwner, fBlockerProjectilesEnvironment, fForest, fBlockerTank, fEagle, fBots;

    public override void OnAwake()
    {
        fAll = World.Filter
            .With<CollidableComponent>();

        fPlayerTank = World.Filter
            .With<CollidableComponent>()
            .With<InputComponent>();

        fBots = World.Filter
            .With<CollidableComponent>()
            .With<BotTankComponent>();

        fBlockerTank = World.Filter
            .With<CollidableComponent>()
            .With<BlockerTankComponent>();

        fProjectiles = World.Filter
            .With<CollidableComponent>()
            .With<ProjectileComponent>()
            .With<HealthComponent>();

        fBlockerProjectileAndOwner = World.Filter
            .With<CollidableComponent>()
            .With<BlockerProjectileComponent>()
            .With<OwnerComponent>();

        fBlockerProjectilesEnvironment = World.Filter
            .With<CollidableComponent>()
            .With<BlockerProjectileComponent>()
            .Without<OwnerComponent>()
            .With<BlockerTankComponent>();

        fForest = World.Filter
            .With<CollidableComponent>()
            .With<BlockerProjectileComponent>()
            .Without<BlockerTankComponent>()
            .Without<OwnerComponent>()
            .With<HealthComponent>();

        fHealth = World.Filter
            .With<CollidableComponent>()
            .With<HealthComponent>()
            .Without<ProjectileComponent>()
            .Without<InputComponent>();

        fSpawners = World.Filter
            .With<CollidableComponent>()
            .With<SpawnerComponent>();

        Init();
    }

    void Init()
    {
        var collidables = fAll.Select<CollidableComponent>();

        for (int i = 0; i < fAll.Length; i++)
        {
            ref var collidable = ref collidables.GetComponent(i);

            collidable.Others = new List<AABB>(10);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        CheckTanksAgainstBlockers();
        // CheckSpawners();
        CheckProjectiles();
        // CheckOthers();
    }

    void CheckTanksAgainstBlockers()
    {
        foreach (var entity in fPlayerTank)
        {
            ref var collidable = ref entity.GetComponent<CollidableComponent>();
            ref var input = ref entity.GetComponent<InputComponent>();

            foreach (var entityOther in fBlockerTank)
            {
                if (entity.ID == entityOther.ID)
                    continue;

                ref var other = ref entityOther.GetComponent<CollidableComponent>();

                if (collidable.Collider.Bounds.Intersects(other.Collider.Bounds))
                {
                    if (!collidable.Others.Contains(other.Collider))
                    {
                        collidable.Others.Add(other.Collider);
                    }
                }
                else
                {
                    if (collidable.Others.Contains(other.Collider))
                    {
                        collidable.Others.Remove(other.Collider);
                    }
                }
            }
        }
        foreach (var entity in fBots)
        {
            ref var collidable = ref entity.GetComponent<CollidableComponent>();

            foreach (var entityOther in fBlockerTank)
            {
                if (entity.ID == entityOther.ID)
                    continue;

                ref var other = ref entityOther.GetComponent<CollidableComponent>();

                if (collidable.Collider.Bounds.Intersects(other.Collider.Bounds))
                {
                    if (!collidable.Others.Contains(other.Collider))
                    {
                        collidable.Others.Add(other.Collider);
                    }
                }
                else
                {
                    if (collidable.Others.Contains(other.Collider))
                    {
                        collidable.Others.Remove(other.Collider);
                    }
                }
            }
        }
    }

    void CheckProjectiles()
    {
        foreach (var entity in fProjectiles)
        {
            ref var collidable = ref entity.GetComponent<CollidableComponent>();
            ref var projectile = ref entity.GetComponent<ProjectileComponent>();
            ref var owner = ref entity.GetComponent<OwnerComponent>();

            foreach (var entityOther in fBlockerProjectileAndOwner)
            {
                if (entity.ID == entityOther.ID)
                    continue;
            
                ref var other = ref entityOther.GetComponent<CollidableComponent>();
                ref var otherOwner = ref entityOther.GetComponent<OwnerComponent>();

                if (collidable.Collider.Bounds.Intersects(other.Collider.Bounds))
                {
                    if (!collidable.Others.Contains(other.Collider) && owner.Player != otherOwner.Player)
                    {
                        collidable.Others.Add(other.Collider);
                    }
                }
                else
                {
                    if (collidable.Others.Contains(other.Collider) && owner.Player != otherOwner.Player)
                    {
                        collidable.Others.Remove(other.Collider);
                    }
                }
            }
            foreach (var entityOther in fBlockerProjectilesEnvironment)
            {
                if (entity.ID == entityOther.ID)
                    continue;

                ref var other = ref entityOther.GetComponent<CollidableComponent>();
                ref var healthOther = ref entityOther.GetComponent<HealthComponent>();

                if (collidable.Collider.Bounds.Intersects(other.Collider.Bounds))
                {
                    if (!collidable.Others.Contains(other.Collider))
                    {
                        collidable.Others.Add(other.Collider);
                    }
                }
                else
                {
                    if (collidable.Others.Contains(other.Collider))
                    {
                        collidable.Others.Remove(other.Collider);
                    }
                }
            }
            foreach (var entityOther in fForest)
            {
                if (entity.ID == entityOther.ID)
                    continue;

                ref var other = ref entityOther.GetComponent<CollidableComponent>();
                ref var healthOther = ref entityOther.GetComponent<HealthComponent>();

                if (collidable.Collider.Bounds.Intersects(other.Collider.Bounds))
                {
                    if (projectile.Damage >= healthOther.MinDamageToTrigger)
                    {
                        if (!collidable.Others.Contains(other.Collider))
                        {
                            collidable.Others.Add(other.Collider);
                        }
                    }
                }
                else
                {
                    if (projectile.Damage >= healthOther.MinDamageToTrigger)
                    {
                        if (collidable.Others.Contains(other.Collider))
                        {
                            collidable.Others.Remove(other.Collider);
                        }
                    }
                }
            }
        }
    }
}