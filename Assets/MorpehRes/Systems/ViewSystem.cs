using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ViewSystem))]
public sealed class ViewSystem : UpdateSystem
{
    Filter f;
    
    public override void OnAwake()
    {
        f = World.Filter
            .With<HealthComponent>()
            .With<ViewComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var entity in f)
        {
            ref var health = ref entity.GetComponent<HealthComponent>();
            ref var view = ref entity.GetComponent<ViewComponent>();

            if (health.Health <= 0)
            {
                Destroy(view.Gameobject);
                // World.RemoveEntity(entity);
            }
        }
    }
}