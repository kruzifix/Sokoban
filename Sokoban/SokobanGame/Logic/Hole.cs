namespace SokobanGame.Logic
{
    public class Hole : Entity
    {
        public bool Filled { get; set; } = false;

        public override Entity Copy()
        {
            return new Hole() { Pos = Pos };
        }
    }
}
