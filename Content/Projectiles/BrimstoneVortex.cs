using CalamityEntropy.Content.DimDungeon;
using CalamityEntropy.Content.Dusts;
using CalamityEntropy.Content.Items.Books;
using CalamityEntropy.Util;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Particles;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{
    public class BrimstoneVortex : EBookBaseLaser
    {
        public int counter = 0;
        public override List<Vector2> getSamplePoints()
        {
            if(counter < 40)
            {
                return new List<Vector2>() { Projectile.Center };
            }
            return base.getSamplePoints();
        }
        public float scale = 0;
        public float scalej = 0.32f;
        public float scale2 = 1;
        public override void AI()
        {
            scale += scalej;
            scalej *= 0.9f;
            scale += (1.3f - scale) * 0.1f;
            base.AI();
            counter++;
            if (counter == 40)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.velocity = new Vector2(Projectile.velocity.Length(), 0).RotatedBy((Main.MouseWorld - Projectile.Center).ToRotation());
                    Projectile.netUpdate = true;
                }

            }
            if(counter > 60)
            {
                scale2 -= 0.05f;
                if(scale2 <= 0)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Projectile.getTexture();
            List<Vector2> points = getSamplePoints();
            if (points.Count > 1)
            {
                drawLaser(points);
            }
            Main.spriteBatch.UseBlendState(BlendState.NonPremultiplied);
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Main.GameUpdateCount * 0.2f, tex.Size() / 2f, Projectile.scale * 0.6f * scale * scale2, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Main.GameUpdateCount * 0.4f, tex.Size() / 2f, Projectile.scale * 0.4f * scale * scale2, SpriteEffects.FlipHorizontally, 0);
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Main.GameUpdateCount * 0.6f, tex.Size() / 2f, Projectile.scale * 0.2f * scale * scale2, SpriteEffects.None, 0);
            Main.spriteBatch.UseBlendState(BlendState.AlphaBlend);
            return false;
        }

        public void drawLaser(List<Vector2> points)
        {
             
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            {
                Texture2D tx = ModContent.Request<Texture2D>("CalamityEntropy/Assets/Extra/MegaStreakBacking2").Value;
                List<Vertex> ve = new List<Vertex>();
                Color b = new Color(200, 0, 0);
                float p = -Main.GlobalTimeWrappedHourly * 2;
                for (int i = 1; i < points.Count; i++)
                {
                    ve.Add(new Vertex(points[i] - Main.screenPosition + (points[i] - points[i - 1]).ToRotation().ToRotationVector2().RotatedBy(MathHelper.ToRadians(90)) * 24 * Projectile.scale * scale2,
                          new Vector3(p, 1, 1),
                          b));
                    ve.Add(new Vertex(points[i] - Main.screenPosition + (points[i] - points[i - 1]).ToRotation().ToRotationVector2().RotatedBy(MathHelper.ToRadians(-90)) * 24 * Projectile.scale * scale2,
                          new Vector3(p, 0, 1),
                          b));
                    p += (Util.Util.getDistance(points[i], points[i - 1]) / tx.Width);
                }

                SpriteBatch sb = Main.spriteBatch;
                GraphicsDevice gd = Main.graphics.GraphicsDevice;
                if (ve.Count >= 3)
                {
                    gd.Textures[0] = tx;
                    gd.DrawUserPrimitives(PrimitiveType.TriangleStrip, ve.ToArray(), 0, ve.Count - 2);
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            {
                Texture2D tx = ModContent.Request<Texture2D>("CalamityEntropy/Assets/Extra/Streak1").Value;
                List<Vertex> ve = new List<Vertex>();
                Color b = new Color(255, 240, 240);
                float p = -Main.GlobalTimeWrappedHourly * 2;
                for (int i = 1; i < points.Count; i++)
                {
                    ve.Add(new Vertex(points[i] - Main.screenPosition + (points[i] - points[i - 1]).ToRotation().ToRotationVector2().RotatedBy(MathHelper.ToRadians(90)) * 18 * Projectile.scale * scale2,
                          new Vector3(p, 1, 1),
                          b));
                    ve.Add(new Vertex(points[i] - Main.screenPosition + (points[i] - points[i - 1]).ToRotation().ToRotationVector2().RotatedBy(MathHelper.ToRadians(-90)) * 18 * Projectile.scale * scale2,
                          new Vector3(p, 0, 1),
                          b));
                    p += (Util.Util.getDistance(points[i], points[i - 1]) / tx.Width);
                }


                SpriteBatch sb = Main.spriteBatch;
                GraphicsDevice gd = Main.graphics.GraphicsDevice;
                if (ve.Count >= 3)
                {
                    
                    gd.Textures[0] = tx;
                    gd.DrawUserPrimitives(PrimitiveType.TriangleStrip, ve.ToArray(), 0, ve.Count - 2);
                }
            }
            Util.Util.DrawGlow(points[points.Count - 1], new Color(255, 100, 100), 1.6f * Projectile.scale);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

        }
    }
    

}