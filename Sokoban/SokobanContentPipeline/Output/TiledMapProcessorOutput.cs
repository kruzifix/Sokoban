using System.Collections.Generic;

namespace SokobanContentPipeline
{
    public class TiledMapProcessorOutput
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public string TileSetPath { get; set; }

        public List<TiledMapLayerOutput> Layers { get; private set; }

        public TiledMapProcessorOutput()
        {
            Layers = new List<TiledMapLayerOutput>();
        }
    }
}
