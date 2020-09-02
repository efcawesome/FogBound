using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace FogBound.Projectiles
{
    class FogArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fog Arrow");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 32;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 10;
            projectile.timeLeft = 600;
            projectile.alpha = 125;
            projectile.light = 0.5f;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.extraUpdates = 0;
            aiType = ProjectileID.Bullet;
        }
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
        }
    }
}
