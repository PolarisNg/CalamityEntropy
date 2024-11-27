using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using CalamityEntropy.Buffs;
using CalamityEntropy.Projectiles;
using CalamityEntropy.Util;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.Audio;
using CalamityEntropy.Projectiles.TwistedTwin;
using CalamityMod.Rarities;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Weapons.DraedonsArsenal;
namespace CalamityEntropy.Items
{	
	public class PhantomPlanetKillerEngine : ModItem
	{
       public override void SetStaticDefaults()
	   {
		ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
		ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		ItemID.Sets.ItemNoGravity[Item.type] = true;
	   }
		
		public override void SetDefaults()
		{
			Item.damage = 185;
			Item.crit = 0;
			Item.DamageType = DamageClass.Summon;
			Item.width = 64;
			Item.height = 64;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.knockBack = 2;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			Item.shoot = ModContent.ProjectileType<PlanetKiller>();
			Item.shootSpeed = 2f;
			Item.value = 100000;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item15;
            Item.noMelee = true;
			Item.mana = 80;
			Item.buffType = ModContent.BuffType<PlanetDestroyer>();
            Item.rare = ModContent.RarityType<VoidPurple>();
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 3);
			int projectile = Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, Item.damage, knockback, player.whoAmI, 0, 1, 0);
			Main.projectile[projectile].originalDamage = Item.damage;
            
            return false;
        }

        public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<EndothermicEnergy>(), 8)
				.AddIngredient(ModContent.ItemType<CosmicViperEngine>())
				.AddIngredient(ModContent.ItemType<PoleWarper>())
				.AddIngredient(ModContent.ItemType<VoidBar>(), 8)
				.AddTile(ModContent.TileType<CosmicAnvil>()).Register();
        }
    }
}