using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityEntropy.Projectiles;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Rarities;
using CalamityEntropy.Util;
using CalamityMod;
using CalamityMod.Items;
namespace CalamityEntropy.Items
{	
	public class VoidEcho : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 425;
			Item.DamageType = DamageClass.Magic;
			Item.width = 96;
			Item.noUseGraphic = true;
			Item.height = 96;
			Item.useTime = 1;
			Item.useAnimation = 0;
			Item.channel = true;
			Item.knockBack = 4;
			Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.UseSound = null;
			Item.shoot = ModContent.ProjectileType <VBSpawner>();
			Item.shootSpeed = 1f;
			Item.mana = 110;
			Item.useStyle = -1;
			Item.noMelee = true;
            Item.Entropy().stroke = true;
            Item.Entropy().strokeColor = new Color(20, 26, 92);
            Item.Entropy().tooltipStyle = 4;
            Item.Entropy().NameColor = new Color(60, 80, 140);
            Item.Entropy().HasCustomStrokeColor = true;
            Item.Entropy().HasCustomNameColor = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<VoidBlaster>()] <= 0;
        }
    }
}