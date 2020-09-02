using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Terraria.Localization;

namespace FogBound.NPCs.Bosses
{
    [AutoloadBossHead]
    public class FogBoundBoss : ModNPC
    {
        private int ai;
        private int attackTimer = 0;
        private bool fastSpeed = false;

        private bool stunned;
        private int stunnedTimer;

        private int frame = 0;
        private double counting;

        private int copyTimer = 0;
        private bool hasTalked = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The FogBound");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.width = 190;
            npc.height = 398;

            npc.boss = true;
            npc.aiStyle = -1;
            npc.npcSlots = 5f;

            npc.lifeMax = 65000;
            npc.damage = 80;
            npc.defense = 40;
            npc.knockBackResist = 0f;

            npc.value = Item.buyPrice(gold: 50);

            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.noGravity = true;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            music = MusicID.Boss2;

            bossBag = mod.ItemType("FogBoundTreasureBag");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * bossLifeScale);
            npc.damage = (int)(npc.damage * 1.3f);
        }

        public override void AI()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            Vector2 target = npc.HasPlayerTarget ? player.Center : Main.npc[npc.target].Center;

            npc.rotation = 0.0f;
            npc.netAlways = true;
            npc.TargetClosest(true);

            if (hasTalked == false)
            {
                Talk("You've awoken me... time to pay the price...");
                hasTalked = true;
            }

            if (npc.life >= npc.lifeMax)
                npc.life = npc.lifeMax;

            copyTimer++;

            if (npc.target < 0 || npc.target == 255 || player.dead || !player.active || Main.dayTime)
            {
                npc.TargetClosest(false);
                npc.direction = 1;
                npc.velocity.Y = npc.velocity.Y - 0.1f;
                if(npc.timeLeft > 20)
                {
                    Main.NewText("Try harder next time", 255, 0, 0);
                    npc.timeLeft = 20;
                    return;
                }
            }

            ai++;

            if (copyTimer >= 1200)
            {
                NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-1000, 1000), (int)npc.Center.Y + Main.rand.Next(-1000, 1000), mod.NPCType("FogBoundCopy"), npc.whoAmI);
                copyTimer = 0;
            }

            if (npc.life <= npc.lifeMax/2)
            {
                if (Main.dayTime)
                {
                    npc.TargetClosest(false);
                    npc.direction = 1;
                    npc.velocity.Y = npc.velocity.Y - 0.1f;
                    if (npc.timeLeft > 20)
                    {
                        npc.timeLeft = 20;
                        return;
                    }
                }
                else
                {
                    Main.npc[(int)npc.ai[0]].active = false;
                }
            }


            npc.ai[0] = (float)ai * 1f;
            int distance = (int)Vector2.Distance(target, npc.Center);
            if((double)npc.ai[0] < 450)
            {
                frame = 0;
                MoveTowards(npc, target, (float)(distance > 300f ? 20f : 12f), 30f);
                npc.netUpdate = true;


            } 
            else if((double)npc.ai[0] >= 450.0)
            {
                frame = 0;
                stunned = false;
                npc.damage = 80;
                npc.defense = 40;
                if(!fastSpeed)
                {
                    fastSpeed = true;
                } else
                {
                    if((double)npc.ai[0] % 50 == 0)
                    {
                        float speed = 20f;
                        Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float x = player.position.X + (float)(player.width / 2) - vector.X;
                        float y = player.position.Y + (float)(player.height / 2) - vector.Y;
                        float distance2 = (float)Math.Sqrt(x * x + y * y);
                        float factor = speed / distance2;
                        npc.velocity.X = x * factor;
                        npc.velocity.Y = y * factor;
                    }
                }
                npc.netUpdate = true;
            }

            else
            {
                copyTimer = 0;
            }

            if ((double)npc.ai[0] >= 650.0)
            {
                ai = 0;
                npc.alpha = 0;
                fastSpeed = false;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (frame == 0)
            {
                counting += 1.0;
                if (counting < 8.0)
                {
                    npc.frame.Y = 0;
                }
                else if (counting < 16.0)
                {
                    npc.frame.Y = frameHeight;
                }
                else
                {
                    counting = 0.0;
                }
            }
            else if (frame == 1)
            {
                npc.frame.Y = 0;
            }
        }

        private void MoveTowards(NPC npc, Vector2 playerTarget, float speed, float turnResistance)
        {
            var move = playerTarget - npc.Center;
            float length = move.Length();
            if(length > speed)
            {
                move *= speed / length;
            }
            move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
            length = move.Length();
            if(length > speed)
            {
                move *= speed / length;
            }
            npc.velocity = move;
        }

        private void Talk(string message)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                string text = Language.GetTextValue("You've awoken me... time to pay the price...", Lang.GetNPCNameValue(npc.type), message);
                Main.NewText(text, 255, 0, 0);
            }
            else
            {
                NetworkText text = NetworkText.FromKey("You've awoken me... time to pay the price...", Lang.GetNPCNameValue(npc.type), message);
                NetMessage.BroadcastChatMessage(text, new Color(255, 0, 0));
            }
        }

        public override void NPCLoot()
        {
            FogBoundNpcsWorld.DownedFogBoundBoss = true;
            if(Main.expertMode)
            {
                npc.DropBossBags();
            } else
            {
                int FogBoundWeapon = Main.rand.Next(3);
                if (FogBoundWeapon == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FogSword"), 1);
                }
                else if (FogBoundWeapon == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FoggyPrism"), 1);
                }
                else if (FogBoundWeapon == 2)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FogBow"), 1);
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if(!Main.expertMode)
            {
                target.AddBuff(BuffID.Darkness, 600, false);
                target.AddBuff(BuffID.Obstructed, 60, false);
                int BlackoutChance = Main.rand.Next(3);
                if(BlackoutChance == 0)
                {
                    target.AddBuff(BuffID.Blackout, 300, false);
                }
            } else if(Main.expertMode)
            {
                target.AddBuff(BuffID.Darkness, 1200, false);
                target.AddBuff(BuffID.Obstructed, 60, false);
                target.AddBuff(BuffID.Blackout, 600, false);
            }
        }
    }
}
