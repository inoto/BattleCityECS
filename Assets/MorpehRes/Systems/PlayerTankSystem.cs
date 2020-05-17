using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PlayerTankSystem))]
public sealed class PlayerTankSystem : UpdateSystem
{
    Filter fPlayers;
    public override void OnAwake()
    {
        fPlayers = World.Filter
            .With<PlayerTankComponent>()
            .With<InputComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var players = fPlayers.Select<PlayerTankComponent>();
        var inputs = fPlayers.Select<InputComponent>();

        for (int i = 0; i < fPlayers.Length; i++)
        {
            ref var player = ref players.GetComponent(i);
            ref var input = ref inputs.GetComponent(i);

            player.Direction = input.Direction;
        }
    }
}