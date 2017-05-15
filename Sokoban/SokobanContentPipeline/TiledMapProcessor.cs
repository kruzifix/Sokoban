// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using SokobanContentPipeline.Output;
using SokobanGame;

/*
 * Reference: http://dylanwilson.net/creating-custom-content-importers-for-the-monogame-pipeline
 */

namespace SokobanContentPipeline
{
    [ContentProcessor(DisplayName = "TiledMap Processor - Sokoban")]
    public class TiledMapProcessor : ContentProcessor<TiledMapFile, TiledMapProcessorOutput>
    {
        public string ObjectLayerName { get; set; } = "Objects";
        public string WallSwitchesLayerName { get; set; } = "Walls_Switches";
        public string BackgroundLayerName { get; set; } = "Background";

        public int PlayerId { get; set; } = 8;

        public int BoxId { get; set; } = 11;
        public int StickyBoxId { get; set; } = 19;
        public int HoleId { get; set; } = 27;

        public int WallId { get; set; } = 2;
        public int[] IceGroundIds { get; set; } = { 4, 5, 6, 7, 13, 14, 15, 21, 22, 23, 29, 30, 31 };
        public int SwitchId { get; set; } = 3;

        public override TiledMapProcessorOutput Process(TiledMapFile input, ContentProcessorContext context)
        {
            try
            {
                context.Logger.LogMessage("Processing TiledMap ...");

                context.Logger.LogMessage("Tileset Name: {0}", input.TileSet.Name);
                context.Logger.LogMessage("Layer Count: {0}", input.Layers.Count);

                List<IntVec> telePos = new List<IntVec>();
                List<IntVec> teleTarget = new List<IntVec>();

                foreach (var p in input.Properties)
                {
                    // context.Logger.LogMessage("Property: {0} => {1}", p.Key, p.Value);

                    // parse teleporter stuff from properties
                    if (p.Key == "Teleporters")
                    {
                        context.Logger.LogMessage("\tParsing Teleporters ...");

                        string val = p.Value;
                        if (string.IsNullOrEmpty(val))
                            val = p.Text;
                        if (string.IsNullOrEmpty(val))
                            continue;

                        string[] teles = val.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                        // 2 4 to 11 3
                        for (int i = 0; i < teles.Length; i++)
                        {
                            string[] tokens = teles[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tokens.Length != 5)
                                throw new FormatException("Wrong Teleporter definition format at: " + teles[i]);
                            telePos.Add(new IntVec(int.Parse(tokens[0]), int.Parse(tokens[1])));
                            teleTarget.Add(new IntVec(int.Parse(tokens[3]), int.Parse(tokens[4])));
                        }
                        context.Logger.LogMessage("\tTeleporters: {0}", telePos.Count);
                    }
                }
                
                int lastDot = input.TileSet.Image.Source.LastIndexOf('.');

                // HACK!!!
                string tilesetPath = input.TileSet.Image.Source.Substring(0, lastDot).Replace("../", "");

                var tileset = new TiledMapTilesetOutput()
                {
                    Name = input.TileSet.Name,
                    TileWidth = input.TileSet.TileWidth,
                    TileHeight = input.TileSet.TileHeight,
                    TileCount = input.TileSet.TileCount,
                    Columns = input.TileSet.Columns,
                    TilesetPath = tilesetPath
                };

                var output = new TiledMapProcessorOutput()
                {
                    Width = input.Width,
                    Height = input.Height,
                    TileWidth = input.TileWidth,
                    TileHeight = input.TileHeight,
                    Tileset = tileset,
                    Room = new RoomProcessorOutput(input.Width, input.Height),
                    Properties = input.Properties
                };
                output.Room.InitialState = new RoomStateProcessorOutput();
                
                output.Room.TeleporterPos = telePos.ToArray();
                output.Room.TeleporterTarget = teleTarget.ToArray();
                
                foreach (var layerData in input.Layers)
                {
                    context.Logger.LogMessage("Parsing Layer: {0}", layerData.Name);
                    var layer = new TiledMapLayerOutput(layerData.Name, layerData.Width, layerData.Height);

                    string lines = layerData.Data.Content.Replace(Environment.NewLine, "");
                    string[] tokens = lines.Split(',');
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        layer[i] = int.Parse(tokens[i].Trim());
                    }

                    if (layer.Name.Equals(WallSwitchesLayerName))
                    {
                        context.Logger.LogMessage("Copying Walls and Switches to Room.");
                        // copy walls
                        List<IntVec> switches = new List<IntVec>();
                        for (int i = 0; i < output.Width; i++)
                        {
                            for (int j = 0; j < output.Height; j++)
                            {
                                if (layer.Data[i, j] - 1 == WallId)
                                    output.Room.Walls[i, j] = 1;
                                if (layer.Data[i, j] - 1 == SwitchId)
                                    switches.Add(new IntVec(i, j));
                            }
                        }
                        output.Room.Switches = switches.ToArray();
                        context.Logger.LogMessage("\tSwitches: {0}", output.Room.Switches.Length);
                        output.Layers.Add(layer);
                    }
                    else if (layer.Name.Equals(ObjectLayerName))
                    {
                        context.Logger.LogMessage("Parsing initial room state.");
                        
                        var ents = new List<EntityProcessorOutput>();
                        for (int i = 0; i < output.Width; i++)
                        {
                            for (int j = 0; j < output.Height; j++)
                            {
                                int id = layer.Data[i, j] - 1;
                                if (id == PlayerId)
                                    output.Room.InitialState.PlayerPosition = new IntVec(i, j);
                                else if (id == BoxId)
                                    ents.Add(new EntityProcessorOutput() { Type = "box", Pos = new IntVec(i, j) });
                                else if (id == StickyBoxId)
                                    ents.Add(new EntityProcessorOutput() { Type = "sbox", Pos = new IntVec(i, j) });
                                else if (id == HoleId)
                                    ents.Add(new EntityProcessorOutput() { Type = "hole", Pos = new IntVec(i, j) });
                            }
                        }
                        output.Room.InitialState.Entities = ents.ToArray();

                        context.Logger.LogMessage("\tPlayerPosition: {0}", output.Room.InitialState.PlayerPosition);
                        context.Logger.LogMessage("\tEntities: {0}", output.Room.InitialState.Entities.Length);
                    }
                    else if (layer.Name.Equals(BackgroundLayerName))
                    {
                        context.Logger.LogMessage("Parsing Background Layer for Ice.");
                        for (int i = 0; i < output.Width; i++)
                        {
                            for (int j = 0; j < output.Height; j++)
                            {
                                for (int k = 0; k < IceGroundIds.Length; k++)
                                {
                                    if (layer.Data[i, j] - 1 == IceGroundIds[k])
                                    {
                                        output.Room.Walls[i, j] = 2;
                                        break;
                                    }
                                }
                            }
                        }

                        output.Layers.Add(layer);
                    }
                    else
                    {
                        output.Layers.Add(layer);
                    }
                }
                return output;
            }
            catch (Exception ex)
            {
                context.Logger.LogMessage("Error processing TiledMap: {0}", ex);
                throw;
            }
        }
    }
}
