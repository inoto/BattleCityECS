using System.Collections.Generic;
using Morpeh;
using SimpleBattleCity;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct CollidableComponent : IComponent
{
    public AABB Collider;
    public List<AABB> Others;
}