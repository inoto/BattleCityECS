using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PositionSystem))]
public sealed class PositionSystem : UpdateSystem
{
    Filter fAll, fPLayers, fProjectiles, fBots;

    public override void OnAwake()
    {
        fAll = World.Filter
            .With<PositionComponent>()
            .With<TransformComponent>();

        fPLayers = World.Filter
            .With<PositionComponent>()
            .With<InputComponent>()
            .With<MoveComponent>()
            .With<CollidableComponent>();

        fBots = World.Filter
            .With<PositionComponent>()
            .With<BotTankComponent>()
            .With<MoveComponent>()
            .With<CollidableComponent>();

        fProjectiles = World.Filter
            .With<ProjectileComponent>()
            .With<PositionComponent>()
            .With<MoveComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        CheckNotInitialized();
        UpdatePlayers(deltaTime);
        UpdateBots(deltaTime);
        UpdateProjectiles(deltaTime);
    }

    void CheckNotInitialized()
    {
        foreach (var entity in fAll)
        {
            ref var position = ref entity.GetComponent<PositionComponent>();
            ref var transform = ref entity.GetComponent<TransformComponent>();

            if (!position.IsInitialized)
            {
                position.Position = transform.Transform.position;
                position.LastPosition = transform.Transform.position;
                position.IsInitialized = true;
            }
        }
    }

    void UpdatePlayers(float deltaTime)
    {
        var positions = fPLayers.Select<PositionComponent>();
        var inputs = fPLayers.Select<InputComponent>();
        var moves = fPLayers.Select<MoveComponent>();

        for (int i = 0; i < fPLayers.Length; i++)
        {
            ref var input = ref inputs.GetComponent(i);
            if (input.Direction == Vector2.zero)
                continue;

            ref var position = ref positions.GetComponent(i);
            ref var move = ref moves.GetComponent(i);

            position.LastPosition = position.Position;
            position.Position += input.Direction.normalized * move.Speed * deltaTime;
        }
    }

    void UpdateBots(float deltaTime)
    {
        var positions = fBots.Select<PositionComponent>();
        var bots = fBots.Select<BotTankComponent>();
        var moves = fBots.Select<MoveComponent>();

        for (int i = 0; i < fBots.Length; i++)
        {
            ref var bot = ref bots.GetComponent(i);
            if (bot.Direction == Vector2.zero)
                continue;

            ref var position = ref positions.GetComponent(i);
            ref var move = ref moves.GetComponent(i);

            position.LastPosition = position.Position;
            position.Position += bot.Direction.normalized * move.Speed * deltaTime;
        }
    }

    void UpdateProjectiles(float deltaTime)
    {
        var positions = fProjectiles.Select<PositionComponent>();
        var projectiles = fProjectiles.Select<ProjectileComponent>();
        var moves = fProjectiles.Select<MoveComponent>();

        for (int i = 0; i < fProjectiles.Length; i++)
        {
            ref var position = ref positions.GetComponent(i);
            ref var projectile = ref projectiles.GetComponent(i);
            ref var move = ref moves.GetComponent(i);

            position.LastPosition = position.Position;
            position.Position += projectile.Direction.normalized * move.Speed * deltaTime;
        }
    }
}