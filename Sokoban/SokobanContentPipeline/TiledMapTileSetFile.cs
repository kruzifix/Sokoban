using System;
using System.Xml.Serialization;

namespace SokobanContentPipeline
{
    [Serializable]
    public class TiledMapTileSetFile
    {
        [XmlAttribute("firstgid")]
        public int FirstId { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }

        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }

        [XmlAttribute("tilecount")]
        public int TileCount { get; set; }

        [XmlAttribute("columns")]
        public int Columns { get; set; }

        [XmlElement("image")]
        public TiledMapTileSetImageFile Image { get; set; }
    }

    [Serializable]
    public class TiledMapTileSetImageFile
    {
        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }
    }
}