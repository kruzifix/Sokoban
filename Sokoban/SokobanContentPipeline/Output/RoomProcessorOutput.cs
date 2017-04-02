using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SokobanGame;

namespace SokobanContentPipeline.Output
{
    public class RoomProcessorOutput
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[,] Walls { get; set; }
        public IntVec[] Switches { get; set; }

        // TODO: encapsulate in own data structure!!
        public IntVec[] TeleporterPos { get; set; }
        public IntVec[] TeleporterTarget { get; set; }

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
                output.Write(Switches[i]);
            }

            output.Write(TeleporterPos.Length);
            for (int i = 0; i < TeleporterPos.Length; i++)
            {
                output.Write(TeleporterPos[i]);
                output.Write(TeleporterTarget[i]);
            }

            output.Write(Width);
            output.Write(Height);
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    output.Write(Walls[i, j]);
                }
            }
        }
    }
}
