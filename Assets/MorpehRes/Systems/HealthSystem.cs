using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HealthSystem))]
public sealed class HealthSystem : UpdateSystem {
    Filter fAll, fDamaged, fProjectiles;

    public override void OnAwake()
    {
        fAll = World.Filter
            .With<HealthComponent>()
            .With<ViewComponent>();

        fProjectiles = World.Filter
            .With<HealthComponent>()
            .With<ProjectileComponent>()
            .With<CollidableComponent>();

        fDamaged = World.Filter
            .With<HealthComponent>()
            .With<CollidableComponent>()
            .Without<ProjectileComponent>();

        foreach (var entity in fAll)
        {
            ref var health = ref entity.GetComponent<HealthComponent>();
            health.Health = health.HealthMax;
            health.IsInitialized = true;
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        // CheckDamaged();
        CheckProjectiles();
    }

    void CheckDamaged()
    {
        foreach (var entity in fDamaged)
        {
            ref var health = ref entity.GetComponent<HealthComponent>();
            ref var collidable = ref entity.GetComponent<CollidableComponent>();

            Debug.Log("check damaged");
            foreach (var other in collidable.Others)
            {
                // Debug.Log($"check {collidable.Collider.gameObject} with {other.gameObject}");
                var provider = other.GetComponent<ProjectileProvider>();
                if (provider == null)
                    continue;
                
                var entityOther = provider.Entity;

                if (entityOther.Has<ProjectileComponent>())
                { 
                    ref var projectile = ref entityOther.GetComponent<ProjectileComponent>();
                    // Debug.Log("projectile damaged brick");
                    if (projectile.Damage >= health.MinDamageToTrigger)
                    {
                        health.Health -= projectile.Damage;
                    }
                }
            }
        }
    }

    void CheckProjectiles()
    {
        foreach (var entity in fProjectiles)
        {
            ref var health = ref entity.GetComponent<HealthComponent>();
            ref var projectile = ref entity.GetComponent<ProjectileComponent>();
            ref var collidable = ref entity.GetComponent<CollidableComponent>();

            if (!health.IsInitialized)
            {
                health.Health = health.HealthMax;
                health.IsInitialized = true;
            }

            foreach (var other in collidable.Others)
            {
                // Debug.Log($"check {collidable.Collider.gameObject} with {other.gameObject}");
                var provider = other.GetComponent<HealthProvider>();
                if (provider == null)
                    continue;

                ref var healthOther = ref provider.Entity.GetComponent<HealthComponent>();

                if (projectile.Damage >= healthOther.MinDamageToTrigger)
                {
                    healthOther.Health -= projectile.Damage;
                }
            }

            if (collidable.Others.Count > 0)
            {
                health.Health = 0;
            }
        }
    }
}