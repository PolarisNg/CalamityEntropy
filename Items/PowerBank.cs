using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityEntropy.Projectiles.VoidBlade;
using CalamityEntropy.Util;
using Microsoft.Xna.Framework;
using CalamityMod.Rarities;
using CalamityMod.Events;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod;
using Terraria.Audio;
using CalamityEntropy.NPCs.Cruiser;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityEntropy.Projectiles;
using CalamityMod.Items.Placeables.DraedonStructures;
namespace CalamityEntropy.Items
{
    public class PowerBank : ModItem
    {
        public static float CHARGE_PER_TICK = 0.05f;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 17;
        }
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 56;
            Item.consumable = false;
            Item.rare = ItemRarityID.Orange;

        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<PowerCellFactoryItem>(), 1).
                AddIngredient(ModContent.ItemType<ChargingStationItem>(), 1).
                AddTile(TileID.Anvils).
                Register();
        }

        public override void UpdateInventory(Player player)
        {
            foreach (Item item in player.inventory)
            {
                if (!item.IsAir)
                {
                    if (item.Calamity().UsesCharge)
                    {
                        if (item.Calamity().Charge < item.Calamity().MaxCharge)
                        {
                            item.Calamity().Charge += CHARGE_PER_TICK;
                            if (item.Calamity().Charge > item.Calamity().MaxCharge)
                            {
                                item.Calamity().Charge = item.Calamity().MaxCharge;
                            }
                        }
                    }
                }
            }
        }
    }
}