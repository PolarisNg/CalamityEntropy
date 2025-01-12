﻿using CalamityEntropy.Util;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityEntropy.Content.ArmorPrefixes
{
    public class LastStand : ArmorPrefix
    {
        public override void updateEquip(Player player, Item item)
        {
            player.Entropy().damageReduce += 0.05f;
            player.Entropy().LastStand = true;
        }
        public override float AddDefense()
        {
            return 0.3f;
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
