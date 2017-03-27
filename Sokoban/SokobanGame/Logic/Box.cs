namespace SokobanGame.Logic
{
    public class Box : Entity
    {
        public override Entity Copy()
        {
            return new Box() { Pos = this.Pos };
        }
    }
}
