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
using Terraria.Localization;

namespace FogBound.NPCs.Bosses
{
    public class FogBoundCopy : ModNPC
    {
        private int ai;
        private int attackTimer = 0;
        private bool fastSpeed = false;

        private bool stunned;

        private int frame = 0;
        private double counting;
        
        private bool isPassive = true;
        private int passiveTimer = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FogBound Copy");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {

            npc.width = 190;
            npc.height = 398;

            npc.aiStyle = -1;
            npc.npcSlots = 5f;

            npc.lifeMax = 65000;
            npc.knockBackResist = 0f;
            npc.defense = 9999;
            npc.damage = 80;

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

            passiveTimer++;
            if (passiveTimer <= 600)
            {
                isPassive = true;
            }
            else if (passiveTimer >= 601 && passiveTimer <= 1200)
            {
                isPassive = false;
            }
            else
            {
                passiveTimer = 0;
            }

            if (npc.life >= npc.lifeMax)
                npc.life = npc.lifeMax;

            if(npc.target < 0 || npc.target == 255 || player.dead || !player.active || Main.dayTime)
            {
                npc.TargetClosest(false);
                npc.direction = 1;
                npc.velocity.Y = npc.velocity.Y - 0.1f;
                if(npc.timeLeft > 20)
                {
                    npc.timeLeft = 20;
                    return;
                }
            }

            ai++;

            npc.ai[0] = (float)ai * 1f;
            int distance = (int)Vector2.Distance(target, npc.Center);
            if((double)npc.ai[0] < 450)
            {
                if(isPassive == true)
                {
                    npc.damage = 0;
                    frame = 0;
                } 
                else if (isPassive == false) 
                {
                    npc.damage = 80;
                    frame = 1;
                }
                MoveTowards(npc, target, (float)(distance > 300f ? 20f : 12f), 30f);
                npc.netUpdate = true;
            } 
            else if((double)npc.ai[0] >= 450.0)
            {
                frame = 0;
                stunned = false;
                if(!fastSpeed)
                {
                    fastSpeed = true;
                } else
                {
                    if((double)npc.ai[0] % 50 == 0)
                    {
                        if(isPassive == true)
                        {
                            npc.damage = 0;
                            frame = 0;
                        }
                        else if(isPassive == false)
                        {
                            npc.damage = 80;
                            frame = 1;
                        }
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

            if(frame == 1)
            {
                counting += 1.0;
                if(counting < 8.0)
                {
                    npc.frame.Y = frameHeight * 2;
                }
                else if (counting < 16.0)
                {
                    npc.frame.Y = frameHeight * 3;
                }
                else
                {
                    counting = 0.0;
                }
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
