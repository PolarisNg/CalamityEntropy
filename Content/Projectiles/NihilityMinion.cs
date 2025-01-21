using CalamityEntropy.Content.Buffs;
using CalamityEntropy.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{
    public class NihilityMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
            Projectile.minion = true;
            Projectile.minionSlots = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 26;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public bool stickOnNPC = false;
        public int stickNPCIndex = -1;
        public int direction = 0;
        public Vector2 offset = Vector2.Zero; 
        public void spawnParticle(Vector2 center)
        {/*
            Vector2 vel = (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * (float)Math.Cos(Projectile.localAI[0] * 0.3f) * 14;
            Vector2 vel2 = vel * -1;
            vel -= Projectile.velocity * 0.7f;
            vel2 -= Projectile.velocity * 0.7f;
            Dust.NewDust(center, 1, 1, DustID.MagicMirror, vel.X, vel.Y);
            Dust.NewDust(center, 1, 1, DustID.MagicMirror, vel2.X, vel2.Y);*/

        }
        public override void AI()
        {
            Projectile.localAI[0]++;
            Player player = Main.player[Projectile.owner];
            if (Util.Util.getDistance(Projectile.Center, player.Center) > 2600)
            {
                Projectile.Center = player.Center;
            }
            if (player.HasBuff(ModContent.BuffType<NihilityBacteriophageBuff>()))
            {
                Projectile.timeLeft = 3;
            }

            NPC target = Util.Util.findTarget(player, Projectile, 1800, false);
            if (target != null)
            {
                bool needHealOwner = player.statLife < player.statLifeMax - 80;
                if (needHealOwner)
                {
                    Projectile.ai[1] = 0;
                    if (!stickOnNPC)
                    {
                        Projectile.rotation = (Projectile.Center - target.Center).ToRotation();
                        Projectile.velocity *= 0.9f; 
                        Projectile.velocity += (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 9f;
                        if (Projectile.Colliding(Projectile.getRect(), target.getRect()))
                        {
                            if (!target.dontTakeDamage)
                            {
                                stickOnNPC = true;
                                stickNPCIndex = target.whoAmI;
                                offset = target.Center - Projectile.Center;
                                offset *= -0.5f;
                            }
                        }
                    }
                    else
                    {
                        
                        if(Util.Util.getDistance(Projectile.Center, stickNPCIndex.ToNPC().Center) > 200) {
                            stickOnNPC = false;
                        }
                        if (stickNPCIndex.ToNPC().dontTakeDamage)
                        {
                            stickOnNPC = false;
                        }
                        if (stickOnNPC)
                        {
                            Projectile.velocity *= 0;
                            Projectile.Center = stickNPCIndex.ToNPC().Center + offset;
                            Projectile.ai[0]++;
                            if (Projectile.ai[0] % 5 == 0)
                            {
                                player.Heal(1);
                            }
                        }
                        Projectile.rotation = (Projectile.Center - stickNPCIndex.ToNPC().Center).ToRotation();
                    }
                }
                else
                {
                    stickOnNPC = false;
                    Projectile.ai[1]--;
                    if (Projectile.ai[1] < -24)
                    {
                        Projectile.ai[1] = 16 + Main.rand.Next(0, 8);
                    }
                    if (Projectile.ai[1]-- > 0)
                    {
                        Projectile.velocity += Projectile.rotation.ToRotationVector2() * 8f;
                        for (int i = 0; i < 4; i++)
                        {
                            spawnParticle(Projectile.Center + Projectile.velocity * ((float)i / 4f));
                        }
                    }
                    else
                    {
                        Projectile.rotation = Util.Util.rotatedToAngle(Projectile.rotation, (target.Center - Projectile.Center).ToRotation(), 0.34f, false);
                        Projectile.rotation = Util.Util.rotatedToAngle(Projectile.rotation, (target.Center - Projectile.Center).ToRotation(), 1.4f);
                    }
                    Projectile.velocity *= 0.92f;
                }
            }
            else
            {
                Vector2 standPos = player.Center + new Vector2(0, -140);
                float dist = Util.Util.getDistance(Projectile.Center, standPos);
                if (dist > 100)
                {
                    Projectile.velocity += (standPos - Projectile.Center).SafeNormalize(Vector2.Zero);
                    if (dist > 160)
                    {
                        Projectile.velocity *= 0.982f;
                    }
                }
                Projectile.rotation = Util.Util.rotatedToAngle(Projectile.rotation, Projectile.velocity.ToRotation(), 0.12f, false);
            }
            Projectile.pushByOther(1);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[1] > 5)
            {
                Projectile.ai[1] = 5;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (stickOnNPC)
            {
                modifiers.SourceDamage /= 3;
            }
        }
    }
}

