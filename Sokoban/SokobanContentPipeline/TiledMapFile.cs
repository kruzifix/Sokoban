using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SokobanContentPipeline
{
    [Serializable]
    [XmlRoot("map")]
    public class TiledMapFile
    {
        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }

        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }

        [XmlElement("tileset")]
        public TiledMapTileSetFile TileSet { get; set; }
        
        [XmlElement("layer")]
        public List<TiledMapLayerFile> Layers { get; set; }
    }
}