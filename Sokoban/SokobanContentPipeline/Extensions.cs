using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SokobanGame;

namespace SokobanContentPipeline
{
    static class Extensions
    {
        public static void Write(this ContentWriter output, IntVec v)
        {
            output.Write(v.X);
            output.Write(v.Y);
        }
    }
}
