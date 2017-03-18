using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using SokobanGame;

namespace SokobanContentPipeline
{
    [ContentProcessor(DisplayName = "TiledMap Processor - Sokoban")]
    public class TiledMapProcessor : ContentProcessor<TiledMapFile, TiledMap>
    {
        public override TiledMap Process(TiledMapFile input, ContentProcessorContext context)
        {
            try
            {
                context.Logger.LogMessage("Processing TiledMap ...");

                return new TiledMap(input.Width);
            }
            catch (Exception ex)
            {
                context.Logger.LogMessage("Error processing TiledMap: {0}", ex);
                throw;
            }
        }
    }
}
