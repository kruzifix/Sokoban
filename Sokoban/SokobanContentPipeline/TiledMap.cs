using System;
using System.Xml.Serialization;

namespace SokobanContentPipeline
{
    [Serializable]
    [XmlRoot("map")]
    public class TiledMap
    {
        [XmlAttribute("width")]
        public int Width { get; set; }
    }
}