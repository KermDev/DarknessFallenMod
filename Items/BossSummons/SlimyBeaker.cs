using DarknessFallenMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DarknessFallenMod.Items.BossSummons
{
	public class SlimyBeaker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimy Beaker"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Summons the slime rain");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = 1692;
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
            Item.maxStack = 99;
        }

        public override bool? UseItem(Player player)
        {
			if(!Main.slimeRain)
            {
				Main.StartSlimeRain();
				Main.NewText("Is that slime in the air?", 20, 100, 51);
            }
			return null;
        }
    }
}