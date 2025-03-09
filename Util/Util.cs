using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using CalamityEntropy;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using NATUPNPLib;
using System.Linq;
using CalamityEntropy.Common;
using Terraria.Audio;
using Terraria.GameContent;

namespace CalamityEntropy.Util
{
    public static class Util {
        public static Texture2D pixelTex => ModContent.Request<Texture2D>("CalamityEntropy/Assets/Extra/white").Value;
        public static Texture2D getTexture(this Projectile p)
        {
            return TextureAssets.Projectile[p.type].Value;
        }
        public static Vector2 normalize(this Vector2 v)
        {
            return v.SafeNormalize(Vector2.Zero);
        }

        public static void DrawGlow(Vector2 worldPos, Color color, float scale, bool additive = true)
        {
            Texture2D glow = getExtraTex("Glow2");
            SpriteBatch sb = Main.spriteBatch;
            var blend = sb.GraphicsDevice.BlendState;
            var sample = sb.GraphicsDevice.SamplerStates[0];
            var depth = sb.GraphicsDevice.DepthStencilState;
            var rasterizer = sb.GraphicsDevice.RasterizerState;
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, additive ? BlendState.Additive : BlendState.NonPremultiplied, sample, depth, rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
            sb.Draw(glow, worldPos - Main.screenPosition, null, color, 0, glow.Size() * 0.5f, scale * 0.4f, SpriteEffects.None, 0);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, blend, sample, depth, rasterizer, null, Main.GameViewMatrix.ZoomMatrix);

        }

