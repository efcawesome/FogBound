using Terraria;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FogBound
{
    public class FogBoundNpcsWorld : ModWorld
    {
        public static bool DownedFogBoundBoss = false;

        public override void Initialize()
        {
            DownedFogBoundBoss = false;
        }

        public override TagCompound Save()
        {
            var Downed = new List<string>();
            if (DownedFogBoundBoss) Downed.Add("fogboundBoss");

            return new TagCompound
            {
                {
                    "Version", 0
                },
                {
                    "Downed", Downed
                }
            };
        }

        public override void Load(TagCompound tag)
        {
            var Downed = tag.GetList<string>("Downed");
            DownedFogBoundBoss = Downed.Contains("fogboundBoss");
        }

        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            if(loadVersion == 0)
            {
                BitsByte flags = reader.ReadByte();
                DownedFogBoundBoss = flags[0];
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = DownedFogBoundBoss;

            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            DownedFogBoundBoss = flags[0];
        }
    }
}
