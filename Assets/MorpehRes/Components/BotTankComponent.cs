using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct BotTankComponent : IComponent
{
    public Vector2 Direction;
    public Vector2 RotationDirection;
    public float RotateChance;
    public float FireChance;
    public bool Fired;
    public bool WithBonus;
}