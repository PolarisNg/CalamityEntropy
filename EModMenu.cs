﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityEntropy
{
    public class EModMenu : ModMenu
    {
        public int counter = 0;
        public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>("CalamityEntropy/Extra/white");
        public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>("CalamityEntropy/Extra/white");
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/startmenu");
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("CalamityEntropy/Extra/Logo");
        public override string DisplayName => "Calamity Entropy";
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Texture2D l1 = ModContent.Request<Texture2D>("CalamityEntropy/Extra/menu/layer1").Value;
            Texture2D l2 = ModContent.Request<Texture2D>("CalamityEntropy/Extra/menu/layer2").Value;
            Texture2D l3 = ModContent.Request<Texture2D>("CalamityEntropy/Extra/menu/layer3").Value;
            Texture2D pixel = ModContent.Request<Texture2D>("CalamityEntropy/Extra/white").Value;
            drawColor = Color.White;
            counter++;
            logoScale = 1;
            logoRotation = 0;
            logoDrawCenter += new Vector2(36, (float)Math.Cos(counter * 0.008f) * 16 + 30);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            spriteBatch.Draw(pixel, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);
            spriteBatch.Draw(l1, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), null, Color.White, MathHelper.ToRadians(counter * 0.12f), l1.Size() / 2, 4, SpriteEffects.None, 0);

            spriteBatch.Draw(l2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            spriteBatch.Draw(l3, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * (0.5f + (float)Math.Cos(counter * 0.01f) * 0.25f));

            Texture2D logo = Logo.Value;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            for (int i = 1; i < 10; i++)
            {
                float rot = 0;
                for (int j = 0; j < 8; j++)
                {
                    spriteBatch.Draw(ModContent.Request<Texture2D>("CalamityEntropy/Extra/Logool").Value, logoDrawCenter + rot.ToRotationVector2() * ((float)i * 0.5f), null, Color.LightBlue * 0.15f, logoRotation, logo.Size() / 2, logoScale, SpriteEffects.None, 0);
                    rot += MathHelper.ToRadians(45);
                }
            }
            if (counter % 15 == 0)
            {
                MenuParticle particle = new MenuParticle(new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), Util.Util.randomRot().ToRotationVector2() * 1, new Vector2(1.5f, 1), 460);
                MenuParticle.particles.Add(particle);
                particle.pos += particle.velocity * 2;
            }
            if (Main.rand.NextBool(140))
            {
                LightningParticle.lightningParticles.Add(new LightningParticle());
            }
            foreach (MenuParticle p in MenuParticle.particles)
            {
                p.update();
                p.draw();
            }
            for (int i = MenuParticle.particles.Count - 1; i >= 0; i--)
            {
                if (MenuParticle.particles[i].timeleft <= 0)
                {
                    MenuParticle.particles.RemoveAt(i);
                }
            }
            foreach (LightningParticle p in LightningParticle.lightningParticles)
            {
                p.draw();
            }
            for (int i = LightningParticle.lightningParticles.Count - 1; i >= 0; i--)
            {
                if (LightningParticle.lightningParticles[i].timeleft <= 0)
                {
                    LightningParticle.lightningParticles.RemoveAt(i);
                }
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            spriteBatch.Draw(logo, logoDrawCenter, null, Color.White, logoRotation, logo.Size() / 2, logoScale, SpriteEffects.None, 0);

            return false;
        }

        public override bool IsAvailable => true;
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<MenuBack>();


    }

    public class MenuParticle
    {
        public Vector2 pos;
        public Vector2 center;
        public Vector2 velocity;
        public Vector2 size;
        public float timeleft;
        public float alpha = 0f;
        public MenuParticle(Vector2 pos, Vector2 center, Vector2 vel, Vector2 size, float time)
        {
            this.pos = pos;
            this.center = center;
            this.velocity = vel;
            this.size = size;
            this.timeleft = time;

        }

        public void update()
        {
            if (alpha < 1)
            {
                alpha += 0.01f;
            }
            this.pos += this.velocity;
            this.velocity = this.velocity.RotatedBy(MathHelper.ToRadians(0.2f) + MathHelper.ToRadians(1) * (2 / (float)Util.Util.getDistance(pos, center)));
            timeleft--;
            this.velocity *= 1.002f;
        }

        public void draw()
        {
            Texture2D tx = ModContent.Request<Texture2D>("CalamityEntropy/Extra/lightball").Value;
            float op = 1;
            if (timeleft < 60)
            {
                op = (float)timeleft / 60f;
            }
            op *= alpha;
            Main.spriteBatch.Draw(tx, pos, null, Color.LightBlue * op, this.velocity.ToRotation(), tx.Size() / 2, this.size * 0.1f, SpriteEffects.None, 0);
        }
        public static List<MenuParticle> particles = new List<MenuParticle>();
    }

    public class LightningParticle
    {
        public List<Vector2> points = new List<Vector2>();
        public List<Vector2> points2 = new List<Vector2>();
        public static List<LightningParticle> lightningParticles = new List<LightningParticle>();
        public LightningParticle()
        {
            Vector2 centerp = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) + new Vector2(Main.rand.Next(-60, 61), Main.rand.Next(-60, 61));
            float a1 = Util.Util.randomRot();
            float a2 = a1 + MathHelper.ToRadians(180);
            Vector2 p1 = centerp;
            Vector2 p2 = centerp;
            for (int i = 0; i < 20; i++)
            {
                points.Add(p1);
                points2.Add(p2);
                a1 += ((float)Main.rand.NextDouble() - 0.5f) * 1f;
                a2 += ((float)Main.rand.NextDouble() - 0.5f) * 1f;
                p1 += a1.ToRotationVector2() * Main.rand.Next(8, 26);
                p2 += a2.ToRotationVector2() * Main.rand.Next(8, 26);
            }
        }

        public int timeleft = 20;

        public void draw()
        {

            timeleft--;
            Texture2D px = ModContent.Request<Texture2D>("CalamityEntropy/Extra/white").Value;

            float jd = 1;
            float lw = 2f * ((float)timeleft / 20f);
            Color color = Color.Purple;
            for (int i = 1; i < points.Count; i++)
            {
                Vector2 jv = points[i] - points[i - 1];
                jv.Normalize();
                jv *= 2;
                Util.Util.drawLine(Main.spriteBatch, px, points[i - 1], points[i] + jv, color * jd, 2f * lw, 0, false);
                lw -= 2f * ((float)timeleft / 20f) / ((float)points.Count + 1);
            }

            jd = 1;
            lw = 2f * ((float)timeleft / 20f);
            color = Color.White;
            for (int i = 1; i < points2.Count; i++)
            {
                Vector2 jv = points2[i] - points2[i - 1];
                jv.Normalize();
                jv *= 2;
                Util.Util.drawLine(Main.spriteBatch, px, points2[i - 1], points2[i] + jv, color * jd, 2f * lw, 0, false);
                lw -= 2f * ((float)timeleft / 20f) / ((float)points2.Count + 1);
            }
        }
    }
}
