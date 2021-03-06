﻿// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

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

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<Property> Properties { get; set; }

        [XmlElement("tileset")]
        public TiledMapTileSetFile TileSet { get; set; }
        
        [XmlElement("layer")]
        public List<TiledMapLayerFile> Layers { get; set; }
    }

    [Serializable]
    public class Property
    {
        [XmlAttribute("name")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}