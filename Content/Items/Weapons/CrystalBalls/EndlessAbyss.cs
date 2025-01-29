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
    public class EndlessAbyss : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 4));
        }
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.damage = 560;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.knockBack = 1f;
            Item.UseSound = Util.Util.GetSound("soulshine");
            Item.maxStack = 1;
            Item.value = CalamityGlobalItem.RarityCalamityRedBuyPrice;
            Item.rare = ModContent.RarityType<VoidPurple>();
            Item.shoot = ModContent.ProjectileType<EndlessAbyssHoldout>();
            Item.shootSpeed = 16f;
            Item.mana = 10;
            Item.DamageType = DamageClass.Magic;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<VoidBar>(), 10)
                .AddIngredient(ItemID.CrystalBall, 1)
                .AddTile(ModContent.TileType<CosmicAnvil>())
                .Register();
        }
    }
}
