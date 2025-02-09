﻿using System;
using System.Collections;
using System.Collections.Generic;
using CalamityEntropy.Content.Buffs;
using CalamityEntropy.Content.Buffs.Wyrm;
using CalamityEntropy.Content.Particles;
using CalamityEntropy.Util;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{
	public class YstralynProj : ModProjectile
	{
        public List<Vector2> pointsCrack = new List<Vector2>();
        public override void SetStaticDefaults() {
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.DefaultToWhip();
			Projectile.MaxUpdates = 8;
			Projectile.WhipSettings.Segments = 19;
			Projectile.WhipSettings.RangeMultiplier = 4.8f; 
			Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
		
        public override bool PreAI()
        {
            var owner = Projectile.owner.ToPlayer();
            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            List<Vector2> points_ = Projectile.WhipPointsForCollision;
            points_.Clear();
            Projectile.FillWhipControlPoints(Projectile, points_);
            List<Vector2> points = points_;
			
            float swingProgress = Timer / swingTime;
            if (swingProgress > 0.46f && swingProgress < 0.9f)
            {
                Vector2 top = points[points.Count - 1] - owner.Center;
                if (pointsCrack.Count > 0)
				{
					Vector2 o = pointsCrack[pointsCrack.Count - 1];
					Vector2 nv = top;
					for (float i = 0.1f; i <= 1; i += 0.1f)
					{
						pointsCrack.Add(Vector2.Lerp(o, nv, i));
						if (pointsCrack.Count > 170)
						{
							pointsCrack.RemoveAt(0);
						}
					}
				}
				else
				{
					pointsCrack.Add(top);
				}
            }
            return true;
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			var player = Projectile.getOwner();
			player.AddBuff(ModContent.BuffType<WyrmPhantom>(), 480);
			target.AddBuff(ModContent.BuffType<WyrmWhipDebuff>(), 380);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Util.Util.PlaySound("ystn_hit", Main.rand.NextFloat(0.86f, 1.2f), target.Center, 3, 0.76f);
			for (int ii = 0; ii < 4; ii++)
			{
                for (int i = 0; i < (Projectile.Calamity().stealthStrike ? 6 : 1); i++)
                {
                    EParticle.spawnNew(new AbyssalLine() { lx = 1.2f, xadd = 1.2f }, target.Center, Vector2.Zero, Color.White, 1, 1, true, BlendState.Additive, Util.Util.randomRot());
                }
            }


		}
        public void draw_crack()
        {
            if (pointsCrack.Count < 1)
            {
                return;
            }
            Texture2D px = ModContent.Request<Texture2D>("CalamityEntropy/Assets/Extra/white").Value;
            float jd = 1;
            float lw = 1 - Timer / (Projectile.owner.ToPlayer().itemAnimationMax * Projectile.MaxUpdates);
			if(lw > 0.26f)
			{
				lw = 0.26f;
			}
            Color color = Color.White;
            for (int i = 1; i < pointsCrack.Count; i++)
            {
                Vector2 jv = Vector2.Zero;
                Util.Util.drawLine(Main.spriteBatch, px, pointsCrack[i - 1] + Projectile.owner.ToPlayer().Center, pointsCrack[i] + jv + Projectile.owner.ToPlayer().Center, color * jd, 1f * lw * (new Vector2(-100, 0).RotatedBy(MathHelper.ToRadians(180 * ((float)i / pointsCrack.Count)))).Y, 4);
            }
        }
        // This method draws a line between all points of the whip, in case there's empty space between the sprites.
        private void DrawLine(List<Vector2> list) {
			Texture2D texture = ModContent.Request<Texture2D>("CalamityEntropy/Assets/Extra/white").Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(0, 0.5f);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count - 1; i++) {
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation();
				Color color = Color.DeepSkyBlue;
				Vector2 scale = new Vector2(diff.Length() + 2, 2);
				if (i == list.Count - 2)
				{
					scale.X -= 8;
				}

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}
        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override bool PreDraw(ref Color lightColor) {
			List<Vector2> list_ = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list_);
			List<Vector2> list = list_;
			DrawLine(list);

			//Main.DrawWhip_WhipBland(Projectile, list);
			// The code below is for custom drawing.
			// If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
			// However, you must adhere to how they draw if you do.

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 pos = list[0];

			for (int i = 0; i < list.Count - 1; i++) {
				// These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
				// You can change them if they don't!
				Rectangle frame = new Rectangle(0, 0, 34, 36); // The size of the Handle (measured in pixels)
				Vector2 origin = new Vector2(17, 16); // Offset for where the player's hand will start measured from the top left of the image.
				float scale = 1.5f;

				// These statements determine what part of the spritesheet to draw for the current segment.
				// They can also be changed to suit your sprite.
				if (i == list.Count - 2) {
					// This is the head of the whip. You need to measure the sprite to figure out these values.
					frame.Y = 64; // Distance from the top of the sprite to the start of the frame.
					frame.Height = 20; // Height of the frame.

					// For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
					Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
					float t = Timer / timeToFlyOut;
					scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true)) * 1.5f;
                    origin = new Vector2(17, 0);
					// Offset for where the player's hand will start measured from the top left of the image.

                }
                else if(i > 0)
				{
                    if (i % 2 == 0)
                    {
                        // Third segment
                        frame.Y = 36;
                        frame.Height = 14;
                        origin = new Vector2(17, 0);
                    }
                    else
                    {
                        // Second Segment
                        frame.Y = 50;
                        frame.Height = 14;
                        origin = new Vector2(17, 0);
                    }
                }

				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
				Color color = Color.White;

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;
			}
            return false;
		}
	}
}
