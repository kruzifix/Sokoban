// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

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

        public List<Property> Properties { get; set; }

        public TiledMapTilesetOutput Tileset { get; set; }

        public List<TiledMapLayerOutput> Layers { get; private set; }

        public RoomProcessorOutput Room { get; set; }

        public TiledMapProcessorOutput()
        {
            Properties = new List<Property>();
            Layers = new List<TiledMapLayerOutput>();
        }

        public void WriteToOutput(ContentWriter output)
        {
            output.Write(Width);
            output.Write(Height);
            output.Write(TileWidth);
            output.Write(TileHeight);

            output.Write(Properties.Count);
            foreach (var p in Properties)
            {
                output.Write(p.Key);
                output.Write(p.Value);
            }

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
