using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace FogBound.Items
{
    public class Fog : ModItem
    {
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 14;
			item.maxStack = 999;
		}
	}
}
