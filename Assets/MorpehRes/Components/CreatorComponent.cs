using System.Collections.Generic;
using Assets.MorpehRes.Data.Creator;
using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct CreatorComponent : IComponent
{
    public List<CreatorProjectileData> Projectiles;
    public List<CreatorPlayerData> Players;
}