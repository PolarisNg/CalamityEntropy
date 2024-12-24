﻿using CalamityEntropy.Content.ArmorPrefixes;
using CalamityEntropy.Content.Projectiles;
using CalamityEntropy.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalamityEntropy.Content.UI.Poops
{
    public class PoopGolden : Poop
    {
        public override int ProjectileType()
        {
            return ModContent.ProjectileType<PoopGoldenProjectile>();
        }
        public override float getRollChance()
        {
            return 0.32f;
        }
    }
}
