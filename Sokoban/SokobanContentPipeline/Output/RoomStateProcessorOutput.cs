﻿using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SokobanGame;

namespace SokobanContentPipeline.Output
{
    public class RoomStateProcessorOutput
    {
        public IntVec PlayerPosition { get; set; }

        public EntityProcessorOutput[] Entities { get; set; }

        public void WriteToOutput(ContentWriter output)
        {
            output.Write(PlayerPosition.X);
            output.Write(PlayerPosition.Y);
            
            output.Write(Entities.Length);
            for (int i = 0; i < Entities.Length; i++)
            {
                Entities[i].WriteToOutput(output);
            }
        }
    }
}
