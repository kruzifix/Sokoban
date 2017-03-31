namespace SokobanGame.Logic
{
    public class StickyBox : Entity
    {
        public override Entity Copy()
        {
            return new StickyBox() { Pos = this.Pos };
        }
    }
}
