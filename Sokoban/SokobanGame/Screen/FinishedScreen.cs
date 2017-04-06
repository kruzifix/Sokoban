using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame.Screen
{
    public class FinishedScreen : Screen
    {
        private int finishedLevel;
        private int moveCount;

        public FinishedScreen(int finishedLevel) 
            : base(false, true)
        {
            this.finishedLevel = finishedLevel;
            this.moveCount = Assets.Levels[finishedLevel].Room.Moves;
        }

        public override void Draw(GameTime gameTime)
        {
            string msg = "You did it!";
            
        }

        public override void Update(GameTime gameTime)
        {
            if (KeyPress(Keys.Escape))
            {
                // HACK
                // TODO: add function to screenmanager to remove screens until specific is found
                ScreenManager.RemoveScreen(); // finished
                ScreenManager.RemoveScreen(); // gamescreen
            }

            if (KeyPress(Keys.Enter))
            {
                ScreenManager.RemoveScreen(); // finished
                ScreenManager.RemoveScreen(); // gamescreen
                ScreenManager.AddScreen(new GameScreen( (finishedLevel + 1) % Assets.Levels.Length));
            }

            if (KeyPress(Keys.R))
            {
                ScreenManager.RemoveScreen();
                Assets.Levels[finishedLevel].Room.Reset();
            }

            if (KeyPress(Keys.Z))
            {
                ScreenManager.RemoveScreen();
                Assets.Levels[finishedLevel].Room.Undo();
            }
        }
    }
}
