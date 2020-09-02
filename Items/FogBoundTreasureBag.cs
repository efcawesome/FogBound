using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FogBound.Items
{
    class FogBoundTreasureBag : ModItem
    {
        public override int BossBagNPC => mod.NPCType("FogBoundBoss");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("<right> to open");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 999;
            item.consumable = true;
            item.rare = ItemRarityID.Expert;
            item.expert = true;
        }

        public override void OpenBossBag(Player player)
        {
            player.QuickSpawnItem(ItemID.LunarOre, 1);
            player.QuickSpawnItem(ItemID.GoldCoin, 50);
            player.QuickSpawnItem(ItemID.GreaterHealingPotion, Main.rand.Next(5, 10));
            int FogBoundWeapon = Main.rand.Next( 3);
            if (FogBoundWeapon == 0)
            {
                player.QuickSpawnItem(mod.ItemType("FogSword"));
            }
            else if (FogBoundWeapon == 1)
            {
                player.QuickSpawnItem(mod.ItemType("FoggyPrism"));
            }
            else if (FogBoundWeapon == 2)
            {
                player.QuickSpawnItem(mod.ItemType("FogBow"));
            }
        }
    }
}
