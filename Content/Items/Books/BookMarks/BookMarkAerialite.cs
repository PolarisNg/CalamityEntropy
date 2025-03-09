
using CalamityEntropy.Content.ArmorPrefixes;
using CalamityEntropy.Content.Projectiles;
using CalamityEntropy.Content.Projectiles.TwistedTwin;
using CalamityEntropy.Content.UI.EntropyBookUI;
using CalamityEntropy.Util;
using CalamityMod.Items;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.Pkcs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Books.BookMarks
{
    public class BookMarkAerialite : BookMark
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
        }
        public override Texture2D UITexture => BookMark.GetUITexture("Aerialite");
        public override EBookProjectileEffect getEffect()
        {
            return new AerialiteBMEffect();
        }
        public override void ModifyStat(EBookStatModifer modifer)
        {
            modifer.shotSpeed += 0.4f;
        }

        public override Color tooltipColor => new Color(42, 227, 231);
    }

    public class AerialiteBMEffect : EBookProjectileEffect
    {
        public override void onHitNPC(Projectile projectile, NPC target, int damageDone)
        {
            Vector2 shootPosition = projectile.Center + new Vector2(0, -900) + Util.Util.randomVec(128);
            (Projectile.NewProjectile(projectile.GetSource_FromThis(), shootPosition, (target.Center - shootPosition).normalize() * Math.Max(projectile.velocity.Length() * 0.8f, 8), ModContent.ProjectileType<WelkinFeather2>(), damageDone / 5, projectile.knockBack / 3, projectile.owner).ToProj().ModProjectile as EBookBaseProjectile).homing = (projectile.ModProjectile as EBookBaseProjectile).homing;
        }
    }
}