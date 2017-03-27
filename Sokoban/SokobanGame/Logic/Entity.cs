namespace SokobanGame.Logic
{
    public abstract class Entity
    {
        public IntVec Pos { get; set; }

        public abstract Entity Copy();
    }
}
