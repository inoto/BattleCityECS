using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TransformSystem))]
public sealed class TransformSystem : UpdateSystem
{
    Filter filter, fPlayers, fProjectiles, fBots;

    public override void OnAwake()
    {
        fPlayers = World.Filter
            .Without<ProjectileComponent>()
            .With<InputComponent>()
            .With<TransformComponent>();

        fBots = World.Filter
            .Without<ProjectileComponent>()
            .With<BotTankComponent>()
            .With<TransformComponent>();

        fProjectiles = World.Filter
            .With<ProjectileComponent>()
            .With<TransformComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        UpdatePlayers();
        UpdateBots();
        UpdateProjectiles();
    }

    void UpdatePlayers()
    {
        var inputs = fPlayers.Select<InputComponent>();
        var positions = fPlayers.Select<PositionComponent>();
        var transforms = fPlayers.Select<TransformComponent>();

        for (int i = 0; i < fPlayers.Length; i++)
        {
            ref var input = ref inputs.GetComponent(i);
            ref var position = ref positions.GetComponent(i);
            ref var transform = ref transforms.GetComponent(i);

            transform.Transform.position = position.Position;
            transform.Transform.rotation = DirectionToQuaternion(input.RotationDirection, -90);
        }
    }

    void UpdateBots()
    {
        var bots = fBots.Select<BotTankComponent>();
        var positions = fBots.Select<PositionComponent>();
        var transforms = fBots.Select<TransformComponent>();

        for (int i = 0; i < fBots.Length; i++)
        {
            ref var bot = ref bots.GetComponent(i);
            ref var position = ref positions.GetComponent(i);
            ref var transform = ref transforms.GetComponent(i);

            transform.Transform.position = position.Position;
            transform.Transform.rotation = DirectionToQuaternion(bot.RotationDirection, 90);
        }
    }

    void UpdateProjectiles()
    {
        var projectiles = fProjectiles.Select<ProjectileComponent>();
        var positions = fProjectiles.Select<PositionComponent>();
        var transforms = fProjectiles.Select<TransformComponent>();

        for (int i = 0; i < fProjectiles.Length; i++)
        {
            ref var projectile = ref projectiles.GetComponent(i);
            ref var position = ref positions.GetComponent(i);
            ref var transform = ref transforms.GetComponent(i);

            transform.Transform.position = position.Position;
            transform.Transform.rotation = DirectionToQuaternion(projectile.Direction, -180);
        }
    }

    Quaternion DirectionToQuaternion(Vector2 dir, float spriteAngle = 0)
    {
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle + spriteAngle, Vector3.forward);
    }

    // get direction from sprite
}