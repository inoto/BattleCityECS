using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.InputSystem;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InputSystem))]
public sealed class InputSystem : UpdateSystem {
    Filter filter;
    Keyboard kb;

    bool dirUp, dirLeft, dirDown, dirRight;

    public override void OnAwake()
    {
        filter = World.Filter
            .With<InputComponent>()
            .With<CollidableComponent>();

        kb = UnityEngine.InputSystem.InputSystem.GetDevice<Keyboard>();
    }

    public override void OnUpdate(float deltaTime) {
        var inputs = filter.Select<InputComponent>();
        var collidables = filter.Select<CollidableComponent>();

        for (int i = 0; i < filter.Length; i++)
        {
            ref var input = ref inputs.GetComponent(i);
            ref var collidable = ref collidables.GetComponent(i);

            if (collidable.Others.Count > 0)
            {
                input.Direction = Vector2.zero;
            }

            // pressed
            if (kb.wKey.wasPressedThisFrame)
            {
                input.Direction = Vector2.up;
                input.RotationDirection = input.Direction;
            }
            else if (kb.aKey.wasPressedThisFrame)
            {
                input.Direction = Vector2.left;
                input.RotationDirection = input.Direction;
            }
            else if (kb.sKey.wasPressedThisFrame)
            {
                input.Direction = Vector2.down;
                input.RotationDirection = input.Direction;
            }
            else if (kb.dKey.wasPressedThisFrame)
            {
                input.Direction = Vector2.right;
                input.RotationDirection = input.Direction;
            }
            // released
            if (kb.wKey.wasReleasedThisFrame)
            {
                if (input.Direction == Vector2.up)
                    input.Direction = Vector2.zero;
            }
            else if (kb.aKey.wasReleasedThisFrame)
            {
                if (input.Direction == Vector2.left)
                    input.Direction = Vector2.zero;
            }
            else if (kb.sKey.wasReleasedThisFrame)
            {
                if (input.Direction == Vector2.down)
                    input.Direction = Vector2.zero;
            }
            else if (kb.dKey.wasReleasedThisFrame)
            {
                if (input.Direction == Vector2.right)
                    input.Direction = Vector2.zero;
            }

            if (kb.spaceKey.isPressed)
                input.Fired = true;
        }
    }
}