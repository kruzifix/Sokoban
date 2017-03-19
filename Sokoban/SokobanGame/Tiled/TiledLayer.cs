using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame.Tiled
{
    public class TiledLayer
    {
        private TiledMap map;

        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int[,] Data { get; private set; }
        
        public TiledLayer(TiledMap map, string name, int width, int height)
        {
            this.map = map;
            Name = name;
            Width = width;
            Height = height;

            Data = new int[width, height];
        }

        public void SetData(int i, int value)
        {
            SetData(i % Width, i / Width, value);
        }

        public void SetData(int x, int y, int value)
        {
            Data[x, y] = value;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int dat = Data[x, y];
                    if (dat > 0)
                    {
                        Rectangle dest = new Rectangle(x * map.TileWidth, y * map.TileHeight, map.TileWidth, map.TileHeight);
                        batch.Draw(map.Tileset.Texture, dest, map.Tileset.GetSourceRect(dat - 1), Color.White);
                    }
                }
            }
            batch.End();
        }
    }
}
