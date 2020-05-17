using Morpeh;
using Morpeh.Globals;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ProjectileSystem))]
public sealed class ProjectileSystem : UpdateSystem
{
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

            if (projectile.LifeTimer <= projectile.LifeTime)
                projectile.LifeTimer += deltaTime;
        }
    }
}