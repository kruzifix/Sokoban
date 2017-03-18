using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SokobanGame;

namespace SokobanContentPipeline
{
    [ContentTypeWriter]
    public class TiledMapWriter : ContentTypeWriter<TiledMap>
    {
        protected override void Write(ContentWriter output, TiledMap value)
        {
            output.Write(value.Width);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(TiledMap).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Sokoban.TiledMapReader, Sokoban, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null";
            //return typeof(TiledMapReader).AssemblyQualifiedName;
        }
    }
}
