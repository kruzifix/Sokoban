using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame
{
    public class TiledMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        public TiledTileset Tileset { get; private set; }
        private List<TiledLayer> layers;
        
        public TiledMap(int width, int height, int tileWidth, int tileHeight, TiledTileset tileset)
        {
            Width = width;
            Height = height;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            Tileset = tileset;
            layers = new List<TiledLayer>();
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

        public void Draw(SpriteBatch batch)
        {
            foreach (var layer in layers)
            {
                layer.Draw(batch);
            }
        }
    }
}
