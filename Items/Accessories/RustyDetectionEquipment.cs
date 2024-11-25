using CalamityEntropy.Buffs;
using CalamityEntropy.Util;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Items.Accessories
{
	public class RustyDetectionEquipment : ModItem
	{

		public override void SetDefaults() {
			Item.width = 36;
            Item.defense = 4;
			Item.height = 46;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
			
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rocketBoots += 35;
            player.noFallDmg = true;
            player.jumpSpeedBoost += 4f;
            player.maxRunSpeed *= 1.4f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 6).
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 4).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
