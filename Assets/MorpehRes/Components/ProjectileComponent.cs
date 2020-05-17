using System;
using Morpeh;
using Morpeh.Globals;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Events;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct ProjectileComponent : IComponent
{
    public bool IsInitialized;
    // public bool InPool;
    public Vector2 StartPosition;
    public Vector2 Direction;
    public int Damage;
    public float LifeTime;
    [HideInInspector] public float LifeTimer;
}