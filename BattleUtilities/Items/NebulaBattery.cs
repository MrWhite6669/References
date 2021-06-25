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
    class NebulaBattery:ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nebula Battery");
            Tooltip.SetDefault("Battery used to create and recharge advanced arcane devices.");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 10;
            item.value = Item.sellPrice(platinum: 5);
            item.rare = ItemRarityID.Expert;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.GetInstance<QuantumBattery>());
            recipe.AddIngredient(ItemID.FragmentNebula, 40);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(this);
            recipe2.AddIngredient(ModContent.GetInstance<ArcaneTargetDevice>());
            recipe2.SetResult(ModContent.GetInstance<ArcaneTargetDevice>());
            recipe2.AddRecipe();
        }
    }
}
