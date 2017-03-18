using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame
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

            batch.End();
        }
    }
}
