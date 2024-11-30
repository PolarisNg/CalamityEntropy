﻿using CalamityEntropy.Items;
using CalamityEntropy.Util;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Events;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Biomes;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Projectiles.SamsaraCasket
{
    public class SoulDissipate : SamsaraSword
    {
        public int counter { get { return (int)Projectile.ai[1]; } set { Projectile.ai[1] = value; } }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.localNPCHitCooldown = 0;
        }
        public override void attackAI(NPC t)
        {
            setDamage(2);
            if(counter % 10 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item74, Projectile.position);
                alpha = 1;
                Projectile.Center = (t.Center + Util.Util.randomRot().ToRotationVector2() * (180 + (t.width + t.height) / 2));
                Vector2 tPos = t.Center + (t.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (180 + (t.width + t.height) / 2);
                Projectile.velocity = (tPos - Projectile.Center) / 10;
            }
            if (counter % 10 > 7)
            {
                alpha -= 1f / 3f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            counter++;

        }
        public override void backing()
        {
            if(alpha < 1)
            {
                alpha += 1f / 26f;
            }
        }

    }
}
