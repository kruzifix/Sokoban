namespace SokobanContentPipeline
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
    }
}