using Morpeh;
using Morpeh.Globals;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEditor.Events;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ProjectileSystem))]
public sealed class ProjectileSystem : UpdateSystem
{
    public GlobalEvent CollidedEvent;

    Filter fProjectiles;

    public override void OnAwake()
    {
        fProjectiles = World.Filter
            .With<HealthComponent>()
            .With<ProjectileComponent>()
            .With<CollidableComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var entity in fProjectiles)
        {
            ref var projectile = ref entity.GetComponent<ProjectileComponent>();
            ref var health = ref entity.GetComponent<HealthComponent>();
            ref var collidable = ref entity.GetComponent<CollidableComponent>();

            if (!projectile.IsInitialized)
            {
                // projectile.CollidedEvent += OnCollidedEvent;
                projectile.IsInitialized = true;
            }

            if (projectile.LifeTimer <= projectile.LifeTime)
                projectile.LifeTimer += deltaTime;
        }
    }

    // void OnCollidedEvent(IEntity entity, GameObject go)
    // {
    //     ref var projectile = ref entity.GetComponent<ProjectileComponent>();
    //     projectile.CollidedEvent -= OnCollidedEvent;
    //     // Debug.Log("projectile collided IN SYSTEM");
    //     World.RemoveEntity(entity);
    // }
}