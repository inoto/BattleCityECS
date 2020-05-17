using Morpeh;
using Morpeh.Globals;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EagleSystem))]
public sealed class EagleSystem : UpdateSystem
{
    [SerializeField] GlobalEventInt GameOverEvent;

    Filter fEagles;

    public override void OnAwake()
    {
        fEagles = World.Filter
            .With<EagleComponent>()
            .With<HealthComponent>()
            .With<OwnerComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        var healths = fEagles.Select<HealthComponent>();
        var owners = fEagles.Select<OwnerComponent>();

        for (int i = 0; i < fEagles.Length; i++)
        {
            ref var health = ref healths.GetComponent(i);
            ref var owner = ref owners.GetComponent(i);

            if (health.Health <= 0)
            {
                Debug.Log("eagle health 0");
                GameOverEvent.Publish(owner.Player);
            }
        }
    }
}