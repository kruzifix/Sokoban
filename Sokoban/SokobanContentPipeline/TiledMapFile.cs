using System;
using System.Xml.Serialization;

namespace SokobanContentPipeline
{
    [Serializable]
    [XmlRoot("map")]
    public class TiledMapFile
    {
        [XmlAttribute("width")]
        public int Width { get; set; }
    }
}