// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

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