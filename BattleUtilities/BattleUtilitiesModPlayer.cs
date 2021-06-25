using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BattleUtilities
{
    class BattleUtilitiesModPlayer:ModPlayer
    {
        public bool bulletHomingEffect = false;
        public bool magicHomingEffect = false;
        public bool btdCharged = true;
        public bool atdCharged = true;


        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void ResetEffects()
        {
            bulletHomingEffect = false;
            magicHomingEffect = false;
        }

        public override void clientClone(ModPlayer clientClone)
        {
            BattleUtilitiesModPlayer clone = clientClone as BattleUtilitiesModPlayer;
            // Here we would make a backup clone of values that are only correct on the local players Player instance.
            // Some examples would be RPG stats from a GUI, Hotkey states, and Extra Item Slots
            clone.bulletHomingEffect = bulletHomingEffect;
            clone.magicHomingEffect = magicHomingEffect;
            clone.btdCharged = btdCharged;
            clone.atdCharged = atdCharged;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)BattleUtilities.BattleUtilitiesMessageType.SyncPlayer);
            packet.Write((byte)player.whoAmI);
            packet.Write(bulletHomingEffect);
            packet.Write(magicHomingEffect);
            packet.Write(btdCharged);
            packet.Write(atdCharged);
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            // Here we would sync something like an RPG stat whenever the player changes it.
            BattleUtilitiesModPlayer clone = clientPlayer as BattleUtilitiesModPlayer;
            if (clone.bulletHomingEffect != bulletHomingEffect || clone.magicHomingEffect != magicHomingEffect || clone.btdCharged != btdCharged || clone.atdCharged != atdCharged)
            {
                // Send a Mod Packet with the changes.
                var packet = mod.GetPacket();
                packet.Write((byte)BattleUtilities.BattleUtilitiesMessageType.HomingBulletsChange);
                packet.Write((byte)player.whoAmI);
                packet.Write(bulletHomingEffect);
                packet.Write(magicHomingEffect);
                packet.Write(btdCharged);
                packet.Write(atdCharged);
                packet.Send();
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (bulletHomingEffect && !target.friendly)
            {
                for (int i = 3; i < player.armor.Length; i++)
                {
                    Item item = player.armor[i];
                    if (item.modItem is Items.BulletTargetDevice btd) {btd.UseCharge(); if (btd.btdCharge > 0) btdCharged = true; else btdCharged = false; }
                }
            }

            if (magicHomingEffect && !target.friendly)
            {
                for (int i = 3; i < 8 + player.armor.Length; i++)
                {
                    Item item = player.armor[i];
                    if (item.modItem is Items.ArcaneTargetDevice atd) { atd.UseCharge(); if (atd.atdCharge > 0) atdCharged = true; else atdCharged = false; }
                }
            }
        }
    }
}
