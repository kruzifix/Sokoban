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

        public TiledTileset Tileset { get; private set; }
        private List<TiledLayer> layers;

        public Room Room { get; private set; }

        public TiledMap(int width, int height, int tileWidth, int tileHeight, TiledTileset tileset, Room room)
        {
            sb = SokobanGame.Instance.SpriteBatch;

            Width = width;
            Height = height;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            Tileset = tileset;
            layers = new List<TiledLayer>();

            Room = room;
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
                layer.Draw();
            }
            DrawRoomState(Room.CurrentState);
        }

        private void DrawRoomState(RoomState rs)
        {
            sb.Begin();

            Rectangle dest = new Rectangle(0, 0, TileWidth, TileHeight);

            for (int i = 0; i < rs.Switches.Length; i++)
            {
                dest.X = rs.Switches[i].X * TileWidth;
                dest.Y = rs.Switches[i].Y * TileHeight;
                sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(24), Color.White);
            }

            for (int i = 0; i < rs.Boxes.Length; i++)
            {
                dest.X = rs.Boxes[i].X * TileWidth;
                dest.Y = rs.Boxes[i].Y * TileHeight;
                sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(6), Color.White);
            }

            dest.X = rs.PlayerPosition.X * TileWidth;
            dest.Y = rs.PlayerPosition.Y * TileHeight;
            sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(65), Color.White);

            /*
            // draw walls for debugging
            for (int i = 0; i < Room.Width; i++)
            {
                for (int j = 0; j < Room.Height; j++)
                {
                    if (Room.GetWall(i, j) == 0)
                        continue;

                    dest.X = i * TileWidth;
                    dest.Y = j * TileHeight;
                    sb.Draw(Tileset.Texture, dest, Tileset.GetSourceRect(41), Color.White);
                }
            }
            */

            sb.DrawString(Assets.DebugFont, string.Format("History: {0}", Room.Moves), new Vector2(200, 10), Color.Black);

            sb.End();
        }
    }
}
