using Assets.MorpehRes.Data.Creator;
using Morpeh;
using Photon.Pun;
using SimpleBattleCity;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HealthSystem))]
public sealed class HealthSystem : UpdateSystem
{
    [SerializeField] CreatorSystem creator;

    Filter fAll, fDamaged, fProjectiles, fPlayers, fBots;

    public override void OnAwake()
    {
        fAll = World.Filter
            .With<HealthComponent>()
            .With<ViewComponent>();

        fProjectiles = World.Filter
            .With<HealthComponent>()
            .With<ProjectileComponent>()
            .With<CollidableComponent>();

        fPlayers = World.Filter
            .With<InputComponent>()
            .With<HealthComponent>()
            .With<OwnerComponent>();

        fBots = World.Filter
            .With<BotTankComponent>()
            .With<HealthComponent>()
            .With<OwnerComponent>();

        fDamaged = World.Filter
            .With<HealthComponent>()
            .With<CollidableComponent>()
            .Without<ProjectileComponent>();

        foreach (var entity in fAll)
        {
            ref var health = ref entity.GetComponent<HealthComponent>();
            health.Health = health.HealthMax;
            health.IsInitialized = true;
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        CheckProjectiles();
        CheckPlayers();
        CheckBots();
    }

    void CheckProjectiles()
    {
        foreach (var entity in fProjectiles)
        {
            ref var health = ref entity.GetComponent<HealthComponent>();
            ref var projectile = ref entity.GetComponent<ProjectileComponent>();
            ref var collidable = ref entity.GetComponent<CollidableComponent>();

            if (!health.IsInitialized)
            {
                health.Health = health.HealthMax;
                health.IsInitialized = true;
            }

            foreach (var other in collidable.Others)
            {
                var provider = other.GetComponent<HealthProvider>();
                if (provider == null)
                    continue;

                ref var healthOther = ref provider.Entity.GetComponent<HealthComponent>();

                if (projectile.Damage >= healthOther.MinDamageToTrigger
                && !provider.Entity.Has<InvulnerableComponent>())
                {
                    healthOther.Health -= projectile.Damage;
                }
            }

            if (collidable.Others.Count > 0)
            {
                health.Health = 0;
            }
        }
    }

    void CheckPlayers()
    {
        foreach (var entity in fPlayers)
        {
            ref var health = ref entity.GetComponent<HealthComponent>();
            ref var owner = ref entity.GetComponent<OwnerComponent>();

            if (!health.IsInitialized)
            {
                health.Health = health.HealthMax;
                health.IsInitialized = true;
            }

            if (health.Health <= 0)
            {
                CreatorPlayerData data = new CreatorPlayerData();
                data.Owner.Player = owner.Player;

                creator.Players.Enqueue(data);

                if (!PhotonNetwork.IsConnected)
                {
                    GameController.Instance.PlayerDiedSinglePlayer();
                }
            }
        }
    }

    void CheckBots()
    {
        foreach (var entity in fBots)
        {
            ref var health = ref entity.GetComponent<HealthComponent>();
            ref var owner = ref entity.GetComponent<OwnerComponent>();

            if (!health.IsInitialized)
            {
                health.Health = health.HealthMax;
                health.IsInitialized = true;
            }

            if (health.Health <= 0)
            {
                CreatorBotData data = new CreatorBotData();
                data.Owner = owner.Player;

                creator.Bots.Enqueue(data);
            }
        }
    }
}