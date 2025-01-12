﻿using CalamityEntropy.Util;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityEntropy.Content.ArmorPrefixes
{
    public class VoidTouched : ArmorPrefix
    {
        public override void updateEquip(Player player, Item item)
        {
            player.Entropy().AttackVoidTouch += 0.3f;
        }
        public override int getRollChance()
        {
            return 1;
        }
        public override bool? canApplyTo(Item item)
        {
            if(!Main.hardMode) return false;
            return Main.rand.NextBool(2);
        }
        public override Color getColor()
        {
            return Color.Violet;
        }
        public override bool Dramatic()
        {
            return true;
        }

    }
}
