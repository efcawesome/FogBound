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
    class FogSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Sword of the Foggy Menace");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 38;
            item.melee = true;
            item.damage = 125;
            item.useTime = 6;
            item.useAnimation = 6;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 4f;
            item.value = Item.buyPrice(gold: 4);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.alpha = 125;
        }
    }
}
