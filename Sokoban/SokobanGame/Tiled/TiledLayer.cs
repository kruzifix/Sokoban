// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SokobanGame.Tiled
{
    public class TiledLayer
    {
        private SpriteBatch sb;

        private TiledMap map;

        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int[,] Data { get; private set; }
        
        public TiledLayer(TiledMap map, string name, int width, int height)
        {
            sb = SokobanGame.Instance.SpriteBatch;

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

        public void Draw(IntVec offset)
        {
            Rectangle dest = new Rectangle(0, 0, map.TileWidth, map.TileHeight);

            sb.Begin();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int dat = Data[x, y];
                    if (dat > 0)
                    {
                        dest.X = x * map.TileWidth + offset.X;
                        dest.Y = y * map.TileHeight + offset.Y;
                        sb.Draw(map.Tileset.Texture, dest, map.Tileset.GetSourceRect(dat - 1), Color.White);
                    }
                }
            }
            sb.End();
        }
    }
}
