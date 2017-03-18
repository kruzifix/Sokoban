using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SokobanGame;

namespace SokobanContentPipeline
{
    [ContentTypeWriter]
    public class TiledMapWriter : ContentTypeWriter<TiledMapProcessorOutput>
    {
        protected override void Write(ContentWriter output, TiledMapProcessorOutput value)
        {
            output.Write(value.Width);
            output.Write(value.Height);
            output.Write(value.TileWidth);
            output.Write(value.TileHeight);

            output.Write(value.TileSetPath);

            // --- Layers ---
            output.Write(value.Layers.Count);
            foreach (var layer in value.Layers)
            {
                output.Write(layer.Name);
                output.Write(layer.Width);
                output.Write(layer.Height);
                for (int j = 0; j < layer.Height; j++)
                {
                    for (int i = 0; i < layer.Width; i++)
                    {
                        output.Write(layer.Data[i, j]);
                    }
                }
            }
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(TiledMap).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Sokoban.TiledMapReader, Sokoban, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null";
        }
    }
}
