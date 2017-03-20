using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace SokobanContentPipeline.Output
{
    public class RoomProcessorOutput
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[,] Walls { get; set; }

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
