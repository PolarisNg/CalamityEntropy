using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityEntropy.Util;

namespace CalamityEntropy.Buffs
{
    public class YharimPower : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) *= 1.05f;
            player.GetCritChance(DamageClass.Generic) += 5;
            player.pickSpeed *= 1.4f;
            player.GetAttackSpeed(DamageClass.Melee) *= 1.1f;
            player.GetKnockback(DamageClass.Summon) *= 2;
            player.moveSpeed *= 1.2f;
            player.luck += 4;
            player.GetArmorPenetration(DamageClass.Generic) += 10f;
            player.Calamity().contactDamageReduction *= 1.08f;
            player.Calamity().projectileDamageReduction *= 1.08f;
            player.statDefense += 10;
            player.lifeRegen += 8;
        }
    }
}
