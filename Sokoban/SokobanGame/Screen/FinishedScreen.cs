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

        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
