using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.Tools.LightRod
{
    public class LightRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodFishingPole);

            Item.fishingPole = 30;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileID.BobberFiberglass;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float f = 1;
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(source, position, velocity * f, type, 0, 0, player.whoAmI);
                f += 0.08f;
            }
            return false;
        }

        public override void HoldItem(Player player)
        {
            player.accFishingLine = true;
        }
    }
}
