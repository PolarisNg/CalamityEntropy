﻿using CalamityEntropy.Content.Projectiles;
using CalamityEntropy.Content.Projectiles.BNE;
using CalamityEntropy.Content.Rarities;
using CalamityEntropy.Util;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Weapons
{
    public class TheBeginingAndTheEnd : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 38;
            Item.damage = 3260;
            Item.ArmorPenetration = 80;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.ArmorPenetration = 86;
            Item.knockBack = 1f;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.value = CalamityGlobalItem.RarityCalamityRedBuyPrice;
            Item.rare = ModContent.RarityType<AbyssalBlue>();
            Item.shoot = ModContent.ProjectileType<TheBeginning>();
            Item.shootSpeed = 9f;
            Item.DamageType = CUtil.rogueDC;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p1 = ModContent.ProjectileType<TheBeginning>();
            int p2 = ModContent.ProjectileType<TheEnd>();
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position, velocity.RotatedBy(0.2f), p1, damage, knockback, player.whoAmI);
                Main.projectile[p].Calamity().stealthStrike = true;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p);
                }
                p = Projectile.NewProjectile(source, position, velocity.RotatedBy(-0.2f), p2, damage, knockback, player.whoAmI);
                Main.projectile[p].Calamity().stealthStrike = true;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p);
                }
                return false;
            }
            if(player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, p2, damage, knockback, player.whoAmI);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, p1, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public static void playShootSound(Vector2 c)
        {
            Util.Util.PlaySound("bne" + Main.rand.Next(0, 3).ToString(), 1, c);
        }
        public override float StealthDamageMultiplier => 1.2f;
        public override float StealthVelocityMultiplier => 0.8f;
        public override float StealthKnockbackMultiplier => 3f;

        public override void AddRecipes()
        {
        }
    }
}
