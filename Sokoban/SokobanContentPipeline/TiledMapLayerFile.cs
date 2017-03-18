using System;
using System.Xml.Serialization;

namespace SokobanContentPipeline
{
    [Serializable]
    public class TiledMapLayerFile
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        [XmlElement("data")]
        public TiledMapLayerDataFile Data { get; set; }
    }
    
    [Serializable]
    public class TiledMapLayerDataFile
    {
        [XmlAttribute("encoding")]
        public string Encoding { get; set; }
     
        [XmlText]
        public string Content { get; set; }   
    }
}