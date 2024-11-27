﻿using CalamityEntropy.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityEntropy.ArmorPrefixes
{
    public class LastStand : ArmorPrefix
    {
        public override void updateEquip(Player player, Item item)
        {
            player.Entropy().damageReduce += 0.2f;
            player.Entropy().LastStand = true;
        }
        public override float AddDefense()
        {
            return 0.6f;
        }
        public override int getRollChance()
        {
            return 1;
        }
        public override Color getColor()
        {
            return Color.Violet;
        }
        public override bool Dramatic()
        {
            return true;
        }
        public override bool Precious()
        {
            return true;
        }
        public override bool? canApplyTo(Item item)
        {
            return Main.rand.NextBool(3);
        }
    }
}