        public static Terraria.DataStructures.DrawData getDrawData(this Projectile projectile, Color color)
        {
            Texture2D tx = projectile.getTexture();
            return new Terraria.DataStructures.DrawData(tx, projectile.Center + projectile.gfxOffY * Vector2.UnitY - Main.screenPosition, null, color, projectile.rotation, tx.Size() / 2, projectile.scale, SpriteEffects.None);
        }
        public static void showItemTooltip(Item item)
        {
            Main.HoverItem = item.Clone();
            Main.hoverItemName = item.HoverName;
        }
        public static void SyncProj(int proj)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
            }
        }
        public static void pushByOther(this Projectile proj, float strength)
        {
            foreach(Projectile p in Main.ActiveProjectiles)
            {
                if(p.type == proj.type && p.Colliding(p.getRect(), proj.getRect()) && !(p.whoAmI == proj.whoAmI))
                {
                    proj.velocity += (proj.Center - p.Center).SafeNormalize(randomRot().ToRotationVector2()) * strength;
                }
            }
        }
        public static Vector2 randomVec(float max)
        {
            return new Vector2(Main.rand.NextFloat(-max, max), Main.rand.NextFloat(-max, max));
        }
        public static Vector2 Bezier(List<Vector2> points, float lerp)
        {
            if (points == null || points.Count == 0)
                return Vector2.Zero;

            if (points.Count == 1)
                return points[0];

            List<Vector2> newPoints = new List<Vector2>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                newPoints.Add(Vector2.Lerp(points[i], points[i + 1], lerp));
            }

            return Bezier(newPoints, lerp);
        }

        public static Texture2D getTexture(this NPC npc)
        {
            return TextureAssets.Npc[npc.type].Value;
        }
        public static float GetAngleBetweenVectors(Vector2 vector1, Vector2 vector2)
        {
                         float dotProduct = Vector2.Dot(vector1, vector2);

            float magnitude1 = vector1.Length();
            float magnitude2 = vector2.Length();

            float cosTheta = dotProduct / (magnitude1 * magnitude2);

            float angleInRadians = (float)Math.Acos(cosTheta);


            return angleInRadians;
        }
        public static Vector2 GetSymmetryPoint(this Vector2 point, Vector2 linePoint1, Vector2 linePoint2)
        {
            return point.ClosestPointOnLine(linePoint1, linePoint2) + point.ClosestPointOnLine(linePoint1, linePoint2) - point;
        }
        public static Rectangle getRectCentered(this Vector2 center, float w, float h)
        {
            return new Rectangle((int)(center.X - w / 2), (int)(center.Y - h / 2), (int)w, (int)h);
        }
        public static void DrawLines(List<Vector2> points, Color color, float width)
        {
            for (int i = 1; i < points.Count; i++)
            {
                drawLine(Main.spriteBatch, ModContent.Request<Texture2D>("CalamityEntropy/Assets/Extra/white").Value, points[i - 1], points[i], color, width, 2, true);
            }
        }
        public static SoundStyle GetSound(string name, float pitch = 1, int maxIns = 4, float volume = 1)
        {
            SoundStyle s = new SoundStyle("CalamityEntropy/Assets/Sounds/" + name);
            s.Pitch = pitch - 1;
            s.Volume = volume;
            s.MaxInstances = maxIns;
            return s;
        }
        public static void PlaySound(string name, float pitch = 1, Vector2? pos = null, int maxIns = 2, float volume = 1)
        {
            SoundStyle s = new SoundStyle("CalamityEntropy/Assets/Sounds/" + name);
            s.Pitch = pitch - 1;
            s.Volume = volume;
            s.MaxInstances = maxIns;
            SoundEngine.PlaySound(in s, pos);
        }
        public static void UseSampleState_UI(this SpriteBatch sb, SamplerState sampler)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, sampler, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
        }
        public static void UseBlendState_UI(this SpriteBatch sb, BlendState blend)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, blend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
        }
        public static void UseBlendState(this SpriteBatch sb, BlendState blend, SamplerState s = null)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, blend, s == null ? Main.DefaultSamplerState : s, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
        }
        public static void UseState_UI(this SpriteBatch sb, BlendState blend, SamplerState sampler)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, blend, sampler, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
        }
        public static void begin_(this SpriteBatch sb)
        {
            sb.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
        }
        public static void DrawRectAlt(Rectangle rect, Color color, float width, int num = 16)
        {
            drawLine(new Vector2(rect.X + num, rect.Y), new Vector2(rect.X + rect.Width - num, rect.Y), color, width, 2);
            drawLine(new Vector2(rect.X + rect.Width - num, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + num), color, width, 2);
            drawLine(new Vector2(rect.X + rect.Width, rect.Y + num), new Vector2(rect.X + rect.Width, rect.Y + rect.Height - num), color, width, 2);
            drawLine(new Vector2(rect.X + rect.Width, rect.Y + rect.Height - num), new Vector2(rect.X + rect.Width - num, rect.Y + rect.Height), color, width, 2);
            drawLine(new Vector2(rect.X + num, rect.Y + rect.Height), new Vector2(rect.X + rect.Width - num, rect.Y + rect.Height), color, width, 2);
            drawLine(new Vector2(rect.X + num, rect.Y + rect.Height), new Vector2(rect.X, rect.Y + rect.Height - num), color, width, 2);
            drawLine(new Vector2(rect.X, rect.Y + num), new Vector2(rect.X, rect.Y + rect.Height - num), color, width, 2);
            drawLine(new Vector2(rect.X, rect.Y + num), new Vector2(rect.X + num, rect.Y), color, width, 2);

        }
        public static void recordOldPosAndRots(Projectile p, ref List<Vector2> odp, ref List<float> odr, int maxLength = 12)
        {
            odp.Add(p.Center);
            odr.Add(p.rotation);
            if (odp.Count > maxLength)
            {
                odp.RemoveAt(0);
                odr.RemoveAt(0);
            }
        }
        public static float randomRot()
        {
            return (float)(Main.rand.NextDouble() * MathHelper.Pi * 2);
        }
        public static bool inWorld(int i, int j)
        {
            return !(i < 0 || j < 0 || i > Main.tile.Width || j > Main.tile.Height);
        }
        public static Projectile ToProj_Identity(this int id)
        {
            return Main.projectile.FirstOrDefault(x => x.identity == id);
        }
        public static bool isAir(int i, int j, bool plat = false)
        {
            return isAir(new Vector2(i * 16, j * 16), plat);
        }
        public static bool isAir(Vector2 dp, bool plat = false)
        {
            try
            {
                if (dp.X < 0 || dp.Y < 0 || (int)(dp.X / 16) > Main.tile.Width || (int)(dp.Y / 16) > Main.tile.Height)
                {
                    return true;
                }
                Tile tile = Main.tile[(int)(dp.X / 16), (int)(dp.Y / 16)];
                if (plat)
                {
                    if (tile != null && tile.HasTile)
                    {
                        if (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType])
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (tile != null && tile.HasTile)
                    {
                        if (Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch { return false; }
        }

        public static NPC findTarget(Player player, Projectile proj, int maxDistance, bool check = false)
        {
            NPC target = proj.FindTargetWithinRange(maxDistance, check);
            if (player.MinionAttackTargetNPC >= 0 && player.MinionAttackTargetNPC.ToNPC().active)
            {
                target = player.MinionAttackTargetNPC.ToNPC();
            }
            return target;
        }
        public static Texture2D getExtraTex(string name)
        {
            return ModContent.Request<Texture2D>("CalamityEntropy/Assets/Extra/" + name).Value;
        }
        public static Rectangle GetCutTexRect(Texture2D tex, int count, int index, bool hor = true)
        {
            if (hor)
            {
                return new Rectangle(tex.Width / count * index, 0, tex.Width / count, tex.Height);
            }
            return new Rectangle(0, tex.Height / count * index, tex.Width, tex.Height / count);
        }
        public static void DrawAfterimage(Texture2D tx, List<Vector2> odp, List<float> odr, float scale = 1)
        {
            float ap = 1f / (float)odp.Count;
            for (int i = 0; i < odp.Count; i++)
            {
                Main.spriteBatch.Draw(tx, odp[i] - Main.screenPosition, null, Color.White * ap * 0.5f, odr[i], tx.Size() / 2, scale, SpriteEffects.None, 0);
                ap += 1f / (float)odp.Count;
            }
        }
        public static EGlobalItem Entropy(this Item item)
        {
            return item.GetGlobalItem<EGlobalItem>();
        }
        public static bool IsArmor(Item item, bool vanity = false)
        {
            return (vanity || !item.vanity) && (item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1) && item.maxStack == 1;
        }
        public static EGlobalProjectile Entropy(this Projectile p)
        {
            return p.GetGlobalProjectile<EGlobalProjectile>();
        }
        public static EGlobalNPC Entropy(this NPC npc)
        {
            return npc.GetGlobalNPC<EGlobalNPC>();
        }
        public static NPC ToNPC(this int ins)
        {
            return Main.npc[ins];
        }

        public static EModPlayer Entropy(this Player player)
        {
            if(player.GetModPlayer<EModPlayer>() == null)
            {
                return new EModPlayer();
            }
            return player.GetModPlayer<EModPlayer>();
        }

        public static Player getOwner(this Projectile proj)
        {
            if(proj.owner < 0)
            {
                return null;
            }
            return proj.owner.ToPlayer();
        }
        public static Player ToPlayer(this int ins)
        {
            if(ins < 0 || !Main.player[ins].active)
            {
                return Main.LocalPlayer;
            }
            return Main.player[ins];
        }
        public static Projectile ToProj(this int ins)
        {
            return Main.projectile[ins];
        }
        public static float getRotateAngle(float rNow, float rTo, float rotateSpeed, bool sameSpeed = true)
        {
            float angleNow = MathHelper.ToDegrees(rNow);
            float angleTo = MathHelper.ToDegrees(rTo);
            if (angleNow > 180)
            {
                while (angleNow > 180)
                {
                    angleNow -= 360;
                }
            }
            if (angleNow < -180)
            {
                while (angleNow < -180)
                {
                    angleNow += 360;
                }
            }
            if (angleTo > 180)
            {
                while (angleTo > 180)
                {
                    angleTo -= 360;
                }
            }
            if (angleTo < -180)
            {
                while (angleTo < -180)
                {
                    angleTo += 360;
                }
            }
                         float tz = 0;
            if (Math.Abs(angleNow + 360 - angleTo) < Math.Abs(angleTo - angleNow))
            {
                tz = angleTo - angleNow - 360;
            }
            else
            {
                if (Math.Abs(angleTo + 360 - angleNow) < Math.Abs(angleTo - angleNow))
                {
                    tz = angleTo + 360 - angleNow;
                }
                else
                {
                    tz = angleTo - angleNow;
                }
            }
            if (sameSpeed)
            {
                if (tz > rotateSpeed)
                {
                    tz = rotateSpeed;
                }
                if (tz < (rotateSpeed * -1))
                {
                    tz = rotateSpeed * -1;
                }
            }
            else
            {
                tz *= rotateSpeed;
            }
            return MathHelper.ToRadians(tz);

        }
        public static float rotatedToAngle(float rNow, float rTo, float rotateSpeed, bool sameSpeed=true){
            float angleNow = MathHelper.ToDegrees(rNow);
            float angleTo = MathHelper.ToDegrees(rTo);
            if (angleNow > 180){
                while(angleNow > 180){
                    angleNow -= 360;
                }
            }
            if (angleNow < -180){
                while(angleNow < -180){
                    angleNow += 360;
                }
            }
            if (angleTo > 180){
                while(angleTo > 180){
                    angleTo -= 360;
                }
            }
            if (angleTo < -180){
                while(angleTo < -180){
                    angleTo += 360;
                }
            }
                         float tz = 0;
            if (Math.Abs(angleNow + 360 - angleTo) < Math.Abs(angleTo - angleNow)){
                tz = angleTo - angleNow - 360;
            }else{
                if (Math.Abs(angleTo + 360 - angleNow) < Math.Abs(angleTo - angleNow)){
                    tz = angleTo + 360 - angleNow;
                }else{
                    tz = angleTo - angleNow;
                }
            }
            if(sameSpeed){
                if(tz > rotateSpeed){
                    tz = rotateSpeed;
                }
                if(tz < (rotateSpeed * -1)){
                    tz = rotateSpeed * -1;
                }
            }else{
                tz *= rotateSpeed;
            }return MathHelper.ToRadians(angleNow + tz);

        }
        public static void wormFollow(int npc1, int npc2, int spacing = 48, bool type2 = false, float t2speed = 0.2f, float jrot=0,float angleP = 0f){
            if (type2){
                NPC npc = Main.npc[npc1];
                NPC targetNPC = Main.npc[npc2];
                float rot = npc.rotation - jrot;
                npc.rotation = rotatedToAngle(rot, targetNPC.rotation + angleP - jrot, t2speed, false) + jrot;
                npc.Center = targetNPC.Center;
                Vector2 displacement = (rotatedToAngle(rot, targetNPC.rotation + angleP - jrot, t2speed, false)).ToRotationVector2() * -1 * spacing;
                npc.Center += displacement;
            }else{
                NPC npc = Main.npc[npc1];
                NPC targetNPC = Main.npc[npc2];
                float angle = (float)Math.Atan2(targetNPC.Center.Y - npc.Center.Y, targetNPC.Center.X - npc.Center.X);
                npc.rotation = angle + angleP + jrot;
                npc.Center = targetNPC.Center;
                Vector2 displacement = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * -1 * spacing;
                npc.Center += displacement;
                
            }
        }
        public static void drawChain(Vector2 startPos, Vector2 endPos, int spacing, Texture2D tx, Color color){
            int distance = ((int) Math.Sqrt(Math.Pow(endPos.X - startPos.X, 2) + Math.Pow(endPos.Y - startPos.Y, 2)));
            float rot = (endPos - startPos).ToRotation();
            float px = startPos.X;
            float py = startPos.Y;
            int num = ((int) (distance / spacing));
            Vector2 addVec = new Vector2((endPos.X - startPos.X) / num, (endPos.Y - startPos.Y) / num);
            addVec.Normalize();
            float adx = (endPos.X - startPos.X) / num;
            float ady = (endPos.Y - startPos.Y) / num;
            Vector2 drawPos = new Vector2(px, py);
            for(int i = 0; i <= num; i++){
                Main.EntitySpriteDraw(tx, drawPos - Main.screenPosition, null, color, rot, new Vector2(tx.Width / 2, tx.Height / 2), (new Vector2(1, 1)), SpriteEffects.None, 0);
                drawPos.X += addVec.X * spacing;
                drawPos.Y += addVec.Y * spacing;
            }
        }
        public static void drawChain(Vector2 startPos, Vector2 endPos, int spacing, string texturePath, Color color){
            drawChain(startPos, endPos, spacing, ModContent.Request<Texture2D>(texturePath).Value, color);
        }
        public static void drawChain(Vector2 startPos, Vector2 endPos, int spacing, string texturePath){
            drawChain(startPos, endPos, spacing, ModContent.Request<Texture2D>(texturePath).Value, Color.White);
        }
        public static void drawChain(Vector2 startPos, Vector2 endPos, int spacing, Texture2D texture){
            drawChain(startPos, endPos, spacing, texture, Color.White);
        }
        public static float getDistance(Vector2 v1, Vector2 v2){
            return ((float) Math.Sqrt(Math.Pow(v2.X - v1.X, 2) + Math.Pow(v2.Y - v1.Y, 2)));
        }
        

        public static void drawTexture(Texture2D tex, Vector2 pos, float rotation, Color color, Vector2 scale, SpriteEffects eff=SpriteEffects.None){
            Rectangle rectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            Vector2 origin = rectangle.Size() / 2f;
            Main.spriteBatch.Draw(tex, pos - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle), color, rotation, origin, scale, eff, 0f);
        }
        public static bool LineThroughRect(Vector2 start, Vector2 end, Rectangle rect, int lineWidth = 4, int checkDistance = 8)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(rect.TopLeft(), rect.Size(), start, end, lineWidth, ref point);
        }

        public static void drawLine(SpriteBatch spriteBatch, Texture2D px, Vector2 start, Vector2 end, Color color, float width, int wa = 0, bool worldpos = true)
        {
            spriteBatch.Draw(px, start - (worldpos ? Main.screenPosition : Vector2.Zero), null, color, (end - start).ToRotation(), new Vector2(0, 0.5f), new Vector2(getDistance(start, end) + wa, width), SpriteEffects.None, 0);
        }
        public static void drawLine(Vector2 start, Vector2 end, Color color, float width, int wa = 0, bool worldpos = true)
        {
            Main.spriteBatch.Draw(getExtraTex("white"), start - (worldpos ? Main.screenPosition : Vector2.Zero), null, color, (end - start).ToRotation(), new Vector2(0, 0.5f), new Vector2(getDistance(start, end) + wa, width), SpriteEffects.None, 0);
        }
        public static void drawTextureToPoint(SpriteBatch sb, Texture2D texture, Color color, Vector2 lu, Vector2 ru, Vector2 ld, Vector2 rd) {
            sb.End();

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex> ve = new List<Vertex>();

            ve.Add(new Vertex(lu,
                      new Vector3(0, 0, 1),
                      color));
            ve.Add(new Vertex(ld,
                      new Vector3(0, 1, 1),
                      color));
            ve.Add(new Vertex(ru,
                      new Vector3(1, 0, 1),
                      color));
            ve.Add(new Vertex(rd,
                      new Vector3(1, 1, 1),
                      color));


            GraphicsDevice gd = Main.graphics.GraphicsDevice;
            Texture2D tx = texture;
                gd.Textures[0] = tx;
                gd.DrawUserPrimitives(PrimitiveType.TriangleStrip, ve.ToArray(), 0, ve.Count - 2);

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        }
        public static Vector2 getTxP(List<Vector2> points, float p)
        {
            float dl = 0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                float ds = getDistance(points[i], points[i + 1]);
                if (dl + ds < p)
                {
                    dl += ds;
                }
                else
                {
                    float pc = (p - dl) / ds;
                    return points[i] + (points[i + 1] - points[i]) * pc;
                }
            }

            return points[points.Count - 1];
        }
        public static void drawLaser(SpriteBatch sb, List<Texture2D> txs, List<Vector2> points, int txLength, Color color, int width = 64, int starttx = 0, float startRot = 0)
        {
            for (int j = 0; j < points.Count; j++) {
                points[j] -= Main.screenPosition;
            }
            float dl = 0;
            float al = 0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                al += getDistance(points[i], points[i + 1]);
            }
            int txc = starttx;
            float lr = startRot;             Vector2 tp = Vector2.Zero;
            while (true)
            {
                Texture2D tx = txs[txc % txs.Count];
                if (dl > al)
                {
                    break;
                }
                if (dl + txLength > al)
                {
                    Vector2 dp = getTxP(points, dl);
                    Vector2 de = getTxP(points, dl + txLength);
                    tp = de;
                    float rot = (points[points.Count - 1] - points[points.Count - 2]).ToRotation();
                    Vector2 lrof = lr.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-90)) * width / 2;
                    Vector2 rof = rot.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-90)) * width / 2;
                    drawTextureToPoint(sb, tx, color, dp + lrof, de + rof, dp - lrof, de - rof);
                    lr = rot;
                    dl += txLength;
                    break;
                }
                else
                {
                    Vector2 dp = getTxP(points, dl);
                    Vector2 de = getTxP(points, dl + txLength);
                    tp = de;
                    float rot = (de - dp).ToRotation();
                    Vector2 lrof = lr.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-90)) * width / 2;
                    Vector2 rof = rot.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-90)) * width / 2;
                    drawTextureToPoint(sb, tx, color, dp + lrof, de + rof, dp - lrof, de - rof);
                    lr = rot;
                    dl += txLength;
                }

                txc += 1;
            }
        }

    }
}