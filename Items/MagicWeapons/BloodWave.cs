using DarknessFallenMod.Items.MagicWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.MagicWeapons
{
    public class BloodWaveBook : ModItem
    {
        public int BloodWaveProjectile { get; private set; }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Holds the power of Waves of blood");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //this is for journey mode researching;
        }

        public override void SetDefaults()
        {
            Item.damage = 57; //damage;
            Item.DamageType = DamageClass.Magic; //damge class (melee, ranged, amgic, summoner);
            Item.width = 64; //replace with wwidth of book sprite;
            Item.height = 48; //replace with height of book sprite;
            Item.useTime = 20; //duration that the item gets used in frames (1 frame is 1/60 seconds));
            Item.useAnimation = 20; //duration of the animation;
                                    //important note for usetime and useanimations: use time of 12 and a use animation of 24 would mean it is used twice for one animations. this is how the clockwork assult rifle works if i remember correctly;
            Item.useStyle = 5; //style shows how the item is used, 1 is sword swing, 5 is point at mouse, etc.;
            Item.knockBack = 20; //knockback;
            Item.noMelee = true; //doesnt hurt the enemy if they come into contact with the book;
            Item.value = 18352; //sell value, in copper coins;
            Item.rare = 5; //rarity (check wiki for values);
            Item.UseSound = SoundID.Item1; //sound played when used;
            Item.autoReuse = true; //autoswing;
            Item.useTurn = true; //the player cna turn aroudn when using the item (should be enabled for projectile weapoons);
            Item.shoot = ModContent.ProjectileType<BloodWaveProjectile>(); //code that shoots projectile;
            Item.shootSpeed = 13f; //speed of projectile (this may not work);
            Item.mana = 5;//mana requirement;
        }



                    public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SoulofFright, 20);
            recipe.AddIngredient(ItemID.SoulofNight, 20);
            recipe.AddIngredient(ItemID.HellstoneBar, 30);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

