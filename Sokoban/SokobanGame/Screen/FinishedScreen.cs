using Microsoft.Xna.Framework;

namespace SokobanGame.Screen
{
    public class FinishedScreen : Screen
    {
        private int finishedLevel;
        private int moveCount;

        public FinishedScreen(int finishedLevel, int moveCount) 
            : base(false, true)
        {
            this.finishedLevel = finishedLevel;
            this.moveCount = moveCount;
        }

        public override void Draw(GameTime gameTime)
        {
            SokobanGame.Instance.DrawDebugMessage("Finished Screen", new Vector2(10, 10), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
