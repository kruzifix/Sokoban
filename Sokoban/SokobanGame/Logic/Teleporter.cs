namespace SokobanGame.Logic
{
    public class Teleporter
    {
        public IntVec Pos { get; private set; }
        public IntVec Target { get; private set; }

        public Teleporter(IntVec pos, IntVec target)
        {
            Pos = pos;
            Target = target;
        }
    }
}