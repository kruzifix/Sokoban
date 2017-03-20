using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace SokobanContentPipeline.Output
{
    public class TiledMapLayerOutput
    {
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int[,] Data { get; private set; }

        public int this[int i] { set { Data[i % Width, i / Width] = value; } }

        public TiledMapLayerOutput(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
            Data = new int[width, height];
        }

        public void WriteToOutput(ContentWriter output)
        {
            output.Write(Name);
            output.Write(Width);
            output.Write(Height);
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    output.Write(Data[i, j]);
                }
            }
        }
    }
}