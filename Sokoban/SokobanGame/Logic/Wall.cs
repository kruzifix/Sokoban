namespace SokobanGame.Logic
{
    public class Wall : Entity
    {
        public override Entity Copy()
        {
            return new Wall() { Pos = this.Pos };
        }
    }
}
