﻿using CalamityEntropy.Content.Projectiles;
using CalamityEntropy.Content.Rarities;
using CalamityEntropy.Util;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Weapons.CrystalBalls
{
    public class OverloadLunar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.damage = 78;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.knockBack = 1f;
            Item.UseSound = Util.Util.GetSound("soulshine");
            Item.maxStack = 1;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<OverloadLunarHoldout>();
            Item.shootSpeed = 16f;
            Item.mana = 14;
            Item.DamageType = DamageClass.Magic;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 5)
                .AddIngredient(ItemID.CrystalBall, 1)
                .AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
        public override bool MagicPrefix()
        {
            return true;
        }
    }
}
