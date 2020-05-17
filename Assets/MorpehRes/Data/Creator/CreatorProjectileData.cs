using System;

namespace Assets.MorpehRes.Data.Creator
{
    [Serializable]
    public class CreatorProjectileData
    {
        public ProjectileComponent Projectile = new ProjectileComponent();
        public OwnerComponent Owner = new OwnerComponent();
    }
}