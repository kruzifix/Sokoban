using System;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace SokobanContentPipeline
{
    [ContentProcessor(DisplayName = "TiledMap Processor - Sokoban")]
    public class TiledMapProcessor : ContentProcessor<TiledMapFile, TiledMapProcessorOutput>
    {
        public override TiledMapProcessorOutput Process(TiledMapFile input, ContentProcessorContext context)
        {
            try
            {
                context.Logger.LogMessage("Processing TiledMap ...");

                context.Logger.LogMessage("Tileset Name: {0}", input.TileSet.Name);
                context.Logger.LogMessage("Layer Count: {0}", input.Layers.Count);
                
                var output = new TiledMapProcessorOutput()
                {
                    Width = input.Width,
                    Height = input.Height,
                    TileWidth = input.TileWidth,
                    TileHeight = input.TileHeight,
                    TileSetPath = input.TileSet.Image.Source.Split('.')[0]
                };
                
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
                    output.Layers.Add(layer);
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
