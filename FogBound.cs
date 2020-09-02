using Terraria.ID;
using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using FogBound.NPCs.Bosses;
using FogBound.Items;
using FogBound.Items.Weapons;

namespace FogBound
{
    
	public class FogBound : Mod
	{
        Mod bossChecklist;
        public override void PostSetupContent()
        {
            bossChecklist = ModLoader.GetMod("BossChecklist");
            if(bossChecklist != null)
            {
                bossChecklist.Call
                    (
                        "AddBoss",
                        11.5f,
                        ModContent.NPCType<FogBoundBoss>(),
                        this,
                        "The FogBound",
                        (Func<bool>)(() => FogBoundNpcsWorld.DownedFogBoundBoss),
                        ModContent.ItemType<FoggyMirror>(),
                        new List<int> { },
                        new List<int> { ModContent.ItemType<FogBow>(), ModContent.ItemType<FogSword>(), ModContent.ItemType<FoggyPrism>(), ModContent.ItemType<FogBoundTreasureBag>() },
                        "Spawn by using a [i:" + ModContent.ItemType<FoggyMirror>() + "] at night.",
                        "The FogBound got too foggy!",
                        "FogBound/NPCs/Bosses/FogBoundBoss",
                        "FogBound/NPCs/Bosses/FogBoundBoss_Head_Boss"
                    );
            }
        }
    }
}