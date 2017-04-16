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
                new Color(63, 168, 229),
                Color.LemonChiffon,
                Color.IndianRed
            };

            sb.Begin();
            Rectangle dest = new Rectangle(0, 0, TileWidth, TileHeight);
            int col = 0;
            foreach (var t in Room.Teleporters)
            {
                Vector2 pos = t.Pos.ToVector2();
                pos.X = (pos.X + 0.5f) * TileWidth + RenderOffset.X;
                pos.Y = (pos.Y + 0.5f) * TileHeight + RenderOffset.Y;

                Vector2 target = t.Target.ToVector2();
                target.X = (target.X + 0.5f) * TileWidth + RenderOffset.X;
                target.Y = (target.Y + 0.5f) * TileHeight + RenderOffset.Y;

                Vector2 dir = target - pos;
                dir.Normalize();
                double angle = Math.Atan2(dir.Y, dir.X) + Math.PI * 0.5;

                dest.X = (int)pos.X;
                dest.Y = (int)pos.Y;

                Rectangle srcRect = Tileset.GetSourceRect(37);

                sb.Draw(Tileset.Texture, dest, srcRect, teleporterColors[col],
                        (float)angle, new Vector2(srcRect.Width/2,srcRect.Height/2), SpriteEffects.None, 0);
                
                dest.X = t.Target.X * TileWidth + RenderOffset.X;
                dest.Y = t.Target.Y * TileHeight + RenderOffset.Y;
                sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(38), teleporterColors[col]);

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
                    if (Room.GetObject(i, j) == FieldObject.Empty)
                        continue;

                    dest.X = i * TileWidth + RenderOffset.X;
                    dest.Y = j * TileHeight + RenderOffset.Y;
                    int id = 41;
                    if (Room.GetObject(i, j) == FieldObject.IceGround)
                        id = 42;
                    sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(id), Color.White);
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

            sb.End();
        }
    }
}
