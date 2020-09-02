using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FogBound.Items
{
	public class FoggyMirror : ModItem
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("FoggyMirror"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Summons The FogBound <\b> Non Consumable");
		}

		public override void SetDefaults() 
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 1;
			item.rare = ItemRarityID.LightRed;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.consumable = false;
		}

        public override bool CanUseItem(Player player)
        {
			if(!Main.dayTime)
			{
				return !NPC.AnyNPCs(mod.NPCType("FogBoundBoss"));
			} else
            {
				return false;
            }
        }

        public override bool UseItem(Player player)
        {
			Main.PlaySound(SoundID.Roar, player.position);
			if(Main.netMode != NetmodeID.MultiplayerClient)
            {
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("FogBoundBoss"));
            }
			return true;
        }

        public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Fog"), 15);
			recipe.AddIngredient(ItemID.BeetleHusk, 5);
			recipe.AddIngredient(ItemID.MagicMirror);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(mod.ItemType("Fog"), 15);
			recipe1.AddIngredient(ItemID.BeetleHusk, 5);
			recipe1.AddIngredient(ItemID.IceMirror);
			recipe1.AddTile(TileID.MythrilAnvil);
			recipe1.SetResult(this);
			recipe1.AddRecipe();
		}
	}
}