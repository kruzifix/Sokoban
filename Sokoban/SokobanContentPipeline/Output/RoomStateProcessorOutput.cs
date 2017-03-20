using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SokobanGame;

namespace SokobanContentPipeline.Output
{
    public class RoomStateProcessorOutput
    {
        public IntVec PlayerPosition { get; set; }
        public IntVec[] Switches { get; set; }
        public IntVec[] Boxes { get; set; }

        public void WriteToOutput(ContentWriter output)
        {
            output.Write(PlayerPosition.X);
            output.Write(PlayerPosition.Y);

            output.Write(Switches.Length);
            for (int i = 0; i< Switches.Length; i++)
            {
                output.Write(Switches[i].X);
                output.Write(Switches[i].Y);
            }

            output.Write(Boxes.Length);
            for (int i = 0; i < Boxes.Length; i++)
            {
                output.Write(Boxes[i].X);
                output.Write(Boxes[i].Y);
            }
        }
    }
}
