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
        
        private Texture2D tilesetTexture;

        public Texture2D TilesetTexture { get { return tilesetTexture; } }
        
        private List<TiledLayer> layers;
        
        public TiledMap(int width, int height, int tileWidth, int tileHeight, Texture2D tilesetTexture)
        {
            Width = width;
            Height = height;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            
            this.tilesetTexture = tilesetTexture;
            layers = new List<TiledLayer>();
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
