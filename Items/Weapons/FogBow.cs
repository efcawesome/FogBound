using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FogBound.Items.Weapons
{
    class FogBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Turns Arrows into Fog Arrows");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 38;
            item.ranged = true;
            item.damage = 60;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 2.5f;
            item.value = Item.buyPrice(gold: 4);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ProjectileID.WoodenArrowFriendly;
            item.shootSpeed = 17f;
            item.useAmmo = AmmoID.Arrow;
            item.alpha = 125;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("FogArrow"), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
