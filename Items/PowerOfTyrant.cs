using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityEntropy.Projectiles.VoidBlade;
using CalamityEntropy.Util;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Placeables;
using CalamityEntropy.Projectiles;
using CalamityMod.Items;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
namespace CalamityEntropy.Items
{
    public class PowerOfTyrant : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 236;
            Item.crit = 22;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.width = 142;
            Item.noUseGraphic = true;
            Item.height = 142;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<VoidPurple>();
            Item.UseSound = null;
            Item.channel = true;
            Item.ArmorPenetration = 80;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<PoTProj>();
            Item.shootSpeed = 6f;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<PoTProj>()] < 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<DefiledGreatsword>(), 1).
                AddIngredient(ModContent.ItemType<VoidBar>(), 5).
                AddIngredient(ModContent.ItemType<NightmareFuel>(), 10).
                AddTile(ModContent.TileType<CosmicAnvil>()).Register();
        }
    }
}