﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Particles
{
    public class HadCircle : EParticle
    {
        public override Texture2D texture => ModContent.Request<Texture2D>("CalamityEntropy/Content/Particles/HadCircle").Value;
        public override void onSpawn()
        {
            this.timeLeft = 16;
        }
        public override void update()
        {
            if(this.timeLeft > 8)
            {
                this.timeLeft--;
            }
            base.update();
            this.alpha = (float)Math.Sqrt(1 - (Math.Abs(8f - this.timeLeft) / 8f));
            this.scale = (((float)Math.Sqrt(1 - (Math.Abs(8f - this.timeLeft) / 8f)))) * 0.94f;
        }
    }
    public class HadCircle2 : EParticle
    {
        public override Texture2D texture => ModContent.Request<Texture2D>("CalamityEntropy/Content/Particles/BloomRing").Value;
        public override void onSpawn()
        {
            this.timeLeft = 16;
        }
        public override void update()
        {
            base.update();
            this.alpha = (this.timeLeft / 16f);
            this.scale = (16 - this.timeLeft) / 16f * 2.4f;
        }

        public override void draw()
        {
            base.draw();
            base.draw();
        }
    }
}
