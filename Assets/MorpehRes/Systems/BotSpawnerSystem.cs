using System.Collections.Generic;
using Assets.MorpehRes.Data.Creator;
using Morpeh;
using SimpleBattleCity;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Analytics;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BotSpawnerSystem))]
public sealed class BotSpawnerSystem : UpdateSystem
{
    [SerializeField] int numberOfAliveAllowed;
    [SerializeField] float rate = 5f;
    [SerializeField] CreatorSystem creator;

    Filter fSpawners;

    public override void OnAwake()
    {
        fSpawners = World.Filter
            .With<SpawnerComponent>()
            .With<PositionComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in fSpawners)
        {
            ref var spawner = ref entity.GetComponent<SpawnerComponent>();
            if (spawner.PlayerSpawner)
                continue;

            ref var owner = ref entity.GetComponent<OwnerComponent>();

            if (spawner.Timer >= rate && spawner.NumberOfAlive < numberOfAliveAllowed)
            {
                spawner.Timer = 0;
                CreatorBotData data = new CreatorBotData();
                data.Owner = owner.Player;
                creator.Bots.Enqueue(data);
                spawner.NumberOfAlive += 1;
            }

            if (spawner.Timer <= rate)
                spawner.Timer += deltaTime;
        }
    }
}