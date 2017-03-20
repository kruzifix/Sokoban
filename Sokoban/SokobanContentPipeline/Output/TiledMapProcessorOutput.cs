using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace SokobanContentPipeline.Output
{
    public class TiledMapProcessorOutput
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public TiledMapTilesetOutput Tileset { get; set; }

        public List<TiledMapLayerOutput> Layers { get; private set; }

        public RoomProcessorOutput Room { get; set; }

        public TiledMapProcessorOutput()
        {
            Layers = new List<TiledMapLayerOutput>();
        }

        public void WriteToOutput(ContentWriter output)
        {
            output.Write(Width);
            output.Write(Height);
            output.Write(TileWidth);
            output.Write(TileHeight);

            Tileset.WriteToOutput(output);

            Room.WriteToOutput(output);

            output.Write(Layers.Count);

            foreach (var layer in Layers)
            {
                layer.WriteToOutput(output);
            }
        }
    }
}
