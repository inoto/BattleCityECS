using System.Collections.Generic;
using Assets.MorpehRes.Data.Creator;
using Morpeh;
using Photon.Pun;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CreatorSystem))]
public sealed class CreatorSystem : UpdateSystem
{
    [SerializeField] bool EnablePool = false;

    public Queue<CreatorProjectileData> Projectiles;
    public Queue<CreatorPlayerData> Players;
    public Queue<CreatorBotData> Bots;
    [SerializeField] GameObject ProjectilePrefab;
    [SerializeField] GameObject PlayerTankPrefab;
    [SerializeField] GameObject BotTankPrefab;
    [SerializeField] Sprite[] PlayerTankSprites;
    [SerializeField] Sprite[] BotTankSprites;

    Filter fCreators, fSpawners;

    public override void OnAwake()
    {
        fCreators = World.Filter
            .With<CreatorComponent>();

        fSpawners = World.Filter
            .With<SpawnerComponent>()
            .With<PositionComponent>()
            .With<OwnerComponent>();

        Projectiles = new Queue<CreatorProjectileData>(10);
        Players = new Queue<CreatorPlayerData>(2);
        Bots = new Queue<CreatorBotData>(10);
    }

    public override void OnUpdate(float deltaTime)
    {
        CheckProjectiles();
        CheckPlayers();
        CheckBots();
    }

    void CheckProjectiles()
    {
        if (Projectiles.Count == 0)
            return;

        while (Projectiles.Count > 0)
        {
            var data = Projectiles.Dequeue();

            GameObject go = SpawnPrefab(ProjectilePrefab, Vector2.zero);

            var newEntity = go.GetComponent<ProjectileProvider>().Entity;

            ref var projectile = ref newEntity.GetComponent<ProjectileComponent>();
            projectile.Direction = data.Projectile.Direction;

            ref var projectileOwner = ref newEntity.GetComponent<OwnerComponent>();
            projectileOwner.Player = data.Owner.Player;

            go.transform.position = data.Projectile.StartPosition + projectile.Direction*1.8f;
        }
    }

    void CheckPlayers()
    {
        if (Players.Count == 0)
            return;

        while (Players.Count > 0)
        {
            var data = Players.Peek();

            GameObject go = SpawnPrefab(PlayerTankPrefab, Vector2.zero);
            go.GetComponent<SpriteRenderer>().sprite = PlayerTankSprites[data.Owner.Player-1];

            var newEntity = go.GetComponent<InputProvider>().Entity;

            foreach (var entitySpawner in fSpawners)
            {
                ref var spawner = ref entitySpawner.GetComponent<SpawnerComponent>();
                if (!spawner.PlayerSpawner)
                    continue;

                ref var ownerSpawner = ref entitySpawner.GetComponent<OwnerComponent>();
                if (data.Owner.Player != ownerSpawner.Player)
                    continue;

                ref var position = ref entitySpawner.GetComponent<PositionComponent>();
                go.transform.position = position.Position;

                ref var owner = ref newEntity.GetComponent<OwnerComponent>();
                owner.Player = data.Owner.Player;

                ref var bot = ref newEntity.GetComponent<InputComponent>();
                bot.RotationDirection = data.Owner.Player == 1 ? Vector2.up : Vector2.down;

                Players.Dequeue();
            }
        }
        
    }

    void CheckBots()
    {
        if (Bots.Count == 0)
            return;

        while (Bots.Count > 0)
        {
            var data = Bots.Peek();

            GameObject go = SpawnPrefab(BotTankPrefab, Vector2.zero);
            go.GetComponent<SpriteRenderer>().sprite = BotTankSprites[data.Owner-1];

            var newEntity = go.GetComponent<BotTankProvider>().Entity;

            foreach (var entitySpawner in fSpawners)
            {
                ref var spawner = ref entitySpawner.GetComponent<SpawnerComponent>();
                if (spawner.PlayerSpawner)
                    continue;

                ref var ownerSpawner = ref entitySpawner.GetComponent<OwnerComponent>();
                if (data.Owner != ownerSpawner.Player)
                    continue;

                ref var positionSpawner = ref entitySpawner.GetComponent<PositionComponent>();
                go.transform.position = positionSpawner.Position;

                ref var owner = ref newEntity.GetComponent<OwnerComponent>();
                owner.Player = data.Owner;

                ref var bot = ref newEntity.GetComponent<BotTankComponent>();
                bot.Direction = data.Owner == 1 ? Vector2.up : Vector2.down;
                bot.RotationDirection = bot.Direction;

                Bots.Dequeue();
            }
        }
    }

    GameObject SpawnPrefab(GameObject prefab, Vector2 position)
    {
        GameObject go;
        if (PhotonNetwork.IsConnected)
        {
            go = PhotonNetwork.Instantiate(prefab.name, position, Quaternion.identity);
        }
        else
        {
            go = Instantiate(prefab, position, Quaternion.identity);
        }

        return go;
    }
}