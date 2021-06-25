using System.IO;
using Terraria.ModLoader;
using Terraria;
using Terraria.IO;
using Terraria.ID;

namespace BattleUtilities
{
    public class BattleUtilities : Mod
    {

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            BattleUtilitiesMessageType msgType = (BattleUtilitiesMessageType)reader.ReadByte();
            switch (msgType)
            {
                case BattleUtilitiesMessageType.SyncPlayer:
                    {
                        byte playernumber = reader.ReadByte();
                        BattleUtilitiesModPlayer BUPlayer = Main.player[playernumber].GetModPlayer<BattleUtilitiesModPlayer>();
                        bool bulletHomingEffect = reader.ReadBoolean();
                        bool arcaneHomingEffect = reader.ReadBoolean();
                        bool btdCharged = reader.ReadBoolean();
                        bool atdCharged = reader.ReadBoolean();
                        BUPlayer.bulletHomingEffect = bulletHomingEffect;
                        BUPlayer.magicHomingEffect = arcaneHomingEffect;
                        BUPlayer.btdCharged = btdCharged;
                        BUPlayer.atdCharged = atdCharged;
                    }
                    break;
                case BattleUtilitiesMessageType.HomingBulletsChange:
                    {
                        byte playernumber = reader.ReadByte();
                        BattleUtilitiesModPlayer BUPlayer = Main.player[playernumber].GetModPlayer<BattleUtilitiesModPlayer>();
                        bool bulletHomingEffect = reader.ReadBoolean();
                        bool arcaneHomingEffect = reader.ReadBoolean();
                        bool btdCharged = reader.ReadBoolean();
                        bool atdCharged = reader.ReadBoolean();
                        BUPlayer.bulletHomingEffect = bulletHomingEffect;
                        BUPlayer.magicHomingEffect = arcaneHomingEffect;
                        BUPlayer.btdCharged = btdCharged;
                        BUPlayer.atdCharged = atdCharged;
                        if(Main.netMode == NetmodeID.Server)
                        {
                            var packet = GetPacket();
                            packet.Write((byte)BattleUtilitiesMessageType.HomingBulletsChange);
                            packet.Write(playernumber);
                            packet.Write(bulletHomingEffect);
                            packet.Write(arcaneHomingEffect);
                            packet.Write(btdCharged);
                            packet.Write(atdCharged);
                            packet.Send(-1, playernumber);
                        }
                    }
                    break;
            }

    
            
       }

        internal enum BattleUtilitiesMessageType:byte
        {
			SyncPlayer,
			HomingBulletsChange
        }
	}
}