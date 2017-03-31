using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SokobanGame;

namespace SokobanContentPipeline.Output
{
    public class EntityProcessorOutput
    {
        public string Type { get; set; }
        public IntVec Pos { get; set; }

        public void WriteToOutput(ContentWriter output)
        {
            output.Write(Type);
            output.Write(Pos);
        }
    }
}
