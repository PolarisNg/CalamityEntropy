﻿using CalamityMod.Graphics.Primitives;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using CalamityEntropy.Util;

namespace CalamityEntropy.Content.Particles
{
    public class WindParticle : EParticle
    {
        public List<Vector2> odp = new List<Vector2>();
        public override Texture2D texture => ModContent.Request<Texture2D>("CalamityEntropy/Content/Particles/Wind").Value;
        public override void onSpawn()
        {
            this.timeLeft = 46;
        }
        public float v1 = 12;
        public float v2 = 3;
        public float rv = Main.rand.NextFloat(0.14f, 0.2f);
        public override void update()
        {
            base.update();
            this.alpha = this.timeLeft / 46f;
            this.velocity = this.rotation.ToRotationVector2() * v1 + r.ToRotationVector2() * v2; 
            this.rotation = this.rotation + dir * rv;
            odp.Insert(0, this.position);
            if(odp.Count > 16)
            {
                odp.RemoveAt(odp.Count - 1);
            }
        }
        public float r = Util.Util.randomRot();
        public int dir = Main.rand.NextBool() ? 1 : -1;

        public override void draw()
        {
            Main.spriteBatch.EnterShaderRegion();
            GameShaders.Misc["CalamityMod:ArtAttack"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityEntropy/Content/Particles/Wind"));
            GameShaders.Misc["CalamityMod:ArtAttack"].Apply();
            PrimitiveRenderer.RenderTrail(odp, new PrimitiveSettings(TrailWidth, TrailColor, (float _) => Vector2.Zero, smoothen: true, pixelate: false, GameShaders.Misc["CalamityMod:ArtAttack"]), 180);
            Main.spriteBatch.ExitShaderRegion();
            Main.spriteBatch.UseBlendState(this.useAlphaBlend ? BlendState.AlphaBlend : (this.useAdditive ? BlendState.Additive : BlendState.NonPremultiplied));
        }

        public Color TrailColor(float completionRatio)
        {
            Color result = this.color * completionRatio * this.alpha * new Vector2(1, 0).RotatedBy(completionRatio * MathHelper.Pi).Y;
            return result;
        }

        public float TrailWidth(float completionRatio)
        {
            return this.scale * 26;
        }
    }
}
