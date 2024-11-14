using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityEntropy.Projectiles.Pets;
using CalamityEntropy.Projectiles.Pets.DoG;
using CalamityEntropy.Projectiles.Pets.Deus;
using CalamityEntropy.Projectiles.Pets.DarkFissure;
namespace CalamityEntropy.Buffs.Pets
{
    public class WeakGravity : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            bool unused = false;
			player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<DarkFissure>());
		}
    }
}
