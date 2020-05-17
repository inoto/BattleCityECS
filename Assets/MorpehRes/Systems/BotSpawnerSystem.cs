using System.Collections.Generic;
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
    [SerializeField] List<GameObject> botTankPrefabs = new List<GameObject>();
    [SerializeField] StageController stage;
    [SerializeField] int numberOfAliveAllowed;

    Filter fSpawners;

    List<GameObject> aliveBots;

    public override void OnAwake()
    {
        fSpawners = World.Filter
            .With<BotSpawnerComponent>()
            .With<PositionComponent>();

        aliveBots = new List<GameObject>(5);
    }

    public override void OnUpdate(float deltaTime)
    {
        // if (stage.NumberOfFastTanks)

        CalculateLastTimes(deltaTime);

        if (aliveBots.Count < numberOfAliveAllowed)
            PrepareSpawn();
    }

    void CalculateLastTimes(float deltaTime)
    {
        foreach (var entity in fSpawners)
        {
            ref var spawner = ref entity.GetComponent<BotSpawnerComponent>();

            if (spawner.TimeSinceLastUse <= 10f)
                spawner.TimeSinceLastUse += deltaTime;
        }
    }

    void PrepareSpawn()
    {
        float minTimeSinceLastUse = float.MinValue;
        IEntity bestEntity = fSpawners.GetEntity(0);

        foreach (var entity in fSpawners)
        {
            ref var spawner = ref entity.GetComponent<BotSpawnerComponent>();

            if (spawner.TimeSinceLastUse > minTimeSinceLastUse)
            {
                minTimeSinceLastUse = spawner.TimeSinceLastUse;
                bestEntity = entity;
            }
        }

        ref var position = ref bestEntity.GetComponent<PositionComponent>();

        GameObject go = Instantiate(botTankPrefabs[Random.Range(0, botTankPrefabs.Count - 1)]);
        go.transform.position = position.Position;
        aliveBots.Add(go);
        // stage.NumberOfFastTanks -= 1;
    }
}