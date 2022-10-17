using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using DarknessFallenMod.Utils;

namespace DarknessFallenMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class MagmiteChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(
                "8% increased damage"
                );
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 2000;
            Item.rare = ItemRarityID.Red;
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.08f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.MagmiteBar>(), 20)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            var fx = Filters.Scene["Shader"].GetShader().Shader;

            var tex = TextureAssets.Item[Type].Value;

            fx.Parameters["sampleTexture"].SetValue(tex);
            fx.Parameters["time"].SetValue(Main.GameUpdateCount * 0.1f);

            spriteBatch.End();
            spriteBatch.BeginShader(fx);
            
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            spriteBatch.End();
            spriteBatch.BeginDefault();
        }
    }
}
