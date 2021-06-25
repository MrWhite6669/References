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
    class ArcaneTargetDevice:ModItem
    {

        public float atdCharge = 0;
        float atdMaxCharge = 1000;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("A.T.D - Arcane Targeting Device");
            Tooltip.SetDefault("Modified version of B.T.D, makes ALL magic projectiles homing. \n10% magic damage increased.");
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
            atdCharge = atdMaxCharge;
            UpdateName();
        }

        public string ChargeToString()
        {
            int percentage = (int)((atdCharge / atdMaxCharge) * 100);
            return percentage.ToString();
        }

        int ChargeToPercentage()
        {
            return (int)((atdCharge / atdMaxCharge) * 100);
        }

        public void UpdateName()
        {
            item.RebuildTooltip();
            //DisplayName.SetDefault("B.T.D - Bullet Targeting Device [Charge:" + btdCharge + "%]");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltip;
            if(atdCharge > 0) tooltip = new TooltipLine(mod, "chargeTooltip", "Charge: " + ChargeToString()+ "%");
            else tooltip = new TooltipLine(mod, "chargeTooltip", "No charge!");
            if (ChargeToPercentage() > 70 && ChargeToPercentage() <= 100) tooltip.overrideColor = Colors.RarityCyan;
            if (ChargeToPercentage() > 20 && ChargeToPercentage() < 70) tooltip.overrideColor = Colors.RarityYellow;
            if (ChargeToPercentage() < 20) tooltip.overrideColor = Colors.RarityRed;
            tooltips.Add(tooltip);
        }

        public override TagCompound Save()
        {
            return new TagCompound {
                {"atdCharge",atdCharge }
            };
        }

        public override void Load(TagCompound tag)
        {
            atdCharge = tag.GetFloat("atdCharge");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(atdCharge);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            atdCharge = reader.ReadSingle();
        }

        public void UseCharge()
        {
            if(atdCharge > 0) atdCharge -= 0.1f;
            UpdateName();
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.sellPrice(platinum: 5);
            item.accessory = true;
            item.rare = ItemRarityID.Expert;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.GetInstance<BulletTargetDevice>());
            recipe.AddIngredient(ModContent.GetInstance<NebulaBattery>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BattleUtilitiesModPlayer>().magicHomingEffect = true;
            player.magicDamageMult += 0.10f;
            UpdateName();
        }

        public override ModItem Clone()
        {
            var clone = (ArcaneTargetDevice)base.Clone();
            clone.atdCharge = atdCharge;
            return clone;
        }
    }
}
