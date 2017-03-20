using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace SokobanContentPipeline.Output
{
    public class TiledMapTilesetOutput
    {
        public string Name { get; set; }
        public string TilesetPath { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TileCount { get; set; }
        public int Columns { get; set; }

        public void WriteToOutput(ContentWriter output)
        {
            output.Write(Name);
            output.Write(TileWidth);
            output.Write(TileHeight);
            output.Write(TileCount);
            output.Write(Columns);
            output.Write(TilesetPath);
        }
    }
}