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
            SokobanGame.Instance.DrawDebugMessage("Finished Screen", new Vector2(10, 10), Color.Black);
            
            SokobanGame.Instance.DrawDebugMessage("Press ESC for Level Selection", new Vector2(300, 150), Color.Black);
            SokobanGame.Instance.DrawDebugMessage("Press Z to Undo", new Vector2(300, 170), Color.Black);
            SokobanGame.Instance.DrawDebugMessage("Press R to Reset", new Vector2(300, 190), Color.Black);
            SokobanGame.Instance.DrawDebugMessage("Press ENTER for next Level", new Vector2(300, 210), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            if (KeyPress(Keys.Escape))
            {
                // HACK
                // TODO: add function to screenmanager to remove screens until specific is found
                ScreenManager.RemoveScreen();
                ScreenManager.RemoveScreen();
            }

            if (KeyPress(Keys.Enter))
            {
                ScreenManager.RemoveScreen();
                ScreenManager.RemoveScreen();
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
