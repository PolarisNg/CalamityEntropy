﻿using CalamityEntropy.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CalamityEntropy.ArmorPrefixes
{
    public class Rust : ArmorPrefix
    {
        public override void updateEquip(Player player, Item item)
        {
        }
        public override Color getColor()
        {
            return Color.Gray;
        }
        public override int getRollChance()
        {
            return 10;
        }
        public override float AddDefense()
        {
            return -0.1f;
        }
    }
}
