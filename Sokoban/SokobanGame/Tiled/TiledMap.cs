using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame.Logic;

namespace SokobanGame.Tiled
{
    public class TiledMap
    {
        private SpriteBatch sb;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        public int PixelWidth { get { return Width * TileWidth; } }
        public int PixelHeight { get { return Height * TileHeight; } }

        public IntVec RenderOffset { get; set; }

        public TiledTileset Tileset { get; private set; }
        private List<TiledLayer> layers;

        public Room Room { get; private set; }

        public Dictionary<string, string> Properties { get; private set; }

        public TiledMap(int width, int height, int tileWidth, int tileHeight, TiledTileset tileset, Room room)
        {
            sb = SokobanGame.Instance.SpriteBatch;

            Width = width;
            Height = height;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            RenderOffset = new IntVec();

            Tileset = tileset;
            layers = new List<TiledLayer>();

            Room = room;

            Properties = new Dictionary<string, string>();
        }

        public void SetTileSize(int tileWidth, int tileHeight)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        public void AddLayer(TiledLayer layer)
        {
            layers.Add(layer);
        }

        public void Draw()
        {
            foreach (var layer in layers)
            {
                layer.Draw(RenderOffset);
            }
            
            // TODO: replace with teleporter tiles!!
            Color[] teleporterColors = new Color[] {
                Color.SlateBlue,
                Color.Magenta
            };

            sb.Begin();
            Rectangle dest = new Rectangle(0, 0, TileWidth, TileHeight);
            int col = 0;
            foreach (var t in Room.Teleporters)
            {
                dest.X = t.Pos.X * TileWidth + RenderOffset.X;
                dest.Y = t.Pos.Y * TileHeight + RenderOffset.Y;
                sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(39), teleporterColors[col]);

                dest.X = t.Target.X * TileWidth + RenderOffset.X;
                dest.Y = t.Target.Y * TileHeight + RenderOffset.Y;
                sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(44), teleporterColors[col]);

                col++;
            }
            sb.End();

            DrawRoomState(Room.CurrentState);
        }

        public void DrawDebug()
        {
            sb.Begin();
            Rectangle dest = new Rectangle(0, 0, TileWidth, TileHeight);
            // draw walls for debugging
            for (int i = 0; i < Room.Width; i++)
            {
                for (int j = 0; j < Room.Height; j++)
                {
                    if (Room.GetWall(i, j) == 0)
                        continue;

                    dest.X = i * TileWidth + RenderOffset.X;
                    dest.Y = j * TileHeight + RenderOffset.Y;
                    sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(41), Color.White);
                }
            }
            foreach (var e in Room.CurrentState.Entities)
            {
                string txt = e.Pos.ToString();
                Vector2 size = Assets.DebugFont.MeasureString(txt);

                Vector2 pos = new Vector2(e.Pos.X * TileWidth, e.Pos.Y * TileHeight) + RenderOffset.ToVector2();
                pos.X += (TileWidth - size.X) * 0.5f;
                pos.Y += 10;

                sb.DrawString(Assets.DebugFont, txt, pos, Color.White);
                if (e is Hole)
                {
                    Hole h = e as Hole;
                    txt = h.Filled.ToString();
                    size = Assets.DebugFont.MeasureString(txt);

                    pos = new Vector2(e.Pos.X * TileWidth, e.Pos.Y * TileHeight) + RenderOffset.ToVector2();
                    pos.X += (TileWidth - size.X) * 0.5f;
                    pos.Y += 30;

                    sb.DrawString(Assets.DebugFont, txt, pos, Color.White);
                }
            }
            sb.End();
        }

        private void DrawRoomState(RoomState rs)
        {
            sb.Begin();
            
            Rectangle dest = new Rectangle(0, 0, TileWidth, TileHeight);

            foreach (Hole h in rs.Holes)
            {
                dest.X = h.Pos.X * TileWidth + RenderOffset.X;
                dest.Y = h.Pos.Y * TileHeight + RenderOffset.Y;
                sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(h.Filled ? 58 : 11), Color.White);
            }
            
            int boxPad = 0;
            dest.Width = TileWidth - boxPad * 2;
            dest.Height = TileHeight - boxPad * 2;

            foreach (Box b in rs.Boxes)
            {
                bool boxOnSwitch = Room.OnSwitch(b.Pos);

                dest.X = b.Pos.X * TileWidth + RenderOffset.X + boxPad;
                dest.Y = b.Pos.Y * TileHeight + RenderOffset.Y + boxPad;
                sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(boxOnSwitch ? 19 : 6), Color.White);
            }

            foreach (StickyBox b in rs.StickyBoxes)
            {
                bool boxOnSwitch = Room.OnSwitch(b.Pos);

                dest.X = b.Pos.X * TileWidth + RenderOffset.X + boxPad;
                dest.Y = b.Pos.Y * TileHeight + RenderOffset.Y + boxPad;
                sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(boxOnSwitch ? 22: 9), Color.White);
            }

            dest.Width = TileWidth;
            dest.Height = TileHeight;
            dest.X = rs.PlayerPosition.X * TileWidth + RenderOffset.X;
            dest.Y = rs.PlayerPosition.Y * TileHeight + RenderOffset.Y;
            sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(65), Color.White);
            
            sb.End();
        }
    }
}
