using Assets.MorpehRes.Data.Creator;
using Morpeh;
using Morpeh.Globals;
using SimpleBattleCity;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WeaponSystem))]
public sealed class WeaponSystem : UpdateSystem
{
    [SerializeField] CreatorSystem Creator;
    Filter fPlayers, fProjectiles, fBots;

    float shootTimer = 0f;

    public override void OnAwake()
    {
        fPlayers = World.Filter
            .With<InputComponent>()
            .With<WeaponComponent>()
            .With<PositionComponent>()
            .With<OwnerComponent>();

        fBots = World.Filter
            .With<BotTankComponent>()
            .With<WeaponComponent>()
            .With<PositionComponent>()
            .With<OwnerComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        UpdatePlayers(deltaTime);
        UpdateBots(deltaTime);
    }

    void UpdatePlayers(float deltaTime)
    {
        var inputs = fPlayers.Select<InputComponent>();
        var weapons = fPlayers.Select<WeaponComponent>();
        var positions = fPlayers.Select<PositionComponent>();
        var owners = fPlayers.Select<OwnerComponent>();

        for (int i = 0; i < fPlayers.Length; i++)
        {
            ref var input = ref inputs.GetComponent(i);
            ref var weapon = ref weapons.GetComponent(i);
            ref var position = ref positions.GetComponent(i);
            ref var owner = ref owners.GetComponent(i);

            if (input.Fired)
            {
                input.Fired = false;
                if (weapon.ShootTimer >= weapon.Rate)
                {
                    weapon.ShootTimer = 0f;

                    var data = new CreatorProjectileData();
                    data.Projectile.Direction = input.RotationDirection;
                    data.Projectile.StartPosition = position.Position;
                    data.Owner.Player = owner.Player;

                    Creator.Projectiles.Enqueue(data);
                }
            }

            if (weapon.ShootTimer <= weapon.Rate)
                weapon.ShootTimer += deltaTime;
        }
    }

    void UpdateBots(float deltaTime)
    {
        var bots = fBots.Select<BotTankComponent>();
        var weapons = fBots.Select<WeaponComponent>();
        var positions = fBots.Select<PositionComponent>();
        var owners = fBots.Select<OwnerComponent>();

        for (int i = 0; i < fBots.Length; i++)
        {
            ref var bot = ref bots.GetComponent(i);
            ref var weapon = ref weapons.GetComponent(i);
            ref var position = ref positions.GetComponent(i);
            ref var owner = ref owners.GetComponent(i);

            if (bot.Fired)
            {
                if (weapon.ShootTimer >= weapon.Rate)
                {
                    weapon.ShootTimer = 0f;

                    var data = new CreatorProjectileData();
                    data.Projectile.Direction = bot.RotationDirection;
                    data.Projectile.StartPosition = position.Position;
                    data.Owner.Player = owner.Player;

                    Creator.Projectiles.Enqueue(data);
                }
            }

            if (weapon.ShootTimer <= weapon.Rate)
                weapon.ShootTimer += deltaTime;
        }
    }
}