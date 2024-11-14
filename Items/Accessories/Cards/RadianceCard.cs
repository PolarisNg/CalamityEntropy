using CalamityEntropy.Buffs;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Items.Accessories.Cards
{
	public class RadianceCard : ModItem
	{

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 22;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
			
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen = (int)(player.lifeRegen * 1.2f);
            player.GetModPlayer<EModPlayer>().radianceCard = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StarblightSoot>(), 5)
                .AddIngredient(ModContent.ItemType<EssenceofSunlight>(), 5)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddTile(TileID.CrystalBall)
                .Register();
        }
    }
}
