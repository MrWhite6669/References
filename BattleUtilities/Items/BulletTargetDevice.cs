using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BattleUtilities.Items
{
    class BulletTargetDevice:ModItem
    {

        public float btdCharge = 0;
        float btdMaxCharge = 500;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("B.T.D - Bullet Targeting Device");
            Tooltip.SetDefault("Makes ALL bullets home to enemies. \n10% ranged damage increased.");
        }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void OnCraft(Recipe recipe)
        {
            btdCharge = btdMaxCharge;
            UpdateName();
        }

        public string ChargeToString()
        {
            int percentage = (int)((btdCharge / btdMaxCharge) * 100);
            return percentage.ToString();
        }

        int ChargeToPercentage()
        {
            return (int)((btdCharge / btdMaxCharge) * 100);
        }

        public void UpdateName()
        {
            item.RebuildTooltip();
            //DisplayName.SetDefault("B.T.D - Bullet Targeting Device [Charge:" + btdCharge + "%]");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltip;
            if(btdCharge > 0) tooltip = new TooltipLine(mod, "chargeTooltip", "Charge: " + ChargeToString()+ "%");
            else tooltip = new TooltipLine(mod, "chargeTooltip", "No charge!");
            if (ChargeToPercentage() > 70 && ChargeToPercentage() <= 100) tooltip.overrideColor = Colors.RarityCyan;
            if (ChargeToPercentage() > 20 && ChargeToPercentage() < 70) tooltip.overrideColor = Colors.RarityYellow;
            if (ChargeToPercentage() < 20) tooltip.overrideColor = Colors.RarityRed;
            tooltips.Add(tooltip);
        }

        public override TagCompound Save()
        {
            return new TagCompound {
                {"btdCharge",btdCharge }
            };
        }

        public override void Load(TagCompound tag)
        {
            btdCharge = tag.GetFloat("btdCharge");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(btdCharge);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            btdCharge = reader.ReadSingle();
        }

        public void UseCharge()
        {
            if(btdCharge > 0) btdCharge -= 0.1f;
            UpdateName();
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.sellPrice(gold: 20);
            item.accessory = true;
            item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofSight, 20);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.AddIngredient(ItemID.ChlorophyteOre, 12);
            recipe.AddIngredient(ItemID.DestroyerEmblem);
            recipe.AddIngredient(ModContent.GetInstance<QuantumBattery>());
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BattleUtilitiesModPlayer>().bulletHomingEffect = true;
            player.rangedDamageMult += 0.10f;
            UpdateName();
        }

        public override ModItem Clone()
        {
            var clone = (BulletTargetDevice)base.Clone();
            clone.btdCharge = btdCharge;
            return clone;
        }
    }
}
