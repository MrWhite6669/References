using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BattleUtilities.Items
{
    class QuantumBattery:ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quantum Battery");
            Tooltip.SetDefault("Battery used to create and recharge advanced devices.");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 10;
            item.value = Item.sellPrice(platinum: 1);
            item.rare = ItemRarityID.Expert;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar,2);
            recipe.AddIngredient(ItemID.Nanites, 100);
            recipe.AddIngredient(ItemID.LihzahrdPowerCell);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(this);
            recipe2.AddIngredient(ModContent.GetInstance<BulletTargetDevice>());
            recipe2.SetResult(ModContent.GetInstance<BulletTargetDevice>());
            recipe2.AddRecipe();
        }
    }
}
