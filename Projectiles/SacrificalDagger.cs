using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using CalamityEntropy.Projectiles;
using CalamityMod.Projectiles.Rogue;
using CalamityEntropy.Util;
using Terraria.DataStructures;
using CalamityMod.Projectiles.Typeless;
using CalamityMod;
using Terraria.GameContent;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Particles;
using CalamityMod.Items.Weapons.Melee;
using CalamityEntropy.Items;
namespace CalamityEntropy.Projectiles
{
    public class SacrificalDagger : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = NoneTypeDamageClass.Instance;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
            Projectile.timeLeft = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.ArmorPenetration = 100;
        }
        public float ofs = 0;
        public float ofsVel = 0;
        public Vector2 cPos = Vector2.Zero;
        public override void AI(){
            healCd--;
            Projectile.velocity *= 0;
            Player player = Projectile.owner.ToPlayer();
            if (player.Entropy().sacrMask)
            {
                Projectile.timeLeft = 3;
            }
            NPC n = Projectile.FindTargetWithinRange(300, false);
            if (n != null && Util.Util.getDistance(Projectile.Center, n.Center) > Util.Util.getDistance(n.Center, player.Center + (n.Center - player.Center).SafeNormalize(Vector2.One).RotatedBy(MathHelper.PiOver4) * 64))
            {
                n = null;
            }
            if (n != null && healCd <= 0)
            {
                if(ofs <= 0)
                {
                    ofsVel = 16;
                    cPos = n.Center;
                }
            }
            ofs += ofsVel;
            if(ofs > 0)
            {
                ofs += ofsVel;
                ofsVel -= 1f;
                
            }
            if(ofs < 0)
            {
                ofs = 0;
                ofsVel = 0;
            }
            int index = 0;
            foreach(Projectile p in Main.projectile)
            {
                if(p.whoAmI == Projectile.whoAmI)
                {
                    break;
                }
                if(p.type == Projectile.type && p.active && p.owner == Projectile.owner)
                {
                    index++;
                }
            }
            Projectile.Center = player.Center + (player.Entropy().CasketSwordRot * 0.5f).ToRotationVector2().RotatedBy(MathHelper.ToRadians(index * 360 / 8)) * 64 + (cPos - (player.Center + (player.Entropy().CasketSwordRot * 0.5f).ToRotationVector2().RotatedBy(MathHelper.ToRadians(index * 360 / 8)) * 64)).SafeNormalize(Vector2.Zero) * ofs;
            if(n != null)
            {
                Projectile.rotation = (n.Center - Projectile.Center).ToRotation();
            }
            else if(ofs > 0)
            {
                Projectile.rotation = (Projectile.Center - player.Center).ToRotation();
            }
            else
            {
                Projectile.rotation = player.Entropy().CasketSwordRot * 0.5f + MathHelper.ToRadians(index * 360 / 8);
            }
            Projectile.ai[0]++;
            
        }
        public int healCd = 0;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(Projectile.owner.ToPlayer().statLife < Projectile.owner.ToPlayer().statLifeMax2 && healCd <= 0)
            Projectile.owner.ToPlayer().Heal(2);
            healCd = 14;
        }
    }

}