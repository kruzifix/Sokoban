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
        public string WallLayerName { get; set; } = "Walls";

        public int PlayerId { get; set; } = 72;
        public int BoxId { get; set; } = 6;
        public int SwitchId { get; set; } = 24;

        public override TiledMapProcessorOutput Process(TiledMapFile input, ContentProcessorContext context)
        {
            try
            {
                context.Logger.LogMessage("Processing TiledMap ...");

                context.Logger.LogMessage("Tileset Name: {0}", input.TileSet.Name);
                context.Logger.LogMessage("Layer Count: {0}", input.Layers.Count);

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
                    Room = new RoomProcessorOutput(input.Width, input.Height)
                };
                output.Room.InitialState = new RoomStateProcessorOutput();
                
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

                    if (layer.Name.Equals(WallLayerName))
                    {
                        context.Logger.LogMessage("Copying Walls to Room.");
                        // copy walls
                        for (int i = 0; i < output.Width; i++)
                        {
                            for (int j = 0; j < output.Height; j++)
                            {
                                if (layer.Data[i, j] > 0)
                                    output.Room.Walls[i, j] = 1;
                            }
                        }
                        output.Layers.Add(layer);
                    }
                    else if (layer.Name.Equals(ObjectLayerName))
                    {
                        context.Logger.LogMessage("Parsing initial room state.");

                        List<IntVec> switches = new List<IntVec>();
                        List<IntVec> boxes = new List<IntVec>();
                        for (int i = 0; i < output.Width; i++)
                        {
                            for (int j = 0; j < output.Height; j++)
                            {
                                if (layer.Data[i, j] - 1 == PlayerId)
                                    output.Room.InitialState.PlayerPosition = new IntVec(i, j);
                                else if (layer.Data[i, j] - 1 == SwitchId)
                                    switches.Add(new IntVec(i, j));
                                else if (layer.Data[i, j] - 1 == BoxId)
                                    boxes.Add(new IntVec(i, j));
                            }
                        }
                        output.Room.InitialState.Switches = switches.ToArray();
                        output.Room.InitialState.Boxes = boxes.ToArray();

                        context.Logger.LogMessage("\tPlayerPosition: {0}", output.Room.InitialState.PlayerPosition);
                        context.Logger.LogMessage("\tSwitches: {0}", output.Room.InitialState.Switches.Length);
                        context.Logger.LogMessage("\tBoxes: {0}", output.Room.InitialState.Boxes.Length);
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
