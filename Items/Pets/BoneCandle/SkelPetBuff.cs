using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System;
using DarknessFallenMod.Items;

namespace DarknessFallenMod.Items.Pets.BoneCandle
{
    public class SkelPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeleton Candle");
            Description.SetDefault("A skull with a candle attached to its head, how did this happen?");

            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;

            int projType = ModContent.ProjectileType<SkelPetProj>();

            // If the player is local, and there hasn't been a pet projectile spawned yet - spawn it.
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0)
            {
                var entitySource = player.GetSource_Buff(buffIndex);

                Projectile.NewProjectile(entitySource, player.Center, Vector2.Zero, projType, 0, 0f, player.whoAmI);
            }
        }
    }
}