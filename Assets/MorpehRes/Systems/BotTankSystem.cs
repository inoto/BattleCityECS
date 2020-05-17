using System.Collections.Generic;
using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BotTankSystem))]
public sealed class BotTankSystem : UpdateSystem
{
    Filter fBots, fEagles;

    public override void OnAwake()
    {
        fBots = World.Filter
            .With<BotTankComponent>()
            .With<CollidableComponent>()
            .With<PositionComponent>();

        fEagles = World.Filter
            .With<EagleComponent>()
            .With<PositionComponent>()
            .With<OwnerComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        var bots = fBots.Select<BotTankComponent>();
        var owners = fBots.Select<OwnerComponent>();
        var collidables = fBots.Select<CollidableComponent>();
        var positions = fBots.Select<PositionComponent>();

        for (int i = 0; i < fBots.Length; i++)
        {
            ref var bot = ref bots.GetComponent(i);
            ref var owner = ref owners.GetComponent(i);
            ref var collidable = ref collidables.GetComponent(i);
            ref var position = ref positions.GetComponent(i);

            bot.Fired = false;
            if (Random.Range(0, 100) <= bot.FireChance * 100)
            {
                if (owner.Player == 1 && position.Position.y > -10
                    || owner.Player == 2 && position.Position.y < 10)
                    bot.Fired = true;
            }

            // Vector2 eaglePosition;
            // var ownerSositions = fEagles.Select<PositionComponent>();
            // var eagleOwners = fEagles.Select<OwnerComponent>();
            // for (int j = 0; j < fEagles.Length; j++)
            // {
            //     ref var eagleOwner = ref eagleOwners.GetComponent(j);
            //     if (eagleOwner.Player == owner.Player)
            //         continue;
            //
            //     ref var ownerPosition = ref ownerSositions.GetComponent(j);
            //     eaglePosition = ownerPosition.Position;
            // }

            if (collidable.Others.Count > 0 && !collidable.Others.Contains(null)
                && bot.CollidedTimer > 1f)
            {
                bot.CollidedTimer = 0f;
                bot.Direction = RandomDirectionButNot(-bot.Direction);
                bot.RotationDirection = bot.Direction;
            }
            if (bot.RotateChance > 0f && Random.Range(0, 100) < bot.RotateChance * 100)
            {
                bot.Direction = RandomDirectionButNot(-bot.Direction);
                bot.RotationDirection = bot.Direction;
            }

            bot.CollidedTimer += deltaTime;
        }
    }

    Vector2 RandomDirectionButNot(Vector2 dir)
    {
        List<Vector2> allPossible = new List<Vector2>(4);
        allPossible.Add(Vector2.up);
        allPossible.Add(Vector2.down);
        allPossible.Add(Vector2.left);
        allPossible.Add(Vector2.right);
        allPossible.Remove(dir);

        return allPossible[Random.Range(0, 2)];
    }
}