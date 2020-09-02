using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace FogBound.Projectiles
{
    class FogDart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fog Dart");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 42;
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 500;
            projectile.tileCollide = false;
            projectile.hostile = true;
            projectile.scale = 1.2f;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            projectile.velocity.Y += projectile.ai[0];
        }
    }
}
