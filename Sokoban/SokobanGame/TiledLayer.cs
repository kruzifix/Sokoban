using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanGame
{
    public class TiledLayer
    {
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int[,] Data { get; private set; }

        public TiledLayer(string name, int width, int height)
        {
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
    }
}
