using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BotTankSystem))]
public sealed class BotTankSystem : UpdateSystem
{
    Filter fBots;

    public override void OnAwake()
    {
        fBots = World.Filter
            .With<BotTankComponent>()
            .With<CollidableComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        var bots = fBots.Select<BotTankComponent>();
        var weapons = fBots.Select<WeaponComponent>();
        var collidables = fBots.Select<CollidableComponent>();

        for (int i = 0; i < fBots.Length; i++)
        {
            ref var bot = ref bots.GetComponent(i);
            ref var weapon = ref weapons.GetComponent(i);
            ref var collidable = ref collidables.GetComponent(i);

            bot.Fired = false;
            if (Random.Range(0, 100) <= bot.FireChance * 100)
            {
                bot.Fired = true;
            }

            if (collidable.Others.Count > 0)
            {
                if (bot.RotationDirection == Vector2.zero)
                {
                    bot.Direction = Vector2.down;
                }
                else if (bot.RotationDirection == Vector2.down)
                {
                    bot.Direction = Vector2.left;
                }
                else if (bot.RotationDirection == Vector2.left)
                {
                    bot.Direction = Vector2.up;
                }
                else if (bot.RotationDirection == Vector2.up)
                {
                    bot.Direction = Vector2.right;
                }
                else if (bot.RotationDirection == Vector2.right)
                {
                    bot.Direction = Vector2.down;
                }
                bot.RotationDirection = bot.Direction;
            }
            else if (Random.Range(0, 100) <= bot.RotateChance * 100)
            {
                if (bot.RotationDirection == Vector2.zero)
                {
                    bot.Direction = Random.Range(0, 1) == 0 ? Vector2.down : Vector2.up;
                }
                else if (bot.RotationDirection == Vector2.down)
                {
                    bot.Direction = Random.Range(0, 1) == 0 ? Vector2.left : Vector2.right;
                    // bot.RotationDirection = Vector2.left;
                }
                else if (bot.RotationDirection == Vector2.left)
                {
                    bot.Direction = Random.Range(0, 1) == 0 ? Vector2.down : Vector2.up;
                    // bot.RotationDirection = Vector2.up;
                }
                else if (bot.RotationDirection == Vector2.up)
                {
                    bot.Direction = Random.Range(0, 1) == 0 ? Vector2.left : Vector2.right;
                    // bot.RotationDirection = Vector2.right;
                }
                else if (bot.RotationDirection == Vector2.right)
                {
                    bot.Direction = Random.Range(0, 1) == 0 ? Vector2.down : Vector2.up;
                    // bot.RotationDirection = Vector2.down;
                }
                bot.RotationDirection = bot.Direction;
            }
        }
    }
}