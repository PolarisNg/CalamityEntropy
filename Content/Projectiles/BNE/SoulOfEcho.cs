using System;
using System.Collections.Generic;
using System.IO;
using CalamityEntropy.Common;
using CalamityEntropy.Content.Items.Weapons;
using CalamityEntropy.Content.Particles;
using CalamityEntropy.Util;
using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Particle = CalamityEntropy.Content.Particles.Particle;

namespace CalamityEntropy.Content.Projectiles.BNE
{
    public class SoulOfEcho : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = CUtil.rogueDC;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 370;
            Projectile.light = 0.25f;
        }
        float alpha = 1;
        public override void AI(){
            if(Projectile.timeLeft > 340)
            {
                Projectile.velocity *= 0.96f;
                Projectile.rotation = Projectile.velocity.ToRotation();
                return;
            }
            if(Projectile.timeLeft < 30)
            {
                Projectile.velocity *= 0.96f;
                Projectile.rotation = Util.Util.rotatedToAngle(Projectile.rotation, Projectile.velocity.ToRotation(), 0.09f, false);
                alpha -= 1f / 30f;
                return;
            }
            else
            {
                if (target != null && !target.active)
                {
                    target = null;
                }
                if(target == null)
                {
                    target = Util.Util.findTarget(Projectile.owner.ToPlayer(), Projectile, 6000, false);
                }
                if(target != null)
                {
                    Projectile.rotation = Util.Util.rotatedToAngle(Projectile.rotation, (target.Center - Projectile.Center).ToRotation(), 0.09f, false);
 
                    Projectile.velocity = ((target.Center + (Projectile.Center - target.Center).SafeNormalize(Vector2.UnitX) * ((target.width + target.height) / 2f + 90)) - Projectile.Center) * 0.086f;
                    
                    if (++Projectile.ai[0] % 40 == 0)
                    {
                        if(Main.myPlayer == Projectile.owner)
                        {
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 16, ModContent.ProjectileType<Echo>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
;                       }
                        Util.Util.PlaySound("soulScreem", Main.rand.NextFloat(0.7f, 1.3f), Projectile.Center, 1);
                    }
                }
                else
                {
                    if (Util.Util.getDistance(Projectile.Center, Projectile.owner.ToPlayer().Center) > 100)
                    {
                        Projectile.velocity *= 0.97f;
                        Projectile.velocity += (Projectile.owner.ToPlayer().Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                    }
                    Projectile.rotation = Util.Util.rotatedToAngle(Projectile.rotation, Projectile.velocity.ToRotation(), 0.09f, false);
                }
            }
        }
        NPC target = null;
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, Util.Util.GetCutTexRect(tex, 4, (int)(Projectile.Entropy().counter / 4) % 4), Color.White * alpha, Projectile.rotation + MathHelper.PiOver2, new Vector2(25, 32), Projectile.scale, SpriteEffects.None);
            return false;
        }


    }

}