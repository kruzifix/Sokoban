using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame.Input;

namespace SokobanGame.Screen
{
    public class FinishedScreen : Screen
    {
        private int finishedLevel;
        private int moveCount;

        private SpriteFont font;

        public FinishedScreen(int finishedLevel)
            : base(false, true)
        {
            this.finishedLevel = finishedLevel;
            this.moveCount = Assets.Levels[finishedLevel].Room.Moves;

            font = Assets.SpacePortFont;
        }

        public override void Draw(GameTime gameTime)
        {
            int width = SokobanGame.Width;
            int height = SokobanGame.Height;
            Color bgCol = new Color(51, 51, 51, 200);
            sb.DrawRect(width * 0.5f, height * 0.5f, 520, 220, Color.DarkOliveGreen, Align.Center);
            sb.DrawRect(width * 0.5f, height * 0.5f, 500, 200, bgCol, Align.Center);
            sb.DrawRect(width * 0.5f, height * 0.5f - 75, 500, 50, Color.DarkGray, Align.Center);

            string msg = "Level finished";
            sb.Begin();
            Color ggray = new Color(51, 51, 51, 255);
            sb.DrawString(font, msg, new Vector2(width * 0.5f, height * 0.5f - 75), Color.GreenYellow, Align.Center);
            sb.DrawString(font, moveCount + " Moves", new Vector2(width * 0.5f, height * 0.5f - 25), Color.LightGray, Align.Center);
            sb.DrawString(font, "ESC - back to Level-Select", new Vector2(width * 0.5f, height * 0.5f + 20), Color.LightGray, Align.Center);
            sb.DrawString(font, "ENTER - next Level", new Vector2(width * 0.5f, height * 0.5f + 65), Color.LightGray, Align.Center);
            sb.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Pressed("back"))
            {
                // HACK
                // TODO: add function to screenmanager to remove screens until specific is found
                ScreenManager.RemoveScreen(); // finished
                ScreenManager.RemoveScreen(); // gamescreen
                (ScreenManager.TopScreen as LevelSelectScreen).SelectedLevel = finishedLevel;
            }

            if (InputManager.Pressed("confirm"))
            {
                ScreenManager.RemoveScreen(); // finished
                ScreenManager.RemoveScreen(); // gamescreen
                int newLevel = (finishedLevel + 1) % Assets.Levels.Length;
                (ScreenManager.TopScreen as LevelSelectScreen).SelectedLevel = newLevel;
                ScreenManager.AddScreen(new GameScreen(newLevel));
            }

            if (InputManager.Pressed("reset"))
            {
                ScreenManager.RemoveScreen();
                Assets.Levels[finishedLevel].Room.Reset();
            }

            if (InputManager.Pressed("undo"))
            {
                ScreenManager.RemoveScreen();
                Assets.Levels[finishedLevel].Room.Undo();
            }
        }
    }
}
