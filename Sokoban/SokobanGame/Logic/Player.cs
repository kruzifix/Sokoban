namespace SokobanGame.Logic
{
    public class Player : Entity
    {
        public override Entity Copy()
        {
            return new Player() { Pos = this.Pos };
        }
    }
}
