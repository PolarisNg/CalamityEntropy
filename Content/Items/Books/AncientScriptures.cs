
using CalamityEntropy.Content.ArmorPrefixes;
using CalamityEntropy.Content.Projectiles.TwistedTwin;
using CalamityEntropy.Util;
using CalamityMod.Items.LoreItems;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.Pkcs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Books
{
    public class AncientScriptures : EntropyBook
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 42;
            Item.crit = 2;
        }
        public override int HeldProjectileType => ModContent.ProjectileType<AncientScripturesHeld>();
        public override int SlotCount => 1;

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<LoreAwakening>())
                .AddIngredient(ItemID.Leather, 6)
                .AddIngredient(ItemID.ManaCrystal, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class AncientScripturesHeld : EntropyBookHeldProjectile
    {
        public override string OpenAnimationPath => "CalamityEntropy/Content/Items/Books/Textures/AncientScriptures/AncientScripturesOpen";
        public override string PageAnimationPath => "CalamityEntropy/Content/Items/Books/Textures/AncientScriptures/AncientScripturesPage";
        public override string UIOpenAnimationPath => "CalamityEntropy/Content/Items/Books/Textures/AncientScriptures/AncientScripturesUI";
    }

}