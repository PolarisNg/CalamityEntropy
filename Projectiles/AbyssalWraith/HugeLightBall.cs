using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalamityEntropy.Util;
namespace CalamityEntropy.Projectiles.AbyssalWraith
{
    
    public class HugeLightBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;

        }
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
            Projectile.timeLeft = 600;
        }
        public int cd = 20;
        public int l = 6;
        public override void AI()
        {
            cd--;
            if (cd < 0 && l > 0)
            {
                l--;
                cd = 90;
                for (int i = 0; i < 8; i++)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VoidLightBall>(), (int)(Projectile.damage), 6, -1, 0, 2, 0);
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VoidLightBall>(), (int)(Projectile.damage), 6, -1, 0, 2, 0);

                    }
                }

            }
        }
        float op = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            
            if (Projectile.timeLeft > 20)
            {
                if (op < 1)
                {
                    op += 0.05f;
                }
            }
            if (Projectile.timeLeft < 20)
            {
                op -= 0.05f;
            }
            Texture2D t = Util.Util.getExtraTex("lightball");
            Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, Color.White * op, Projectile.rotation, t.Size() / 2, Projectile.scale * 2, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, Color.White * op, Projectile.rotation, t.Size() / 2, Projectile.scale * 2, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }

}