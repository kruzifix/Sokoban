﻿using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SokobanGame;

namespace SokobanContentPipeline.Output
{
    public class RoomProcessorOutput
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[,] Walls { get; set; }
        public IntVec[] Switches { get; set; }

        public RoomStateProcessorOutput InitialState { get; set; }

        public RoomProcessorOutput(int width, int height)
        {
            Width = width;
            Height = height;
            Walls = new int[Width, Height];
        }

        public void WriteToOutput(ContentWriter output)
        {
            InitialState.WriteToOutput(output);

            output.Write(Switches.Length);
            for (int i = 0; i < Switches.Length; i++)
            {
                output.Write(Switches[i].X);
                output.Write(Switches[i].Y);
            }

            output.Write(Width);
            output.Write(Height);
            for (int j = 0; j < Height; j++)
            {
                for(int i = 0; i < Width; i++)
                {
                    output.Write(Walls[i, j]);
                }
            }
        }
    }
}
