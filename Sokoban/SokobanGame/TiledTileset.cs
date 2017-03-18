using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame
{
    public class TiledTileset
    {
        public string Name { get; private set; }

        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int TileCount { get; private set; }
        public int Columns { get; private set; }

        public Texture2D Texture { get; private set; }

        private Rectangle[] tiles;

        public TiledTileset(string name, int tileWidth, int tileHeight, int tileCount, int columns, Texture2D texture)
        {
            Name = name;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            TileCount = tileCount;
            Columns = columns;
            Texture = texture;

            GenerateTiles();
        }

        private void GenerateTiles()
        {
            tiles = new Rectangle[TileCount];
            for (int i = 0; i < TileCount; i++)
            {
                int x = i % Columns;
                int y = i / Columns;
                tiles[i] = new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
            }
        }

        public Rectangle GetSourceRect(int i)
        {
            return tiles[i];
        }
    }
}
