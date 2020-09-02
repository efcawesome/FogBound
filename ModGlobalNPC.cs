using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FogBound
{
    class ModGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (NPC.downedGolemBoss)
            {
                if (Main.rand.Next(2) == 0)
                {
                    if (npc.type == NPCID.WyvernHead)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Fog"));
                    }
                }
            }
        }
    }
}
