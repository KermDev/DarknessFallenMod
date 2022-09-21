using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ObjectData;
using MonoMod.Cil;
using System.Linq;
using Mono.Cecil;

namespace DarknessFallenMod.Tiles.Furniture
{
    public abstract class Statue : ModTile
    {
        public override void Load()
        {
            IL.Terraria.WorldGen.SetupStatueList += IL_AddStatuesToSpawn;
        }

        void IL_AddStatuesToSpawn(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchStsfld(out FieldReference val))) return;

            c.Index--;

            c.EmitDelegate<Func<List<Point16>, List<Point16>>>(list =>
            {
                var newlist = list;
                newlist.Add(new Point16(Type, 0));
                return newlist;
            });
        }

        public virtual int[] CoordinateHeights => new int[3] { 18, 16, 18 };
        public virtual string MapName => "Statue";
        public virtual int StatueItem => 0;
        public virtual int[] NPCToSpawn => new int[] { 0 };

        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 0;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.CoordinateHeights = CoordinateHeights;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(Type);

			var name = CreateMapEntryName();
			name.SetDefault(MapName);
			AddMapEntry(new Color(180, 180, 180), name);
        }

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, StatueItem);

        public override void HitWire(int i, int j)
        {
            Main.NewText("l");
            NPC.NewNPC(null, i * 16, j * 16, Main.rand.NextFromList(NPCToSpawn));
        }
    }
}
